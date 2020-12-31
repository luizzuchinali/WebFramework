using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Luizanac.Infra.Http;
using Luizanac.Service.Exchange;
using Luizanac.Service.Interfaces;

namespace Luizanac.Web.Controllers
{
    public class ExchangeController : ApplicationController
    {
        private IExchangeService _exchangeService;

        public ExchangeController()
        {
            _exchangeService = new ExchangeService();
        }

        public async Task<string> MXN()
        {
            var value = _exchangeService.Calculate("MXN", "BRL", 1);
            var resourceName = "Luizanac.Web.Views.Exchange.MXN.html";
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(resourceStream);
            var content = await streamReader.ReadToEndAsync();
            content = content.Replace("{BRLValue}", value.ToString());
            return content;
        }

        public async Task<string> USD()
        {
            var value = _exchangeService.Calculate("MXN", "BRL", 1);
            var resourceName = "Luizanac.Web.Views.Exchange.USD.html";
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(resourceStream);
            var content = await streamReader.ReadToEndAsync();
            content = content.Replace("{BRLValue}", value.ToString());
            return content;
        }
    }
}