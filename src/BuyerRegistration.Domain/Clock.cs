using System;

namespace BuyerRegistration.Domain
{
    public class Clock
    {
        public virtual DateTime GetCurrentUtcDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}