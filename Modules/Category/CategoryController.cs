using Microsoft.AspNetCore.Mvc;
using Modules.Auth;

namespace Modules.Category;

[ApiController]
[Route("/api/[controller]")]
public class CategoryController(CategoryService categoryService, AuthService authService)
    : ControllerBase
{
    [HttpPost]
    [AuthRequired("boss")]
    public async Task<IActionResult> Create(CategoryBodyModel categoryBody)
    {
        var created = await categoryService.Create(new CategoryModel { Name = categoryBody.Name });
        return created is null ? BadRequest() : Created("", new { created.Id });
    }

    [HttpGet]
    [AuthRequired("admin")]
    public async Task<IActionResult> List(Guid id)
    {
        return Ok(await categoryService.GetManyNewsOnCategoryId(id));
    }
}
