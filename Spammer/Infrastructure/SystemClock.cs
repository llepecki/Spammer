using System;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace Spammer.Infrastructure
{
    public class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTime.UtcNow;
    }
}
