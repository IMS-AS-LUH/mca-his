using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace HisDemo
{
    /// <summary>
    /// File-Based root logging destination.
    /// Can be wrapped in other Logger instances to build a facility hierarchy.
    /// </summary>
    public class FileLogger : Logger, IDisposable
    {
        private readonly DateTime beginOfLog;
        private StreamWriter writer;
        private DateTime? lastTimestamp;
        private string fullFileName;

        private List<string> lineBuffer = new List<string>();
        private int failedWriteAttempts = 0;
        /// <summary>
        /// Inner line writing handler.
        /// This replaces simple writer.WriteLine calls and instead buffers them in the class.
        /// Upon flush, it tries to dump out the lines to the files.
        /// On errors, the writer is tried to close cleanly or just disposed.
        /// The following attempt will try to recreate the stream and dump buffer again, up to 5 times.
        /// If this fails, the buffer is kept and another 5 retries will occur with the next dumping.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="flush"></param>
        private void WriteLineKernel(string line, bool flush = false)
        {
            lineBuffer.Add(line);
            if (flush)
            {
                for (int retries = 5; retries > 0; retries--)
                {
                    try
                    {
                        if (writer == null)
                        {
                            writer = new StreamWriter(fullFileName, true, Encoding.UTF8);
                            writer.WriteLine($"!!! Reopened Log File Stream. Failed Attempts: {failedWriteAttempts}. Line Buffer; {lineBuffer.Count}");
                        }
                        foreach (string x in lineBuffer)
                            writer.WriteLine(x);
                        writer.Flush();
                        lineBuffer.Clear();
                        failedWriteAttempts = 0;
                        return;
                    } 
                    catch
                    {
                        failedWriteAttempts++;
                        try
                        {
                            writer.Close();
                        }
                        catch
                        {
                        }
                        try
                        {
                            writer.Dispose();
                        } 
                        catch
                        {
                        }
                        writer = null;
                        Thread.Sleep(100);
                    }
                }
                
            }
        }

        /// <summary>
        /// Creates or appends a plaintext log file with a timestamped name.
        /// </summary>
        /// <param name="destinationPath">The path (directory) to use for storing log files.</param>
        /// <param name="facility">Optional root facility name to display in the file entries.</param>
        /// <param name="filePrefix">A file prefix representing the application. By default equals facility, or the calling assembly name.</param>
        public FileLogger(string destinationPath, string facility = null, string filePrefix = null, string fileSuffix = null) : base(facility)
        {
            beginOfLog = DateTime.UtcNow;

            Assembly callingAssembly = Assembly.GetCallingAssembly();

            string fileName = filePrefix;
            if (fileName == null)
                fileName = facility;
            if (fileName == null)
                fileName = callingAssembly.GetName().Name;

            if (fileSuffix == null)
                fileSuffix = $"{Environment.MachineName}_{Environment.UserName}";

            // File name reflects calling assembly/facility/custom prefix
            // and the starting date and time.
                fileName += $"_{beginOfLog:yyyy-MMM-dd_HH-mm}Z_{fileSuffix}.log";

            try
            {
                // In case we call the tool often, we make sure to append, not to overwrite any logs.
                // Neither do we want to care for incrementing suffix counters and such.
                fullFileName = Path.Combine(destinationPath, fileName);
                writer = new StreamWriter(fullFileName, true, Encoding.UTF8);

                lastTimestamp = null;
                WriteLineKernel($"#");
                WriteLineKernel($"# Begin of log for facility: {facility ?? "(null)"}, calling assembly: {callingAssembly.FullName}");
                WriteLineKernel($"# Original log file name: {fullFileName}");
                WriteLineKernel($"# Machine name: {Environment.MachineName}, local user: {Environment.UserDomainName}/{Environment.UserName}");
                WriteLineKernel($"# OS version: {Environment.OSVersion}, CLR version: {Environment.Version}");
                WriteLineKernel($"#", true);
                WriteLog(Level.None, "Log started", "FileLogger");

            } catch (Exception ex)
            {
                throw new ApplicationException($"Can not open log-file for writing: {fileName} - {ex}: {ex.Message}", ex);
            }
        }

        protected override void WriteLog(Level level, string message, string facility = null)
        {
            DateTime timestamp = DateTime.UtcNow;
            if (lastTimestamp != null && timestamp.DayOfYear != ((DateTime)lastTimestamp).DayOfYear)
            {
                WriteLineKernel($"#");
                WriteLineKernel($"# --- DAY BOUNDARY ---");
                WriteLineKernel($"#");
                WriteLineKernel($"[{timestamp:yyyy-MMM-ddTHH:mm:ssZ}]", true);
            }
            lastTimestamp = timestamp;
            StringBuilder levelString = new StringBuilder();
            if (level.HasFlag(Level.Fatal)) levelString.Append('F');
            if (level.HasFlag(Level.Error)) levelString.Append('E');
            if (level.HasFlag(Level.Warning)) levelString.Append('W');
            if (level.HasFlag(Level.Information)) levelString.Append('I');
            if (level.HasFlag(Level.Diagnostic)) levelString.Append('D');

            string[] lines = message.Trim().Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
            if (lines.Length == 0)
                lines = new string[] { "(no message)" };
            WriteLineKernel($"[{timestamp:HH:mm:ss}Z - {levelString}] {facility??"(null)"}: {lines[0]}", lines.Length == 1);
            for (int i = 1; i < lines.Length; i++)
                WriteLineKernel($"    {lines[i]}", i == lines.Length-1);
        }

        /// <summary>
        /// Free up the file resource.
        /// </summary>
        public void Dispose()
        {
            if (writer != null)
            {
                ((IDisposable)writer).Dispose();
            }
        }
    }
}
