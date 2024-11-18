﻿using Application.UseCases.ProductThumbnails.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductThumbnailController : ApiControllerBase
    {
        private readonly IMediator mediator;
        public ProductThumbnailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadPhoto([FromQuery] UploadProductThumbnailCommand uploadProductThumbnailCommand)
        {
            return await this.mediator.Send(uploadProductThumbnailCommand);
        }
    }
}