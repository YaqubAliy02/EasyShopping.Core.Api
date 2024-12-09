using Application.DTOs.Users;
using Application.Models;
using Application.UseCases.Accounts.Command;
using Application.UseCases.Categories.Command;
using Application.UseCases.Products.Command;
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
            ProductMappingRules();
            CategoryMappingRules();
        }

        private void CategoryMappingRules()
        {
            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<Category, CreateCategoryCommandHandlerResult>();
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
            CreateMap<User, UserGetDto>();

            CreateMap<UpdateUserCommand, User>()
                .ForMember(destination => destination.UserRoles,
                option => option.MapFrom(src => src.RolesId
                .Select(x => new UserRole() { RoleId = x })));

            CreateMap<RegisterUserCommand, User>()
                .ForMember(destination => destination.UserRoles,
                options => options.MapFrom(src => src.RolesId
                .Select(x => new UserRole() { RoleId = x })));

            CreateMap<User, RegisterUserCommandResult>();
            CreateMap<Token, RefreshTokenCommandResult>();
            CreateMap<ModifyUserCommand, User>();
            CreateMap<UserGetDto, User>();
        }


        private void ProductMappingRules()
        {
            CreateMap<CreateProductCommand, Product>();
            CreateMap<Product, CreateProductCommandHandlerResult>();
            CreateMap<UpdateProductCommand, Product>();
        }
    }
}
/*{
  "id": "4bf740de-515f-4b7d-bb6c-6a91f9ac09c9",
  "name": "LaptopUpdated",
  "description": "This is laptop Updated",
  "costPrice": 1111,
  "sellingPrice": 111,
  "stockQuantity": 10,
  "categoryId": "fc2c6c18-cd11-4a14-92aa-b0498103e537",
  "thumbnail": "string"

eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyMTExMTFAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNmMTA5OWE4LTVhMDAtNGYyYy05NTIxLTdhY2NiNTdkNWNmNCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzMyMTA3MTA5LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDE1IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MjgxIn0._zGOf0cnQLR1Jr5jVzZktZmhuIwddjNsuhddf1cs9EU
}*/
