using System;

namespace HisDemo
{
    /// <summary>
    /// Use this class to log events within the project.
    /// This is the base logger class, which itself cannot write logs.
    /// It requires e.g. the FileLogger as a parent.
    /// You should chain multiple logger instances to reflect application hierarchy.
    /// Each new logger takes a facility argument which should match the scope of
    /// the logger. Each message can add another facility information on top if
    /// such a level of detail is required.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Creates a new logger on top of an existing logger.
        /// Use this to build a hierarchy that reflects the application structure to enable
        /// proper facility display and more granular filtering options.
        /// </summary>
        /// <param name="parent">The logger which receives entries from this logger.</param>
        /// <param name="facility">The facility name to add to messages before passing up in hierarchy.</param>
        public Logger(Logger parent, string facility = null)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            this.parent = parent;
            this.facility = facility;
        }
        /// <summary>
        /// This constructor is required by derived classes which provide actual log writing.
        /// </summary>
        /// <param name="facility"></param>
        protected Logger(string facility = null) 
        {
            this.facility = facility;
        }

        protected Logger parent;
        protected string facility;

        /// <summary>
        /// Only log entries with a level that match at least one of the flags set here are written.
        /// In short: Messages are only sent down the chain if bitwise-and of level and LevelFilter is not 0.
        /// </summary>
        public Level LevelFilter { get; set; } = Level.All;

        /// <summary>
        /// Various message level severeties.
        /// A message can have multiple flags set, but usually only one flag should be set.
        /// For filtering messages, multiple flags are used.
        /// </summary>
        [Flags]
        public enum Level : byte
        {
            None = 0,
            Diagnostic = 1,
            Information = 2,
            Warning = 4,
            Error = 8,
            Fatal = 16,
            All = 0xFF
        }

        /// <summary>
        /// Create log entry.
        /// This will be sent down the logger chain only if the level is not filtered out.
        /// </summary>
        /// <param name="level">Severity of entry</param>
        /// <param name="message">Content</param>
        /// <param name="facility">Optional sub-facility/function name/... for this entry</param>
        public void Log(Level level, string message, string facility = null)
        {
            if (((byte)level & (byte)LevelFilter) != 0)
            {
                string newFacility = "";
                if (this.facility != null)
                    newFacility = $"/{this.facility}";
                if (facility != null)
                    newFacility += $"/{facility}";

                WriteLog(level, message, newFacility.Length > 0 ? newFacility.Trim('/') : null);
            }
        }

        /// <summary>
        /// Shortcut for Log(Level.Diagnostic, ...).
        /// </summary>
        /// <param name="message">Content</param>
        /// <param name="facility">Optional sub-facility/function name/... for this entry</param>
        public void Diagnostic(string message, string facility = null)
        {
            Log(Level.Diagnostic, message, facility);
        }

        /// <summary>
        /// Shortcut for Log(Level.Information, ...).
        /// </summary>
        /// <param name="message">Content</param>
        /// <param name="facility">Optional sub-facility/function name/... for this entry</param>
        public void Information(string message, string facility = null)
        {
            Log(Level.Information, message, facility);
        }

        /// <summary>
        /// Shortcut for Log(Level.Warning, ...).
        /// </summary>
        /// <param name="message">Content</param>
        /// <param name="facility">Optional sub-facility/function name/... for this entry</param>
        public void Warning(string message, string facility = null)
        {
            Log(Level.Warning, message, facility);
        }

        /// <summary>
        /// Shortcut for Log(Level.Error, ...).
        /// </summary>
        /// <param name="message">Content</param>
        /// <param name="facility">Optional sub-facility/function name/... for this entry</param>
        public void Error(string message, string facility = null)
        {
            Log(Level.Error, message, facility);
        }

        /// <summary>
        /// Shortcut for Log(Level.Fatal, ...).
        /// </summary>
        /// <param name="message">Content</param>
        /// <param name="facility">Optional sub-facility/function name/... for this entry</param>
        public void Fatal(string message, string facility = null)
        {
            Log(Level.Fatal, message, facility);
        }

        /// <summary>
        /// Execute the log writing. In the base class, this sends the log to the parent loggers Log() function.
        /// In a root writer class, override this to perform the actual log write.
        /// This function is only called if the FilterLevel has passed.
        /// </summary>
        /// <param name="level">Severity of entry</param>
        /// <param name="message">Content</param>
        /// <param name="facility">Optional sub-facility/function name/... for this entry</param>
        protected virtual void WriteLog(Level level, string message, string facility = null)
        {
            parent.Log(level, message, facility);
        }
    }
}
