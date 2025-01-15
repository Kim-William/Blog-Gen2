namespace Wkkim.Blog.Web.Models.Domain
{
    public class UserLog
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string IPAddress { get; set; }
        public string RequestUrl { get; set; }
        public string HttpMethod { get; set; }
        public string UserAgent { get; set; }
        public int ResponseCode { get; set; }
        public DateTime AccessTime { get; set; }
    }
}
