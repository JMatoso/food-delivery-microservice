using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Order.Data.Repositories;
using Order.Models;

namespace Order.Controllers
{
    /// <summary>
    /// Ordering management.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _app;
        private readonly ICartRepo _cart;
        public OrderController(
            IOrderRepo app,
            ICartRepo cart)
        {
            _app = app;
            _cart = cart;
        }

        /// <summary>
        /// Get ordered products.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="401">Not Authorized.</response>
        [HttpGet("order")]
        public async Task<ActionResult<List<DTO.Order>>> All() => Ok(await _app.All());

        /// <summary>
        /// Get client ordered products.
        /// </summary>
        /// <param name="clientId">Client Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid client ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("order/{clientId}")]
        public async Task<ActionResult<List<DTO.Order>>> AllClientOrders(Guid clientId)
        {
            if(clientId == Guid.Empty)
            {
                return BadRequest("Insert a valid client ID.");
            }

            var orders = await _app.All(clientId);

            if(orders == null)
            {
                return NotFound("Any order found.");
            }

            return Ok(orders);
        }

        /// <summary>
        /// Get order.
        /// </summary>
        /// <param name="orderId">Order Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid order ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("order/o/{orderId}")]
        public async Task<ActionResult<DTO.Order>> Get(Guid orderId)
        {
            if(orderId == Guid.Empty)
            {
                return BadRequest("Insert a valid order ID.");
            }

            var order = await _app.Get(orderId);

            if(order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }

        /// <summary>
        /// Send order.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        /// <response code="401">Not Authorized.</response>
        [HttpPost("order")]
        public async Task<ActionResult> Add([FromBody]VMOrder model)
        {
            if(ModelState.IsValid)
            {
                DTO.Order order = new()
                {
                    ProductId = model.ProductId,
                    ClientId = model.ClientId,
                    ProductName = model.ProductName,
                    ProductQuantity = model.ProductQuantity,
                    ExtraId = model.ExtraId,
                    ExtraQuantity = model.ExtraQuantity,
                    TotalPrice = model.TotalPrice,
                    Longitude = model.Longitude,
                    Latitude = model.Latitude,
                    DeliveryAddress = model.DeliveryAddress,
                    PaymentType = model.PaymentType,
                    OrderStatus = model.OrderStatus,
                    Created = DateTimeOffset.Now
                };

                if(await _app.Add(order) == true)
                {

                    return Ok("Your order has been shipped you can track its status now.");
                }
                
                return BadRequest("Something went wrong, try again.");
            }

            return BadRequest("Fill all required fields.");
        }
    }
}