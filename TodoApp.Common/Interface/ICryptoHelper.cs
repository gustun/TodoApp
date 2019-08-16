namespace TodoApp.Common.Interface
{
    public interface ICryptoHelper
    {
        string Hash(string input);
        bool Verify(string input, string hashedInput);
    }
}
