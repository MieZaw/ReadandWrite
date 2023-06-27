using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace ReadandWrite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(IFormFile fileInput)
        {
            if (fileInput == null || fileInput.Length == 0)
            {
                ModelState.AddModelError("fileInput", "Please select a file.");
                return Page();
            }

            // Create the directory if it doesn't exist
            string directoryPath = Path.Combine(_environment.ContentRootPath, "Uploads");
            Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, fileInput.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileInput.CopyToAsync(stream);
            }

            // Process the file as needed
            // For example, read its contents:
            string fileContent = await System.IO.File.ReadAllTextAsync(filePath);

            // Pass the file content to the view if necessary
            ViewData["FileContent"] = fileContent;

            return Page();
        }
    }
}
