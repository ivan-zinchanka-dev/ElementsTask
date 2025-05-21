using ElementsTask.Common.Data.Crypto.ROS;

namespace ElementsTask.Common.Data.Crypto.Abstractions
{
    public interface IRosCryptoConfig
    {
        public RosCryptoKey CryptoKey { get; }
    }
}