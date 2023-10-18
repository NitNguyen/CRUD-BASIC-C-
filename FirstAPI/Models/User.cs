using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAPI.Models
{
    [Table("users")]
    public class User
    {
        public int? userId { get; set; }

        public string userName { get; set; }
        public string password { get; set; }
        public string name { get; set; }

        public string? salt { get; set; }
    }

    public class UserLogin
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}
