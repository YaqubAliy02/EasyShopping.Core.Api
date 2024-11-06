//using Application.UseCases.Roles.Command;
//using Application.UseCases.Users.Commands;
using Application.UseCases.Roles.Command;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
           // UserMappingRules();
            UserRoleMappingRules();
        }

        private void UserRoleMappingRules()
        {
            CreateMap<CreateRoleCommand, UserRole>();
            CreateMap<UserRole, CreateRoleCommandResult>();
            CreateMap<UpdateRoleCommand, UserRole>();
        }
    }
}
