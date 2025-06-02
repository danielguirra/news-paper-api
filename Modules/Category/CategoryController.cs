using Microsoft.AspNetCore.Mvc;
using Models;
using Modules.Auth;

namespace Modules.Category;

[ApiController]
[Route("/api/[controller]")]
public class CategoryController(CategoryService categoryService) : ControllerBase
{
    [HttpPost]
    [AuthRequired("admin")]
    public async Task<IActionResult> Create(CategoryBodyModel categoryBody)
    {
        var created = await categoryService.Create(new CategoryModel { Name = categoryBody.Name });
        return Created("", new { created.Id });
    }

    [HttpGet("{id}/news")]
    public async Task<IActionResult> ListNews(Guid id)
    {
        var news = await categoryService.GetManyNewsOnCategoryId(id);
        return Ok(news);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOneCategory(Guid id)
    {
        var category = await categoryService.GetCategory(id);
        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await categoryService.GetAllCategories();
        return Ok(categories);
    }

    [HttpDelete("{id}/inactive")]
    [AuthRequired("admin")]
    public async Task<IActionResult> Inactive(Guid id)
    {
        await categoryService.Inactive(id);
        return Ok(new { message = "Categoria inativada." });
    }
}
