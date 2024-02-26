using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;


namespace WebApplication4.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IConfiguration _configuration; 
        public LibraryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return Content("Welcome to the Library!");
        }

        public async Task<IActionResult> Books()
        {
            var booksList = await System.IO.File.ReadAllTextAsync(_configuration["BooksFilePath"]);
            return Content(booksList);
        }

        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "current";
            }
            
            var userProfile = await System.IO.File.ReadAllTextAsync($"{_configuration["ProfilesDirectoryPath"]}/{id}.json");
            return Content(userProfile);
        }
    }
}
