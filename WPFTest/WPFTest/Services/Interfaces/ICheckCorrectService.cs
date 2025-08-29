namespace WPFTest.Services.Interfaces
{
    public interface ICheckCorrectService
    {
        bool IsPassword(string password);
        bool IsEmail(string email);
    }
}
