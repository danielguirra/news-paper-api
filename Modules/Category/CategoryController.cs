using Microsoft.AspNetCore.Mvc;
using Modules.Auth;
using Modules.Category.Dto;
using Modules.Category.Model;
using Modules.Category.Service;

namespace Modules.Category.Controller;

[ApiController]
[Route("/api/category")]
public class CategoryController(CategoryService categoryService) : ControllerBase
{
    [HttpPost]
    [AuthRequired("admin")]
    public async Task<IActionResult> Create(CategoryBodyModelDto categoryBody)
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
