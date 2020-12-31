using System;
using System.Threading.Tasks;
using Luizanac.Infra.Http;

namespace Luizanac.Web
{
    public class Program
    {
        public static async Task Main()
        {
            var appUrl = "http://localhost:5000/";
            string[] prefixes = { appUrl };
            var webApplication = new WebApplication(prefixes);

            Console.WriteLine("Server running on {0}", appUrl);
            await webApplication.InitAsync();
        }
    }
}