using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Order.Data.Repositories;
using Order.DTO;
using Order.Models;

namespace Order.Controllers
{
    /// <summary>
    /// Shopping Basket/Cart management.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICartRepo _app;
        public ShoppingController(ICartRepo app)
        {
            _app = app;
        }

        /// <summary>
        /// Get client cart products.
        /// </summary>
        /// <param name="clientId">Client Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid client ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("shopping/{clientId}")]
        public async Task<ActionResult<List<Cart>>> All(Guid clientId) => Ok(await _app.All(clientId));

        /// <summary>
        /// Get cart product.
        /// </summary>
        /// <param name="cartId">Cart Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid cart ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("shopping/cart/{cartId}")]
        public async Task<ActionResult<Cart>> Get(Guid cartId)
        {
            if(cartId == Guid.Empty)
            {
                return BadRequest("Insert a valid cart Id");
            }

            var cart = await _app.Get(cartId);
            return cart == null ? null : Ok(cart);
        }

        /// <summary>
        /// Add product in cart.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        /// <response code="401">Not Authorized.</response>
        [HttpPost("shopping")]
        public async Task<ActionResult> Add([FromBody]VMCart model)
        {
            if(ModelState.IsValid)
            {
                Cart cart = new()
                {
                    ProductId = model.ProductId,
                    ClientId = model.ClientId,
                    ProductName = model.ProductName,
                    ExtraId = model.ExtraId,
                    ExtraQuantity = model.ExtraQuantity,
                    TotalPrice = model.TotalPrice,
                    Created = DateTimeOffset.Now
                };

                return await _app.Add(cart) == true ? Ok("Product has been added to cart.") : BadRequest("Something went wrong!");
            }

            return BadRequest("Fill all required fields.");
        }

        /// <summary>
        /// Remove product from cart.
        /// </summary>
        /// <param name="cartId">Cart Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid cart ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpDelete("shopping/{cartId}")]
        public async Task<ActionResult> Remove(Guid cartId)
        {
            if(cartId == Guid.Empty)
            {
                return BadRequest("Insert a valid cart Id");
            }

            var cart = await _app.Get(cartId);

            if(cart == null)
            {
                return NotFound("Cart not found.");
            }

            return await _app.Remove(cartId) == true ? Ok("Product removed from cart.") : BadRequest("Something went wrong.");
        }
    }
}