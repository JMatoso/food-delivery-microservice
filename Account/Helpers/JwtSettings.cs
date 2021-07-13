namespace Account.Helpers
{
    public class JwtSettings
    {
        public static string SecretKey = "tKE+pMd2rQAHBbOjXWTZqacLJRLqlrnTzZdmKRJEXLjtiGOnFY3w+vuUxPSgLdMFbbVXxPrFWNUd/yQyG5PsEg==";
        public static string AudienceToken = "http://localhost:44381";
        public static string IssuerToken = "http://localhost:44381";
        public static int ExpireMinutes = 60;
    }
}