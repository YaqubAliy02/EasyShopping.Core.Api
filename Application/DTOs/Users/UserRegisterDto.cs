using Application.Models;
using Domain.Models;

namespace Application.DTOs.Users
{
    public class UserRegisterDto
    {
        public User User { get; set; }
        public Token UserToken { get; set; }
    }
}
