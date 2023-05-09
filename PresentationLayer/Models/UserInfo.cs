namespace PresentationLayer.Models
{
    public class UserInfo
    {
        public UserInfo(string header, string body)
        {
            this.HeaderInfo = header;
            this.Body = body;
        }

        public string HeaderInfo { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}
