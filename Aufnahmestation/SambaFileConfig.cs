using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.CodeDom;

namespace HisDemo.Aufnahmestation
{
    /// <summary>
    /// Preliminary configuration file parser which loads a config from the samba share.
    /// </summary>
    public class SambaFileConfig
    {
        public enum Section
        {
            Undefined,
            PrefillSuggested,
            PrefillLocked,
            AddOrOverwriteOnSave,
            Software
        }

        public class SoftwareFields {
            public string SignatureLocality = "Cityname";
            
            public bool ShowPrinterSettingsDialog = false;
            public bool Failsafe_DoNotEncryptQRCodes = false;
            public bool HideTeachModeWatermarkOnPrint = false;
            
            public bool QRCodeObligation = false;
            
            public bool DeactivatePrint = false;
            public bool AutomaticPrintQrDetection = false;
            public bool DisableConsentPage = false;
        }

        public SoftwareFields Software = new SoftwareFields();

        private Dictionary<Section, Dictionary<string, string>> Fields = FieldsEmpty;

        private void SanityCheckSoftwareFields()
        {
            int exclusive = 0;
            if (Software.DeactivatePrint)
                exclusive++;
            if (Software.AutomaticPrintQrDetection)
                exclusive++;
            if (exclusive > 1)
                throw new Exception(
                    "SambaFileConfig.SanityCheckSoftwareFields Failed: More than one mutually exclusive printer flags set.");
        }

        public string GetField(Section section, string field)
        {
            if (section == Section.Undefined || section == Section.Software)
                throw new ArgumentException("Cannot read software or undefined fields directly.");
            if (field == null)
                return null;
            if (!Fields.ContainsKey(section))
                return null;
            if (!Fields[section].ContainsKey(field))
                return null;
            return Fields[section][field];
        }
        public string[] GetFields(Section section)
        {
            if (section == Section.Undefined || section == Section.Software)
                throw new ArgumentException("Cannot read software or undefined fields directly.");
            if (!Fields.ContainsKey(section))
                return null;
            return Fields[section].Keys.ToArray();
        }

        public static Dictionary<Section, Dictionary<string, string>> FieldsEmpty { 
            get
            {
                var x = new Dictionary<Section, Dictionary<string, string>>();
                x.Add(Section.PrefillSuggested, new Dictionary<string, string>());
                x.Add(Section.PrefillLocked, new Dictionary<string, string>());
                x.Add(Section.AddOrOverwriteOnSave, new Dictionary<string, string>());
                return x;
            } 
        }

        public static void DumpTemplate(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.WriteLine("[MCAPreliminaryConfig]");
                string dbgRel = "Release";
                #if DEBUG
                    dbgRel = "Debug";
                #endif
                sw.WriteLine($"; This is a template. Generated from SW Version {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version} - {dbgRel}");
                sw.WriteLine($"; by User {Environment.UserName} on Machine {Environment.MachineName}");
                sw.WriteLine("[Software]");
                var defaults = new SoftwareFields();
                foreach (var field in typeof(SoftwareFields).GetFields())
                {
                    sw.WriteLine($"; {field.Name} = {field.GetValue(defaults)?.ToString()}");
                }
                var emptys = FieldsEmpty;
                sw.WriteLine("; Warning: PrefillLocked will OVERWRITE any QR-Readback!");
                sw.WriteLine(";          PrefillSuggested will overwrite only empty fields after QR-Readback data.");
                foreach(var key in FieldsEmpty.Keys)
                {
                    sw.WriteLine();
                    sw.WriteLine($"[{key}]");
                }
            }
        }

        public SambaFileConfig(string path, Logger logger = null)
        {
            
            logger?.Diagnostic("Reading SambaConfigFile.");
            if (!File.Exists(path))
            {
                logger?.Warning($"SambaConfigFile {path} does not exist.");
                try
                {
                    path = path + ".template";
                    DumpTemplate(path);
                    logger?.Information($"Dumped template SambaConfigFile to {path}.");
                } catch (Exception x) {
                    logger?.Error($"Could not dump template SambaConfigFile to {path}. {x}");
                }
                throw new Exception("SambaConfigFile does not exist.");
            }
            using (StreamReader sr = new StreamReader(path,Encoding.UTF8, false))
            {
                char[] sep = { '=' };
                Section section = Section.Undefined;
                if (sr.ReadLine() != "[MCAPreliminaryConfig]")
                    throw new Exception("SambaFileConfig: Magic Header wrong.");
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (line.Length < 1 || line.StartsWith(";"))
                        continue;
                    if (line.StartsWith("["))
                    {
                        // Parse section header
                        if (!line.EndsWith("]"))
                            throw new Exception("SambaFileConfig: Section header not correctly terminated.");
                        line = line.Substring(1, line.Length - 2);
                        if (!Enum.TryParse<Section>(line, out section))
                            throw new Exception($"SambaFileConfig: Unknown section {line}.");
                        logger?.Diagnostic($"Found Section {section}.");
                    } else
                    {
                        if (section == Section.Undefined)
                            throw new Exception("SambaFileConfig: Key-Value-Pair in Undefined section.");
                        // Parse Value
                        string[] split = line.Split(sep, 2);
                        string key = split[0].Trim();
                        string value = null;
                        if (split.Length == 2)
                            value = split[1].Trim();
                        if (section == Section.Software)
                        {
                            var field = typeof(SoftwareFields).GetField(key);
                            if (field == null)
                                throw new Exception($"SambaFileConfig: Undefined Software field: {key}");
                            try
                            {
                                var typeCode = Type.GetTypeCode(field.FieldType);
                                switch (typeCode)
                                {
                                    case TypeCode.Boolean:
                                        field.SetValue(Software, bool.Parse(value));
                                        break;
                                    case TypeCode.Int32:
                                        field.SetValue(Software, Int32.Parse(value));
                                        break;
                                    default:
                                        field.SetValue(Software, value);
                                        break;
                                }
                            } catch (Exception x)
                            {
                                throw new Exception($"SambaFileConfig: Unparseable value for software field {key}.", x);
                            }
                        } else
                        {
                            Fields[section].Add(key, value);
                        }
                        logger?.Diagnostic($"Field {section}.{key} = {value}.");
                    }

                }
            }
            logger?.Information("Running SambaConfigFile.SanityCheckSoftwareFields...");
            SanityCheckSoftwareFields();
            logger?.Information("Reading SambaConfigFile done.");
        }
    }
}
