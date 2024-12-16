namespace Application.DTOs.Users
{
    public class UserGetAllDto
    {

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid[] RolesId { get; set; }
       
    }
}
