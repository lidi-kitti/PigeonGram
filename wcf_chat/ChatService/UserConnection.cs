using System.Diagnostics.Contracts;

namespace ChatService
{
    public class UserConnection
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Room { get; set; }
    }
}
