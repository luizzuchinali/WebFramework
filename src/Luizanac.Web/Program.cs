using System;
using System.Threading.Tasks;
using Luizanac.Infra.Http;
using Luizanac.Service.Exchange;
using Luizanac.Service.Interfaces;

namespace Luizanac.Web
{
    public class Program
    {
        public static async Task Main()
        {
            var appUrl = "http://localhost:5000/";
            string[] prefixes = { appUrl };
            var webApplication = new WebApplication(prefixes);

            webApplication.Configure(container =>
            {
                container.Add(typeof(IExchangeService), typeof(ExchangeService));
            });

            Console.WriteLine("Application running on: " + appUrl);
            await webApplication.InitAsync();
        }
    }
}