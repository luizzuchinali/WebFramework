using System.Net;
using System.Threading.Tasks;
using Luizanac.Infra.Http;

namespace Luizanac.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        public async Task<string> Index() => await View();

    }
}