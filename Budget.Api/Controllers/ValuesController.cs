using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Budget.Data.Models;
using System.Threading.Tasks;
using Budget.Services.Contracts;
using Budget.Services.Models;
using AutoMapper;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public ValuesController(UserManager<User> userManager, ICategoryService categoryService, IMapper mapper)
        {
            this.userManager = userManager;
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<CategoryInfoServiceModel>> Get()
        {
            var categories = await this.categoryService.GetAllCategoriesInfo();

            return categories;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
