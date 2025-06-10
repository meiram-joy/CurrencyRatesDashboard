namespace CurrencyRates.Application.Interfaces;

public interface IExportService
{
    Task Export<TItem>(string fileName, IEnumerable<TItem> items);
}