namespace Core.JSON
{
    public class JWToken
    {
        public class auth_service
        {
            public string sub { get; set; }
            public string iss { get; set; }
            public long exp { get; set; }
            public string session { get; set; }
            public string app { get; set; }
            public string env { get; set; }
        }

        public class ownership_service
        {
            public string sub { get; set; }
            public string iss { get; set; }
            public long exp { get; set; }
            public int uplay_id { get; set; }
            public int product_id { get; set; }
            public int branch_id { get; set; }
            public List<string> flags { get; set; }
        }
    }
}
