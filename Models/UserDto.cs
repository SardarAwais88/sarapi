using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sarapi.Models
{
    public class LoginDto 
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }




        [Required]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

    }
 

    public class UserDto : LoginDto

    {
        public  string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
       
        // we need to add this in user collection 

        public ICollection<string> Roles { get; set; }
    }

}
