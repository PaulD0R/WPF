namespace WPFTest.Data
{
    static class StaticData
    {
        public const int NUMBER_OF_ELEMENTS_PER_PAGE = 20;

        private const string MAIN_ROUDE = @"http://localhost:8080/WPF";
        public const string EXERCISE_ROUDE = MAIN_ROUDE + @"/Exercises/";
        public const string SUBJECT_ROUDE = MAIN_ROUDE + @"/Subjects/";
        public const string AUTHENTICATION_ROUDE = MAIN_ROUDE + @"/Authentication/";
        public const string PERSON_ROUDE = MAIN_ROUDE + @"/Persons/";
        public const string ADMIN_ROUDE = MAIN_ROUDE + @"/Admin/";

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
