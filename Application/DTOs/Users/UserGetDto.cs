using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Application.DTOs.Users
{
    public class UserGetDto
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public Guid[] ProductsId { get; set; }
    }
}
















