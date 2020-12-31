namespace Luizanac.Service.Interfaces
{
    public interface IExchangeService
    {
        decimal Calculate(string currencyOrigin, string currencyDestiny, decimal value);
    }
}