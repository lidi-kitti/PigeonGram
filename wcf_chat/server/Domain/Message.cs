namespace server.Domain
{
    public class Message
    {
        public int IdMessage { get; set; }
        public int user_from_id { get; set; }
        public int user_to_id { get;set; }
        public string message { get; set; }
        public DateTime created_at { get; set; }
    }
}
