using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Product.Data.Repositories;
using Product.DTO;
using Product.Models;

namespace Product.Controllers
{
    /// <summary>
    /// Category management.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _app;
        public CategoryController(ICategoryRepo app)
        {
            _app = app;
        }

        /// <summary>
        /// List all categories.
        /// </summary>
        /// <response code="200">Ok.</response>
        [HttpGet("category")]
        public async Task<ActionResult<List<Category>>> All() => Ok(await _app.All());

        /// <summary>
        /// Get a category by ID.
        /// </summary>
        /// <param name="categoryId">Category Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("category/i/{categoryId}")]
        public async Task<ActionResult<Category>> GetId(Guid categoryId)
        {
            if(categoryId == Guid.Empty)
            {
                return BadRequest("Insert a valid ID.");
            }

            var category = await _app.Get(categoryId);
            return category == null ? NotFound("Category not found.") : Ok(category);
        }

        /// <summary>
        /// Get a category by name.
        /// </summary>
        /// <param name="categoryName">Category name.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid category name.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("category/n/{categoryName}")]
        public async Task<ActionResult<Category>> GetName(string categoryName)
        {
            if(string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Insert a valid category name.");
            }

            var category = await _app.Get(categoryName);
            return category == null ? NotFound("Category not found.") : Ok(category);
        }

        /// <summary>
        /// Add a category.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        /// <response code="401">Not Authorized.</response>
        [HttpPost("category")]
        public async Task<ActionResult> Add(VMCategory model)
        {
            if(ModelState.IsValid)
            {
                Category category = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Created = DateTimeOffset.Now
                };

                return await _app.Add(category) == true ? Ok("Category has been added.") : BadRequest("Something went wrong or category already exists.");
            }

            return BadRequest("Fill all required fields.");
        }

        /// <summary>
        /// Disable/Enable category.
        /// </summary>
        /// <param name="categoryId">Category Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpPut("category/{categoryId}")]
        public async Task<ActionResult> ChangeStatus(Guid categoryId)
        {
            if(categoryId == Guid.Empty)
            {
                return BadRequest("Insert a valid ID.");
            }

            return await _app.ChangeStatus(categoryId) == true ? Ok("Category status has been changed.") : BadRequest("Something went wrong.");
        }

        /// <summary>
        /// Update a category.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        /// <response code="401">Not Authorized.</response>
        [HttpPut("category")]
        public async Task<ActionResult> Update(Category model)
        {
            if(ModelState.IsValid)
            {
                var category = await _app.Get(model.Id);

                if(category == null)
                {
                    return NotFound("Category wasn't found.");
                }

                category.Description = model.Description;
                category.Name = model.Name;

                return await _app.Update(category) == true ? Ok("Category has been updated.") : BadRequest("SOmething went wrong.");
            }

            return BadRequest("Fill all required fields.");
        }

        /// <summary>
        /// Remove a category.
        /// </summary>
        /// <param name="categoryId">Category Id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid ID.</response>
        /// <response code="401">Not Authorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpDelete("category/{categoryId}")]
        public async Task<ActionResult> Remove(Guid categoryId)
        {
            if(categoryId == Guid.Empty)
            {
                return BadRequest("Insert a valid ID.");
            }

            return await _app.Remove(categoryId) == true ? Ok("Category has been removed.") : BadRequest("Something went wrong.");
        }
    }
}