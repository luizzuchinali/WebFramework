using System.Net;
using System.Threading.Tasks;
using Luizanac.Infra.Http;

namespace Luizanac.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(HttpListenerContext httpContext) : base(httpContext)
        {
        }

        public async Task<string> Index() => await View();

    }
}