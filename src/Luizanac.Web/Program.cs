using System;
using Luizanac.Infra.Http;
using Luizanac.Service.Exchange;
using Luizanac.Service.Interfaces;

namespace Luizanac.Web
{
    public class Program
    {
        public static void Main()
        {
            var appUrl = "http://localhost:5000/";
            string[] prefixes = { appUrl };
            var webApplication = new WebApplication(prefixes);

            webApplication.Configure(x =>
            {
                x.Add<IExchangeService, ExchangeService>();
            });

            Console.WriteLine("Application running on: " + appUrl);
            webApplication.Init();
        }
    }
}