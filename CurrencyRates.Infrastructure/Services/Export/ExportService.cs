using ClosedXML.Excel;
using CurrencyRates.Application.DTOs;
using CurrencyRates.Application.Interfaces;

namespace CurrencyRates.Infrastructure.Services.Export;

public class ExportService : IExportService
{
    private readonly FileUtil _fileUtil;
    public ExportService(FileUtil fileUtil) { _fileUtil = fileUtil; }
    public async Task Export<TItem>(string fileName, IEnumerable<TItem> items)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Data");

        var properties = typeof(TItem).GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = GetDisplayName(properties[i]);
        }

        int row = 2;
        foreach (var item in items)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(item);
                worksheet.Cell(row, i + 1).Value = value?.ToString();
            }
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var bytes = stream.ToArray();

        await _fileUtil.SaveAs(fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", bytes);
    }

    private string GetDisplayName(System.Reflection.PropertyInfo property)
    {
        return property.Name;
    }
}