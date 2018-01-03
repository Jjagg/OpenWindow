using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenWindow
{
    /// <summary>
    /// A simple class for logging messages.
    /// </summary>
    public sealed class Logger : IDisposable
    {
        private TextWriter _outputWriter;
        private string _format;

        /// <summary>
        /// Get or set the <see cref="TextWriter"/> that immediately writes away log messages.
        /// </summary>
        public TextWriter OutputWriter
        {
            get { return _outputWriter; }
            set
            {
                _outputWriter?.Dispose();
                _outputWriter = value;
            }
        }

        /// <summary>
        /// A list of logged messages.
        /// </summary>
        public List<Message> Messages { get; }

        /// <summary>
        /// Create a new Logger.
        /// </summary>
        public Logger()
        {
            Messages = new List<Message>();
            _format = "[{0:T}] {1}: {2}";
#if NETSTANDARD1_3
            // default to stdout when available
            _outputWriter = Console.Out;
#endif
        }

        /// <summary>
        /// Log a message. Uses <see cref="DateTime.Now"/>
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="message">The content of the message.</param>
        public void Log(Level level, string message)
        {
            var msg = new Message(level, message, DateTime.Now);
            Messages.Add(msg);
            OutputWriter?.WriteLine(msg.ToString(_format));
        }

        /// <summary>
        /// Delete all logged messages.
        /// </summary>
        public void Clear()
        {
            Messages.Clear();
        }

        /// <summary>
        /// Get a string dump of all messages logged so far.
        /// </summary>
        /// <returns>A string value with all logged messages concatenated, separated by new lines.</returns>
        public string Dump(Level level = Level.Info)
        {
            return Messages
                    .Where(m => (int) m.Level >= (int) level)
                    .Aggregate("", (s1, s2) => s1 + '\n' + s2.ToString());
        }

        public void Dispose()
        {
            _outputWriter?.Dispose();
        }

        /// <summary>
        /// A Message logged by <see cref="Logger"/>.
        /// </summary>
        public class Message
        {
            /// <summary>
            /// The type of the message. Indicates severity.
            /// </summary>
            public Level Level { get; }

            /// <summary>
            /// The content of the message.
            /// </summary>
            public string Content { get; }

            /// <summary>
            /// Time at which the message was logged.
            /// </summary>
            public DateTime TimeStamp { get; }

            /// <summary>
            /// Create a <see cref="Message"/>.
            /// </summary>
            /// <param name="level">Log level of the message.</param>
            /// <param name="content">Content of the message.</param>
            /// <param name="timeStamp">The time at which the message was logged.</param>
            public Message(Level level, string content, DateTime timeStamp)
            {
                Level = level;
                Content = content;
                TimeStamp = timeStamp;
            }

            public override string ToString()
            {
                return $"[{TimeStamp:T}] {Level.ToString().ToUpperInvariant()}: {Content}";
            }

            public string ToString(string format)
            {
                return string.Format(format, TimeStamp, Level.ToString().ToUpperInvariant(), Content);
            }
        }

        /// <summary>
        /// Log level.
        /// </summary>
        public enum Level
        {
            Debug,
            Info,
            Warning,
            Error
        }
    }
}