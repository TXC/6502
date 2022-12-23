using System;
using Simulator.Model;
namespace Simulator.Messages
{
    /// <summary>
    /// Message Class For AssemblyFile
    /// </summary>
    public class AssemblyFileMessage
    {
        /// <summary>
        /// Get Content
        /// </summary>
        public AssemblyFileModel Content { get; set; }

        /// <summary>
        /// Get Notification
        /// </summary>
        public string Notification { get; set; }

        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="file">File object</param>
        /// <param name="message">Message string</param>
        public AssemblyFileMessage(AssemblyFileModel file, string message)
        {
            Content = file;
            Notification = message;
        }
    }
}

