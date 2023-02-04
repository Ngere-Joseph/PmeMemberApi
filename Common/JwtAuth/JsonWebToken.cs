namespace PmeMemberApi
{
    public class JsonWebToken
    {
        public string Token { get; set; }
        public long Expires { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
