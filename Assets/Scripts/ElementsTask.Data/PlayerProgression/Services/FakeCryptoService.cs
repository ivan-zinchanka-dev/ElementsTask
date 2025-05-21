using ElementsTask.Common.Data.Crypto.Abstractions;

namespace ElementsTask.Data.PlayerProgression.Services
{
    public class FakeCryptoService : IStreamCryptoService
    {
        public string Encrypt(string data)
        {
            return data;
        }

        public string Decrypt(string data)
        {
            return data;
        }
    }
}