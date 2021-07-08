using System;

namespace ResourceManager.DataSource.Ftp.Configuration
{
    public class CachingPolicy
    {
        public CachingPolicy(TimeSpan expirationTime)
        {
            CreatedTime = DateTime.UtcNow;
            ExpirationTime = expirationTime;
        }

        public DateTime CreatedTime { get; }

        public TimeSpan ExpirationTime { get; }

        public bool IsExpired => DateTime.UtcNow - CreatedTime > ExpirationTime;
    }
}
