namespace WPFTest.Data
{
    static class StaticData
    {
        public const int NUMBER_OF_ELEMENTS_PER_PAGE = 20;
        public const int MAIN_WINDOW_RADIUS = 15;

        public const string EXERCISE_ROUDE = @"https://localhost:7145/WPF/Exercises/";
        public const string SUBJECT_ROUDE = @"https://localhost:7145/WPF/Subjects/";
        public const string AUTHENTICATION_ROUDE = @"https://localhost:7145/WPF/Authentication/";
        public const string PERSON_ROUDE = @"https://localhost:7145/WPF/Persons/";

        private static string _token { get; set; } = string.Empty;
        public static EventHandler<string> OnTokenChanged { get; set; }

        public static string TOKEN
        {
            get => _token;
            set
            {
                if (_token != value)
                {
                    _token = value;
                    OnTokenChanged?.Invoke(null, value);
                }
            }
        }
    }
}
