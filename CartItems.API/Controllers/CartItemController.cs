using CartItems.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Validation.AspNetCore;
using System;
using System.Collections.Generic;

namespace CartItems.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class CartItemController : ControllerBase
    {
        private readonly ILogger<CartItemController> _logger;

        public CartItemController(ILogger<CartItemController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<CartItem>> Get()
        {
            return Ok(new List<CartItem>
            {
                new CartItem
                {
                    Id = Guid.Empty,
                    Name = "Test Cart"
                }
            });
        }

        [HttpGet("{id}")]
        public ActionResult<CartItem> Get(Guid id)
        {
            var cartItem = new CartItem
            {
                Id = Guid.Empty,
                Name = "Test Cart"
            };
            return Ok(cartItem);
        }
    }
}
