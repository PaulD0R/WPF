namespace WPFTest.Data
{
    public class PageButtonData(int number, bool isChecked)
    {
        public int Number { get; set; } = number;
        public bool IsChecked { get; set; } = isChecked;
    }
}
