using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Wyrmrest.Web.Statics;

namespace Wyrmrest.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            System.Diagnostics.Debug.WriteLine(Strings.DotNetConnectionString);
        }
    }
}
