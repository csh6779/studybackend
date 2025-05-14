using System.ComponentModel.DataAnnotations;

namespace login_app.Models
{
    public class User
    {
        public string ID { get; set; }

        [Required] 
        [StringLength(10)] 
        public string name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15)]
        public string password { get; set; }
        [EmailAddress]
        public string email { get; set; }
        [Phone]
        public string phone { get; set; }
    }
}
