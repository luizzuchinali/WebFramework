using System.Threading.Tasks;
using Luizanac.Infra.Http;
using Luizanac.Service.Interfaces;
using Luizanac.Web.ViewModels;

namespace Luizanac.Web.Controllers
{
    public class ExchangeController : ControllerBase
    {
        private IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        public async Task<string> Calculate(string originCurrency, string destinyCurrency, decimal value)
        {
            var finalValue = _exchangeService.Calculate(originCurrency, destinyCurrency, value);
            var viewModel = new CalculationExchangeViewModel
            {
                OriginCurrency = originCurrency,
                OriginCurrencyValue = value,
                DestinyCurrency = destinyCurrency,
                DestinyCurrencyValue = finalValue,
            };

            return await View(viewModel);
        }

        public async Task<string> Calculate(string destinyCurrency, decimal value)
            => await Calculate("BRL", destinyCurrency, value);

    }
}