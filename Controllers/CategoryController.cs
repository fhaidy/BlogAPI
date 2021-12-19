using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext dbContext)
        {

            try
            {
                var categories = await dbContext.Categories.ToListAsync();
                var result = new ResultViewModel<List<Category>>(categories);
                return Ok(result);
            }
            catch
            {
                return StatusCode(
                    500, 
                    new ResultViewModel<List<Category>>("Falha Interna de Servidor")
                );
            }

            
        } 
            

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id, [FromServices] BlogDataContext dbContext)
        {
            try
            {
                var category = await dbContext
                        .Categories
                        .SingleOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(
                    500,
                    new ResultViewModel<List<Category>>("Falha Interna de Servidor")
                );
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel viewModel, 
            [FromServices] BlogDataContext dbContext)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

                var category = new Category
                {
                    Id = 0,
                    Name = viewModel.Name,
                    Slug = viewModel.Slug.ToLower(),
                };
                await dbContext.Categories.AddAsync(category);
                await dbContext.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    new ResultViewModel<List<Category>>("Não foi possível incluir a categoria.")
                );
            }
            catch
            {
                return StatusCode(
                    500,
                    new ResultViewModel<List<Category>>("Falha Interna de Servidor")
                );
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            int id, 
            [FromBody] EditorCategoryViewModel viewModel,
            [FromServices] BlogDataContext dbContext)
        {
            try
            {
                var category = await dbContext
                        .Categories
                        .SingleOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

                category.Name = viewModel.Name;
                category.Slug = viewModel.Slug;

                dbContext.Categories.Update(category);

                await dbContext.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    new ResultViewModel<List<Category>>("Não foi possível atualizar a categoria.")
                );
            }
            catch
            {
                return StatusCode(
                    500,
                    new ResultViewModel<List<Category>>("Falha Interna de Servidor")
                );
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
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

                dbContext.Categories.Remove(category);

                await dbContext.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    new ResultViewModel<List<Category>>("Não foi possível deletar a categoria.")
                );
            }
            catch
            {
                return StatusCode(
                    500,
                    new ResultViewModel<List<Category>>("Falha Interna de Servidor")
                );
            }
        }
    }
}
