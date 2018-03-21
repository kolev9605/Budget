using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Budget.Data.Models;
using System.Threading.Tasks;
using Budget.Services.Contracts;

namespace Budget.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ICategoryService categoryService;

        public ValuesController(UserManager<User> userManager, ICategoryService categoryService)
        {
            this.userManager = userManager;
            this.categoryService = categoryService;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var colors = this.categoryService.GetAllCategoryColors();

            return colors;
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
