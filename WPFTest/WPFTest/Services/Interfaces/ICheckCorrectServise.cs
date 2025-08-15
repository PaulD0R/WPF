namespace WPFTest.Services.Interfaces
{
    public interface ICheckCorrectServise
    {
        bool IsPassword(string password);
        bool IsEmail(string email);
    }
}
