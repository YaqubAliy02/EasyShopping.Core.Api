//using Application.UseCases.Roles.Command;
//using Application.UseCases.Users.Commands;
using Application.UseCases.Roles.Command;
using Application.UseCases.Users.Command;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
           UserMappingRules();
            UserRoleMappingRules();
        }

        private void UserRoleMappingRules()
        {
            CreateMap<CreateRoleCommand, UserRole>();
            CreateMap<UserRole, CreateRoleCommandResult>();
            CreateMap<UpdateRoleCommand, UserRole>();
        }
        private void UserMappingRules()
        {
            CreateMap<CreateUserCommand, User>()
                .ForMember(destination => destination.UserRoles,
                options => options.MapFrom(src => src.RolesId
                .Select(x => new UserRole() { RoleId = x })));

            CreateMap<User, CreateUserCommandHandlerResult>();
        }
    }
}
