
using ClosedXML.Excel;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DanceStudio.Infrastructure.Services;

public class StyleImportService : IImportService<Style>
{
    private readonly DanceStudioContext _context;

    public StyleImportService(DanceStudioContext context)
    {
        _context = context;
    }

    public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));

        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RowsUsed().Skip(1);

        foreach (var row in rows)
        {
            var styleName = row.Cell(2).GetString();
            if (!string.IsNullOrWhiteSpace(styleName))
            {
                _context.Styles.Add(new Style { Name = styleName });
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
