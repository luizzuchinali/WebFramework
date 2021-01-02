using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Luizanac.Infra.Http;
using Luizanac.Service.Exchange;
using Luizanac.Service.Interfaces;

namespace Luizanac.Web.Controllers
{
    public class ExchangeController : ControllerBase
    {
        private IExchangeService _exchangeService;

        public ExchangeController()
        {
            _exchangeService = new ExchangeService();
        }

        public async Task<string> MXN()
        {
            var value = _exchangeService.Calculate("MXN", "BRL", 1);
            var content = await View();
            return content.Replace("{BRLValue}", value.ToString());
        }

        public async Task<string> USD()
        {
            var value = _exchangeService.Calculate("MXN", "BRL", 1);
            var content = await View();
            return content.Replace("{BRLValue}", value.ToString());
        }
    }
}