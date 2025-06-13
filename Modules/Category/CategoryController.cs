using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
    [EnableRateLimiting("authenticated")]
    public async Task<IActionResult> Create(CategoryBodyModelDto categoryBody)
    {
        CategoryModel created = await categoryService.Create(
            new CategoryModel { Name = categoryBody.Name }
        );
        return Created("", new { created.Id });
    }

    [HttpGet("{id}/news")]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> ListNews(Guid id)
    {
        return Ok(await categoryService.GetManyNewsOnCategoryId(id));
    }

    [HttpGet("{id}")]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> GetOneCategory(Guid id)
    {
        return Ok(await categoryService.GetCategory(id));
    }

    [HttpGet]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> GetAllCategories()
    {
        return Ok(await categoryService.GetAllCategories());
    }

    [HttpDelete("{id}/inactive")]
    [AuthRequired("admin")]
    [EnableRateLimiting("authenticated")]
    public async Task<IActionResult> Inactive(Guid id)
    {
        await categoryService.Inactive(id);
        return Ok(new { message = "Categoria inativada." });
    }
}
