using ClosedXML.Excel;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DanceStudio.Infrastructure.Services;

public class GroupExportService : IExportService<Group>
{
    private readonly DanceStudioContext _context;

    public GroupExportService(DanceStudioContext context)
    {
        _context = context;
    }

    public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanWrite)
            throw new ArgumentException("Потік не підтримує запис", nameof(stream));

        var groups = await _context.Groups
            .Include(g => g.Style)
            .Include(g => g.AgeCategory)
            .Include(g => g.Coach)
            .ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Групи");

        // Заголовки
        worksheet.Cell(1, 1).Value = "Стиль";
        worksheet.Cell(1, 2).Value = "Вікова категорія";
        worksheet.Cell(1, 3).Value = "Рівень";
        worksheet.Cell(1, 4).Value = "Ім'я тренера";
        worksheet.Cell(1, 5).Value = "Прізвище тренера";

        // Запис даних
        int row = 2;
        foreach (var group in groups)
        {
            worksheet.Cell(row, 1).Value = group.Style?.Name ?? "";
            worksheet.Cell(row, 2).Value = group.AgeCategory?.Title ?? "";
            worksheet.Cell(row, 3).Value = group.Level switch
            {
                0 => "Початківець",
                1 => "Середній",
                2 => "Просунутий",
                _ => "Невідомо"
            };
            worksheet.Cell(row, 4).Value = group.Coach?.FirstName ?? "";
            worksheet.Cell(row, 5).Value = group.Coach?.LastName ?? "";
            row++;
        }

        workbook.SaveAs(stream);
    }
}