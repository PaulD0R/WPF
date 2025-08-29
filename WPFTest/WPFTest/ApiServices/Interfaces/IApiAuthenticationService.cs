using WPFTest.MVVM.Model.Person;

namespace WPFTest.ApiServices.Interfaces
{
    public interface IApiAuthenticationService
    {
        Task<Token> Signup(SignupPerson signupPerson);
        Task<Token> Signin(SigninPerson signinPerson);
        Task<Token> SigninWithToken(string token);
        Task<bool> Logout();
    }
}
