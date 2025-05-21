using System.Text;
using ElementsTask.Common.Data.Crypto.Abstractions;

namespace ElementsTask.Common.Data.Crypto.ROS
{
    public class RosCryptoService : IBinaryCryptoService, IStreamCryptoService
    {
        private readonly IRosCryptoConfig _config;
        
        public RosCryptoService(IRosCryptoConfig config)
        {
            _config = config;
        }

        public byte[] Encrypt(byte[] data)
        {
            return BinaryRosAlgorithm(data);
        }

        public byte[] Decrypt(byte[] data)
        {
            return BinaryRosAlgorithm(data);
        }

        public string Encrypt(string data)
        {
            return StreamRosAlgorithm(data);
        }

        public string Decrypt(string data)
        {
            return StreamRosAlgorithm(data);
        }

        private string StreamRosAlgorithm(string data)
        {
            return Encoding.UTF8.GetString(BinaryRosAlgorithm(Encoding.UTF8.GetBytes(data)));
        }

        private byte[] BinaryRosAlgorithm(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte) RosAlgorithm(bytes[i]);
            }
            
            return bytes;
        }

        private int RosAlgorithm(int input)
        {
            RosCryptoKey cryptoKey = _config.CryptoKey;
            
            return input ^ cryptoKey.PartA ^ cryptoKey.PartB ^ cryptoKey.PartC ^ cryptoKey.PartD;
        }
    }
}