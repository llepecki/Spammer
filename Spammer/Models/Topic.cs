using System;

namespace Spammer.Models
{
    public class Topic : IEquatable<Topic>, IEquatable<string>
    {
        public string Abbreviation { get; set; }

        public string Description { get; set; }

        public bool Equals(Topic other)
        {
            return Equals(other.Abbreviation);
        }

        public bool Equals(string abbreviation)
        {
            return string.Equals(Abbreviation, abbreviation, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}