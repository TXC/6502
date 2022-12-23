using System;
using Simulator.Model;
namespace Simulator.Messages
{
    /// <summary>
    /// Message Class For CloseFile
    /// </summary>
    public class CloseFileMessage
    {
        /// <summary>
        /// Get Notification
        /// </summary>
        public string Notification { get; set; }

        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="message">Message string</param>
        public CloseFileMessage(string message)
        {
            Notification = message;
        }
    }
}

