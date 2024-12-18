using Application.DTOs.Comments;
using Application.DTOs.Products;
using Application.DTOs.Users;
using Application.Models;
using Application.UseCases.Accounts.Command;
using Application.UseCases.Categories.Command;
using Application.UseCases.Comments.Command;
using Application.UseCases.Products.Command;
using Application.UseCases.Roles.Command;
using Application.UseCases.SubComments.Command;
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
            CommentMappingRules();
            SubCommentMappingRules();
        }

        private void SubCommentMappingRules()
        {
            CreateMap<CreateSubCommentCommand, SubComment>();
            CreateMap<SubComment, CreateSubCommentCommandHandlerResult>();
        }

        private void CommentMappingRules()
        {
            CreateMap<CreateCommentCommand, Comment>();
            CreateMap<Comment, CreateCommentCommandHandlerResult>();
            CreateMap<UpdateCommentByIdCommand, Comment>();
            CreateMap<Comment, GetCommentDto>()
                .ForMember(destination => destination.SubCommentsId,
                options => options.MapFrom(src => src.SubComments.Select(s => s.Id)));

        }

        private void CategoryMappingRules()
        {
            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<Category, CreateCategoryCommandHandlerResult>();
            CreateMap<UpdateCategoryCommand, Category>();
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
            CreateMap<User, UserGetDto>().
                ForMember(destination => destination.ProductsId,
                option => option.MapFrom(src => src.Products.Select(p => p.Id)));

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
            CreateMap<Product, ProductGetDto>();
            CreateMap<UpdateProductCommand, ProductGetDto>();
            CreateMap<UpdateProductCommand, Product>();
            CreateMap<Product, ProductGetDto>()
                .ForMember(destination => destination.CommentIds,
                option => option.MapFrom(src => src.Comments.Select(c => c.Id)));
        }
    }
}