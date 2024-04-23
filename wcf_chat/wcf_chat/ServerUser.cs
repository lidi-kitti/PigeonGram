using System.ServiceModel;

namespace wcf_chat
{
    public class ServerUser
    {
        public int ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set;}
        public string Email { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public int ID_Rank { get; set; }
        public OperationContext operationContext { get; set; }
    }
}
