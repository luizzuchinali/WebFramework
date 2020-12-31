using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Luizanac.Infra.Http;

namespace Luizanac.Web.Controllers
{
    public class HomeController : ApplicationController
    {
        public async Task<string> Index()
        {
            var resourceName = "Luizanac.Web.Views.Home.Index.html";
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(resourceStream);
            var content = await streamReader.ReadToEndAsync();
            return content;
        }
    }
}