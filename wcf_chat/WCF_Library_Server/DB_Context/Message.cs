using System;
using System.ComponentModel.DataAnnotations;

namespace WCF_Library_Server.DB_Context
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int User_From_Id { get; set; }
        [Required]
        public int User_To_Id { get;set; }
        [Required]
        public int Chat_Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int Content_Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public bool IsRead { get; set; }
    }
}
