using System;
using Luizanac.Service.Interfaces;

namespace Luizanac.Service.Exchange
{
    public class ExchangeService : IExchangeService
    {
        private readonly Random _random = new Random();

        public decimal Calculate(string currencyOrigin, string currencyDestiny, decimal value) =>
            value * (decimal)_random.NextDouble();
    }
}