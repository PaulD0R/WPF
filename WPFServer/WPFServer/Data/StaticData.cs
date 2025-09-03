namespace WPFServer.Data
{
    public class StaticData
    {
        //Data
        public const int NUMBER_OF_ELEMENTS_PER_PAGE = 20;
        public const int MAX_REQUEST_SIZE = 1024 * 1024 * 100;

        //DataBase
        //public const string CONNECTION_STRING = @"Server=localhost;Database=wpf_database;Uid=PaulDor;Pwd=2486;";
        public const string CONNECTION_STRING = @"Server=db;Port=3306;Database=wpfdb;User=PaulDor;Password=2486;";

        //Auth
        public const string ISSURE = "TestWPFServer";
        public const string AUDIENCE = "TestWPFServerYouser";
        public const string KEY = "neveroyatno_secretniy23131kluch228__yes_neveroyatno_secretniy23131kluch228__yes.neveroyatno_secretniy23131kluch228__yes___neveroyatno_secretniy23131kluch228__yes_neveroyatno_secretniy23131kluch228__yes___neveroyatno_secretniy23131kluch228__yes_neveroyatno_secretniy23131kluch228__yes___neveroyatno_secretniy23131kluch228__yes_neveroyatno_secretniy23131kluch228__yes___neveroyatno_secretniy23131kluch228__yes_neveroyatno_secretniy23131kluch228__yes___neveroyatno_secretniy23131kluch228__yes";
    }
}
