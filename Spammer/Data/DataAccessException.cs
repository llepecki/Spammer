using System;

namespace Spammer.Data
{
    public class DataAccessException : Exception
    {
        public DataAccessException()
        {
        }

        public DataAccessException(string message) : base(message)
        {
        }
    }
}
