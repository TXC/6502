using System;
using Simulator.Model;
namespace Simulator.Messages
{
    /// <summary>
    /// Message Class For SaveFile
    /// </summary>
    public class SaveFileMessage
    {
        /// <summary>
        /// Get Notification
        /// </summary>
        public string Notification { get; set; }

        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="message">Message string</param>
        public SaveFileMessage(string message)
        {
            Notification = message;
        }
    }
}

