using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodItems.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Validation.AspNetCore;

namespace FoodItems.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class FoodItemController : ControllerBase
    {

        private readonly ILogger<FoodItemController> _logger;
        public FoodItemController(ILogger<FoodItemController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<FoodItem>> Get()
        {
            var data = new List<FoodItem>
            {
                new FoodItem
                {
                    Id = Guid.Empty,
                    Name = "Burger",
                    Price = 200
                }
            };
            return Ok(data);
        }

        [HttpGet("{id}")]
        public ActionResult<FoodItem> Get(Guid id)
        {
            var foodItem = new FoodItem
            {
                Id = Guid.Empty,
                Name = "Burger",
                Price = 200
            };
            return Ok(foodItem);
        }
    }
}
