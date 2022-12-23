using System;
using Simulator.Model;
namespace Simulator.Messages
{
    /// <summary>
    /// Message Class For StateFile
    /// </summary>
    public class StateFileMessage
    {
        /// <summary>
        /// Get Content
        /// </summary>
        public StateFileModel Content { get; set; }

        /// <summary>
        /// Get Notification
        /// </summary>
        public string Notification { get; set; }

        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="file">File object</param>
        /// <param name="message">Message string</param>
        public StateFileMessage(StateFileModel file, string message)
        {
            Content = file;
            Notification = message;
        }
    }
}

