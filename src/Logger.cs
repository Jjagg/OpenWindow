using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenWindow
{
    public class Logger
    {
        /// <summary>
        /// A list of logged messages.
        /// </summary>
        public List<LogMessage> Messages { get; }

        internal Logger()
        {
#if !DISABLE_LOGGING
            Messages = new List<LogMessage>();
#endif
        }

        /// <summary>
        /// Log a message. Uses <see cref="DateTime"/>
        /// </summary>
        /// <param name="type">The type of the message.</param>
        /// <param name="message">The content of the message.</param>
        internal void LogMessage(MessageType type, string message)
        {
#if !DISABLE_LOGGING
            var msg = new LogMessage(type, message, DateTime.Now);
            Messages.Add(msg);
#endif
        }

        /// <summary>
        /// Delete all logged messages.
        /// </summary>
        public void Clear()
        {
#if !DISABLE_LOGGING
            Messages.Clear();
#endif
        }

        /// <summary>
        /// Get a string dump of all messages logged so far.
        /// </summary>
        /// <returns>A string value with all logged messages concatenated, separated by new lines.</returns>
        public string Dump(MessageType type = MessageType.Info)
        {
#if !DISABLE_LOGGING
            return Messages
                    .Where(m => (int) m.Type >= (int) type)
                    .Aggregate("", (s1, s2) => s1 + '\n' + s2.ToString());
#else
            return string.Empty;
#endif
        }
    }

    /// <summary>
    /// Type for a message logged by <see cref="Logger"/>.
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// The type of the message. Indicates severity.
        /// </summary>
        public MessageType Type { get; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Time at which the message was logged.
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        /// Create a <see cref="LogMessage"/>.
        /// </summary>
        /// <param name="type">Type of the message.</param>
        /// <param name="content">Content of the message.</param>
        /// <param name="timeStamp">The time at which the message was logged.</param>
        internal LogMessage(MessageType type, string content, DateTime timeStamp)
        {
            Type = type;
            Content = content;
            TimeStamp = timeStamp;
        }

        public override string ToString()
        {
            return $"[{TimeStamp:T}] {Type.ToString().ToUpperInvariant()}: {Content}";
        }
    }
}