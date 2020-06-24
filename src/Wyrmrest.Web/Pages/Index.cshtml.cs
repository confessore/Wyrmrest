using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Wyrmrest.Web.Services.Interfaces;

namespace Wyrmrest.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        readonly IMariaService _maria;

        public IndexModel(
            ILogger<IndexModel> logger,
            IMariaService maria)
        {
            _logger = logger;
            _maria = maria;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}
