using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Data.Repositories;
using Product.Models;
using Product.Services;

namespace Product.Controllers
{
    /// <summary>
    /// Product management.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _app;
        private readonly IExtraRepo _extra;
        public ProductController(
            IProductRepo app,
            IExtraRepo extra
        )
        {
            _app = app;
            _extra = extra;
        }

        /// <summary>
        /// List all products.
        /// </summary>
        /// <response code="200">Ok.</response>
        [HttpGet("product")]
        public async Task<ActionResult<List<DTO.Product>>> All() => Ok(await _app.All());

        /// <summary>
        /// List all category products.
        /// </summary>
        /// <param name="categoryId">Category Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        [HttpGet("product/category/{categoryId}")]
        public async Task<ActionResult<List<DTO.Product>>> AllFromCategory(Guid categoryId) => Ok(await _app.All(categoryId));

        /// <summary>
        /// Get a product by ID.
        /// </summary>
        /// <param name="productId">Product Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("product/i/{productId}")]
        public async Task<ActionResult<VMProdExtras>> GetId(Guid productId)
        {
            if(productId == Guid.Empty)
            {
                return BadRequest("Insert a valid ID.");
            }

            var product = await _app.Get(productId);

            if(product == null)
            {
                NotFound("Product not found."); 
            }

            var extras = await _extra.All(product.Id);


            return Ok(new VMProdExtras
            {
                Product =  product,
                Extras = extras
            });
        }

        /// <summary>
        /// Get a product by name.
        /// </summary>
        /// <param name="productName">Product name.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid product name.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("product/n/{productName}")]
        public async Task<ActionResult<DTO.Product>> GetName(string productName)
        {
            if(string.IsNullOrEmpty(productName))
            {
                return BadRequest("Insert a valid product name.");
            }

            var product = await _app.Get(productName);

            if(product == null)
            {
                NotFound("Product not found."); 
            }

            var extras = await _extra.All(product.Id);


            return Ok(new VMProdExtras
            {
                Product =  product,
                Extras = extras
            });
        }

        /// <summary>
        /// Add a product.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        /// <response code="401">Not Authorized.</response>
        [HttpPost("product")]
        public async Task<ActionResult> Add(VMProduct model, IFormFile image)
        {
            if(ModelState.IsValid)
            {
                DTO.Product product = new()
                {
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    Description = model.Description,
                    ReadyTime = model.ReadyTime,
                    Price = model.Price,
                    Image = await UploadFile.Upload(image),
                    Quantity = model.Quantity,
                    Created = DateTimeOffset.Now
                };

                return await _app.Add(product) == true ? Ok("Product has been added.") : BadRequest("Somenthing went wrong, try again.");
            }

            return BadRequest("Fill all required fields.");
        }

        /// <summary>
        /// Disabe/Enable a product.
        /// </summary>
        /// <param name="productId">Product Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpPut("product/{productId}")]
        public async Task<ActionResult> ChangeStatus(Guid productId)
        {
            if(productId == Guid.Empty)
            {
                return BadRequest("Insert a valid ID.");
            }

            return await _app.ChangeStatus(productId) == true ? Ok("Product status has been changed.") : BadRequest("Product wasn't found or something went wrong!");
        }

        /// <summary>
        /// Update a product.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpPut("product")]
        public async Task<ActionResult> Update(DTO.Product model)
        {
            if(ModelState.IsValid)
            {
                var product = await _app.Get(model.Id);

                if(product == null)
                {
                    return NotFound("Product not found.");
                }

                product.CategoryId = model.CategoryId;
                product.Description = model.Description;
                product.Image = model.Image;
                product.Name = model.Name;
                product.Price = model.Price;
                product.Quantity = model.Quantity;
                product.ReadyTime = model.ReadyTime;

                return await _app.Update(product) == true ? Ok("Product has been updated.") : BadRequest("Something went wrong.");
            }   

            return BadRequest("Fill all required fields.");
        }

        /// <summary>
        /// Remove a product.
        /// </summary>
        /// <param name="productId">Product Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpDelete("product/{productId}")]
        public async Task<ActionResult> Remove(Guid productId)
        {
            if(productId == Guid.Empty)
            {
                return BadRequest("Insert a valid ID.");
            }

            return await _app.Remove(productId) == true ? Ok("Product has been removed.") : BadRequest("Product wasn't found or something went wrong!");
        }
    }
}