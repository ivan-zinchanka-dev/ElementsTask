namespace ElementsTask.Common.Data.Crypto.Abstractions
{
    public interface IStreamCryptoService
    {
        public string Encrypt(string data);
        public string Decrypt(string data);
    }
}