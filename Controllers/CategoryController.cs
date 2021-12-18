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
            try
            {
                await dbContext.Categories.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return Created($"v1/categories/{model.Id}", model);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Não foi possível incluir a categoria.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha interna no servidor.");
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            int id, 
            [FromBody] Category model,
            [FromServices] BlogDataContext dbContext)
        {
            try
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
            catch (DbUpdateException)
            {
                return StatusCode(500, "Não foi possível incluir a categoria.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha interna no servidor.");
            }
        }


        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            int id,
            [FromServices] BlogDataContext dbContext)
        {
            try
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
            catch (DbUpdateException)
            {
                return StatusCode(500, "Não foi possível incluir a categoria.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha interna no servidor.");
            }
        }
    }
}
