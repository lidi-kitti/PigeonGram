using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel;

namespace WCF_Library_Server.DB_Context
{
    public class User
    {
        public int Id { get; set; }

        //поля с модификатором required обязательны для ввода
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public byte[] HashedPassword { get; set; }

        // данного поля не будет в моделе
        [NotMapped]
        public OperationContext operationContext { get; set; }

    }
}
