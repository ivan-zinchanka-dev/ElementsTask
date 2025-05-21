using ElementsTask.Common.Data.Crypto.Abstractions;
using UnityEngine;

namespace ElementsTask.Common.Data.Crypto.ROS
{
    [CreateAssetMenu(fileName = "ros_crypto_config", menuName = "Crypto/RosCryptoConfig", order = 0)]
    public class RosCryptoConfig : ScriptableObject, IRosCryptoConfig
    {
        [field:SerializeField] 
        public RosCryptoKey CryptoKey { get; private set; }
    }
}