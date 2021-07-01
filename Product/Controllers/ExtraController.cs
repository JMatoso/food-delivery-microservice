using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Data.Repositories;
using Product.DTO;
using Product.Models;
using Product.Services;

namespace Product.Controllers
{
    /// <summary>
    /// Extra management.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ExtraController : ControllerBase
    {
        private readonly IExtraRepo _app;
        public ExtraController(IExtraRepo app)
        {
            _app = app;
        }

        /// <summary>
        /// List all extras.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="401">Unauthorized.</response>
        [HttpGet("extra")]
        public async Task<ActionResult<List<Extra>>> All() => Ok(await _app.All());

        /// <summary>
        /// Show all product extras.
        /// </summary>
        /// <param name="productId">Product Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        [HttpGet("extra/product/{productId}")]
        public async Task<ActionResult<List<Extra>>> AllFromCategory(Guid productId) => Ok(await _app.All(productId));

        /// <summary>
        /// Get an extra by ID.
        /// </summary>
        /// <param name="extraId">Extra Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("extra/i/{extraId}")]
        public async Task<ActionResult<DTO.Product>> GetId(Guid extraId)
        {
            if(extraId == Guid.Empty)
            {
                return BadRequest("Insert a valid ID.");
            }

            var extra = await _app.Get(extraId);
            return extra == null ? NotFound("Extra not found.") : Ok(extra);
        }

        /// <summary>
        /// Get an extra by name.
        /// </summary>
        /// <param name="extraName">Extra name.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("extra/n/{extraName}")]
        public async Task<ActionResult<DTO.Product>> GetName(string extraName)
        {
            if(string.IsNullOrEmpty(extraName))
            {
                return BadRequest("Insert a valid extra name.");
            }

            var extra = await _app.Get(extraName);
            return extra == null ? NotFound("Extra not found.") : Ok(extra);
        }

        /// <summary>
        /// Add an extra.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        /// <response code="401">Not Authorized.</response>
        [HttpPost("extra")]
        public async Task<ActionResult> Add(VMExtra model, IFormFile image)
        {
            if(ModelState.IsValid)
            {
                Extra extra = new()
                {
                    ProductId = model.ProductId,
                    Name = model.Name,
                    Description = model.Description,
                    ReadyTime = model.ReadyTime,
                    Price = model.Price,
                    Image = await UploadFile.Upload(image),
                    Quantity = model.Quantity,
                    MaxQuantityPerOrder = model.MaxQuantityPerOrder,
                    Created = DateTimeOffset.Now
                };

                return await _app.Add(extra) == true ? Ok("Extra has been added.") : BadRequest("Somenthing went wrong, try again.");
            }

            return BadRequest("Fill all required fields.");
        }

        /// <summary>
        /// Disable/Enable an extra.
        /// </summary>
        /// <param name="extraId">Extra Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpPut("extra/{extraId}")]
        public async Task<ActionResult> ChangeStatus(Guid extraId)
        {
            if(extraId == Guid.Empty)
            {
                return BadRequest("Insert a valid ID.");
            }

            return await _app.ChangeStatus(extraId) == true ? Ok("Extra status has been changed.") : BadRequest("Extra wasn't found or something went wrong!");
        }

        /// <summary>
        /// Update an extra.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpPut("extra")]
        public async Task<ActionResult> Update(Extra model)
        {
            if(ModelState.IsValid)
            {
                var extra = await _app.Get(model.Id);

                if(extra == null)
                {
                    return NotFound("Extra not found.");
                }

                extra.ProductId = model.ProductId;
                extra.Description = model.Description;
                extra.Image = model.Image;
                extra.Name = model.Name;
                extra.Price = model.Price;
                extra.Quantity = model.Quantity;
                extra.MaxQuantityPerOrder = model.MaxQuantityPerOrder;
                extra.ReadyTime = model.ReadyTime;

                return await _app.Update(extra) == true ? Ok("Extra has been updated.") : BadRequest("Something went wrong, try again.");
            }   

            return BadRequest("Fill all required fields.");
        }

        /// <summary>
        /// Remove an extra.
        /// </summary>
        /// <param name="extraId">Extra Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpDelete("extra/{extraId}")]
        public async Task<ActionResult> Remove(Guid extraId)
        {
            if(extraId == Guid.Empty)
            {
                return BadRequest("Insert a valid ID.");
            }

            return await _app.Remove(extraId) == true ? Ok("Extra has been removed.") : BadRequest("Extra wasn't found or something went wrong!");
        }
    }
}