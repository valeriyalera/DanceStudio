
using ClosedXML.Excel;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DanceStudio.Infrastructure.Services;

public class StyleExportService : IExportService<Style>
{
    private readonly DanceStudioContext _context;

    public StyleExportService(DanceStudioContext context)
    {
        _context = context;
    }

    public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanWrite)
            throw new ArgumentException("Потік не підтримує запис", nameof(stream));

        var styles = await _context.Styles.ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Styles");

        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Назва стилю";

        for (int i = 0; i < styles.Count; i++)
        {
            worksheet.Cell(i + 2, 1).Value = styles[i].Id;
            worksheet.Cell(i + 2, 2).Value = styles[i].Name;
        }

        worksheet.Columns().AdjustToContents();
        workbook.SaveAs(stream);
    }
}
