namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IErrorViewModel
    {
        string? Text { get; set; }

        public void LoadError(string error);
    }
}
