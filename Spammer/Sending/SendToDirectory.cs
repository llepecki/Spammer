using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Spammer.Sending
{
    internal class SendToDirectory : ISendContent
    {
        private const string AtCharacterReplacement = "__at__";
        private const string DateTimeFormat = "yyyyMMddTHHmmssZ";
        private const string FileExtension = "txt";

        private readonly ISystemClock _clock;
        private readonly string _directory;

        public SendToDirectory(ISystemClock clock, string directory)
        {
            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _clock = clock;
            _directory = directory;
        }

        public void Send(string mail, Content content)
        {
            Send(new[] { mail }, content);
        }

        public void Send(IEnumerable<string> mails, Content content)
        {
            string timestamp = _clock.UtcNow.ToString(DateTimeFormat);

            foreach (string mail in mails)
            {
                string fileName = CreateFileName(mail, timestamp);
                string fileContent = CreateFileContent(content);

                string destinationFilePath = Path.Combine(_directory, fileName);
                File.WriteAllText(destinationFilePath, fileContent);
            }
        }

        private string CreateFileName(string mail, string timestamp)
        {
            return $"{mail.Replace("@", AtCharacterReplacement)}_{timestamp}.{FileExtension}";
        }

        private string CreateFileContent(Content content)
        {
            return new StringBuilder()
                .Append($"Subject: {content.Subject}")
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append(content.Message)
                .ToString();
        }
    }
}