using System.Collections.Generic;

namespace Spammer.Sending
{
    public interface ISendContent
    {
        void Send(string mail, Content content);

        void Send(IEnumerable<string> mails, Content content);
    }
}