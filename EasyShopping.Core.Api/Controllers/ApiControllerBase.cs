using AutoMapper;
using EasyShopping.Core.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [ApiController]
    [ValidationActionFilter]
    public class ApiControllerBase : ControllerBase
    {
        private readonly IMapper mapper;

        protected IMapper Mapper => this.mapper ?? HttpContext
            .RequestServices.GetRequiredService<IMapper>();
        
    }
}
