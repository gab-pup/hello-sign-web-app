using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HelloSignWebApp.Models;
using HelloSign;
using System.Linq;

namespace HelloSignWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private const string API_KEY = "7c70e16b22df5c3d2007e32ffcff119494866344b0a215ea0f16c68c53de681c";

        private const string CLIENT_ID = "f148769dfb2d9c0818cf9ee26f19d9b8";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var fileName = "file-example_PDF_1MB.pdf";

            //delete previously downloaded file
            System.IO.File.Delete($"C:\\Users\\miguelp\\source\\repos\\HelloSignWebApp\\HelloSignWebApp\\{fileName}");

            //download document to sign
            var webClient = new System.Net.WebClient();
            var remoteUri = "https://file-examples.com/wp-content/uploads/2017/10/";

            //saves by default to application startup path
            webClient.DownloadFile(remoteUri + fileName, fileName);

            //create signature request 
            var client = new Client(API_KEY);

            var request = new SignatureRequest
            {
                Subject = "My First embedded signature request",
                Message = "Awesome, right?",
                TestMode = true
            };
            request.AddSigner("lorenzos@pageuppeople.com", "Renzo McSign");
            request.AddFile($"C:\\Users\\miguelp\\source\\repos\\HelloSignWebApp\\HelloSignWebApp\\{fileName}");

            var response = client.CreateEmbeddedSignatureRequest(request, CLIENT_ID);

            return View(new HomeViewModel
            {
                ClientId = CLIENT_ID,
                SignUrl = client.GetSignUrl(response.Signatures.First().SignatureId).SignUrl
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
