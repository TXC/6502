using System;
using Simulator.Model;
namespace Simulator.Messages
{
    /// <summary>
    /// Message Class For OpenFile
    /// </summary>
    public class OpenFileMessage
    {
        /// <summary>
        /// Get Notification
        /// </summary>
        public string Notification { get; set; }

        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="message">Message string</param>
        public OpenFileMessage(string message)
        {
            Notification = message;
        }
    }
}

