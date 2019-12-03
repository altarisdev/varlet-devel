namespace Variety
{
    public static class Globals
    {
        public static string DefaultPhpVersion { get; set; }
        public static int LastUpdateCheck { get; set; }

        static Globals()
        {
            DefaultPhpVersion = "7.2";
            LastUpdateCheck = 5;
        }
        
        public static string ServiceNameHttp()
        {
            return "VarletHttpd";
        }

        public static string ConfigFileName()
        {
            return "varlet.json";
        }
    }
}
