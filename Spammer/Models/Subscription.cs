using System;
using System.Collections.Generic;

namespace Spammer.Models
{
    public class Subscription : IEquatable<Subscription>, IEquatable<string>
    {
        public string Mail { get; set; }

        public IList<Topic> Topics { get; set; }

        public bool Equals(Subscription other)
        {
            return Equals(other.Mail);
        }

        public bool Equals(string mail)
        {
            return string.Equals(Mail, mail, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
