using Budget.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [Authorize]
    [Route("Categories")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
            => Ok(await _categoryService.GetAllAsync());

        [HttpGet]
        [Route(nameof(GetAllPrimary))]
        public async Task<IActionResult> GetAllPrimary()
            => Ok(await _categoryService.GetAllPrimaryAsync());

        [HttpGet]
        [Route(nameof(GetAllSubcategories))]
        public async Task<IActionResult> GetAllSubcategories(int parentCategoryId)
            => Ok(await _categoryService.GetAllSubcategoriesByParentCategoryId(parentCategoryId));
    }
}
