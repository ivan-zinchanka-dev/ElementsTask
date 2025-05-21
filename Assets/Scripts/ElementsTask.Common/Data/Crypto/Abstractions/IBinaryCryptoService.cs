namespace ElementsTask.Common.Data.Crypto.Abstractions
{
    public interface IBinaryCryptoService
    {
        public byte[] Encrypt(byte[] data);
        public byte[] Decrypt(byte[] data);
    }
}