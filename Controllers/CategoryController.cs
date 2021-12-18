using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext dbContext) => 
            Ok(await dbContext.Categories.ToListAsync());

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id, [FromServices] BlogDataContext dbContext)
        {
            var category = await dbContext
                .Categories
                .SingleOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] Category model, 
            [FromServices] BlogDataContext dbContext)
        {
            await dbContext.Categories.AddAsync(model);
            await dbContext.SaveChangesAsync();
            return Created($"v1/categories/{model.Id}", model);
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            int id, 
            [FromBody] Category model,
            [FromServices] BlogDataContext dbContext)
        {
            var category = await dbContext
                .Categories
                .SingleOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            category.Name = model.Name;
            category.Slug = model.Slug;

            dbContext.Categories.Update(category);

            await dbContext.SaveChangesAsync();
            return Ok(category);
        }


        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            int id,
            [FromServices] BlogDataContext dbContext)
        {
            var category = await dbContext
                .Categories
                .SingleOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            dbContext.Categories.Remove(category);

            await dbContext.SaveChangesAsync();
            return Ok(category);
        }
    }
}
