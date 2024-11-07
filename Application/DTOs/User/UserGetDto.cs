using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class UserGetDto
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
    }
}















