using WPFTest.MVVM.Model.Person;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IAuthenticationViewModel
    {
        public void CheckToken();
        public void OpenMainApplication(Token token);
    }
}
