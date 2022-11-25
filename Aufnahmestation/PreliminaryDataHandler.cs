using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HisDemo.Aufnahmestation
{
    /// <summary>
    /// Simple file-based data handler for extensibility and future data accessibility.
    /// To connect to central database, an extension/replacement of this class is used.
    /// </summary>
    public class PreliminaryDataHandler
    {
        private readonly string storageLocation;
        private readonly string sambaLocation;

        /// <summary>
        /// Track if data was changed after prefill.
        /// </summary>
        private bool ChangedSincePrefill;

        public PreliminaryDataHandler(string storageLocation, string sambaLocation)
        {
            this.storageLocation = storageLocation;
            this.sambaLocation = sambaLocation;
            if (!Directory.Exists(storageLocation))
            {
                Directory.CreateDirectory(storageLocation);
            }

            if (sambaLocation != null && !Directory.Exists(sambaLocation))
            {
                Directory.CreateDirectory(sambaLocation);
            }
        }

        public void ClearDataset()
        {
            values.Clear();
            this.HasChangedSincePrefill = false;
        }

        public void CopyOverValues(PreliminaryDataHandler other, List<string> keyFilter = null)
        {
            // copy over the dictinary values
            foreach (string key in other.values.Keys)
            {
                if (keyFilter.Contains(key))
                    continue;
                values.Add(key, other.values[key]);
            }
        }

        private Dictionary<string, object> values = new Dictionary<string, object>();

        public object GetValue(string key)
        {
            if (values.ContainsKey(key))
                return values[key];
            else
                return null;
        }

        public static bool IsEssentiallyNull(object o)
        {
            return o == null || o.Equals(null) || o.Equals("");
        }

        public void SetValue(string key, object value)
        {
            if (!values.ContainsKey(key))
            {
                if (!IsEssentiallyNull(value))
                    OnChangeValue(key, null, value);
            }
            else if (!Equals(values[key], value) && IsEssentiallyNull(values[key]) != IsEssentiallyNull(value))
            {
                OnChangeValue(key, values[key], value);
            }
            values[key] = value;
        }

        private void OnChangeValue(string key, object oldValue, object newValue)
        {
            ChangedSincePrefill = true;
        }

        public bool HasChangedSincePrefill
        {
            get => ChangedSincePrefill;
            set => ChangedSincePrefill = value;
        }

        public bool IsDatasetExisting(string id, string ending="mcaini")
        {
            Exception x = null;
            for (int retries = 5; retries > 0; retries--)
            {
                try
                {
                    string fileName = Path.Combine(storageLocation, $"{id}.{ending}");
                    string sambaFileName = null;
                    if (sambaLocation != null)
                        sambaFileName = Path.Combine(sambaLocation, $"{id}.{ending}");

                    if (File.Exists(fileName))
                        return true;

                    if (sambaFileName != null && File.Exists(sambaFileName))
                        return true;
                    return false;
                } catch (Exception y)
                {
                    x = y;
                }
            }
            throw x ?? new Exception("Error when checking dataset existence, even after retry loop.");
        }
        public void WriteDataset(string id, string ending="mcaini", List<string> keyFilter=null)
        {
            string fileName = Path.Combine(storageLocation, $"{id}.{ending}");
            string sambaFileName = null;
            if (sambaLocation != null)
                sambaFileName = Path.Combine(sambaLocation, $"{id}.{ending}");
            for (int retries = 5; retries > 0; retries--)
            {
                try
                {
                    if (File.Exists(fileName))
                        throw new DuplicateDatasetException();

                    if (sambaFileName != null && File.Exists(sambaFileName))
                        throw new DuplicateDatasetException();
                }
                catch (Exception x)
                {
                    if (retries == 1)
                    {
                        throw x;
                    }
                }
            }
            
            using (FileStream fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
            {
                var writer = new StreamWriter(fs, Encoding.UTF8);

                writer.WriteLine("[MCAPreliminaryDataset]");
                foreach(string key in values.Keys)
                {
                    if (keyFilter == null || keyFilter.Contains(key))
                    {
                        writer.WriteLine($"{key}={values[key]}");
                    }
                }

                writer.Flush();
                writer.Close();
            }

            File.SetAttributes(fileName, FileAttributes.ReadOnly);

            if (sambaFileName != null)
            {
                File.Copy(fileName, sambaFileName);
                File.SetAttributes(sambaFileName, FileAttributes.ReadOnly);
            }

        }

        public bool ReadDataset(string id, string ending = "mcaini")
        {
            ClearDataset();
            string fileName = Path.Combine(storageLocation, $"{id}.{ending}");

            char[] sep = { '=' };
            try
            {
                using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
                {
                    string head = sr.ReadLine();
                    switch (head)
                    {
                        case "[MCAPreliminaryDataset]":
                            while (!sr.EndOfStream)
                            {
                                string line = sr.ReadLine();
                                if (line.Length == 0) continue;
                                if (line.StartsWith(";")) continue;
                                string[] parts = line.Split(sep, 2);
                                if (parts.Length != 2 || parts[0].Trim().Length <= 0)
                                    throw new Exception("Invalid row format");
                                string key = parts[0].Trim();
                                if (values.ContainsKey(key))
                                    throw new Exception($"Duplicate Key: {key}");
                                values.Add(key, parts[1].Trim());
                            }
                            if (!values.ContainsKey("SpecimenIDTaken"))
                                throw new Exception("Specimen ID Missing!");
                            break;
                        default:
                            throw new Exception("Unknown header.");
                    }

                }
            }
            catch
            {
                ClearDataset();
                return false;
            }
            return true;
        }

        public class DuplicateDatasetException : Exception { }

        public bool HasAnyData
        {
            get
            {
                foreach(var val in values.Values)
                {
                    if (val != null && val?.ToString().Length > 0)
                        return true;
                }
                return false;
            }
        }

    }
}
