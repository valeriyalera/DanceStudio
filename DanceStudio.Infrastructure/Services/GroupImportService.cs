using ClosedXML.Excel;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DanceStudio.Infrastructure.Services;

public class GroupImportService : IImportService<Group>
{
    private readonly DanceStudioContext _context;

    public GroupImportService(DanceStudioContext context)
    {
        _context = context;
    }

    public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));

        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1); // перший аркуш

        // заголовки: Стиль, Вікова категорія, Рівень, Ім'я тренера, Прізвище тренера
        var rows = worksheet.RowsUsed().Skip(1); // пропускаємо заголовок

        foreach (var row in rows)
        {
            var styleName = row.Cell(1).GetString().Trim();
            var ageCategoryTitle = row.Cell(2).GetString().Trim();
            var levelText = row.Cell(3).GetString().Trim();
            var coachFirstName = row.Cell(4).GetString().Trim();
            var coachLastName = row.Cell(5).GetString().Trim();

            if (string.IsNullOrEmpty(styleName) || string.IsNullOrEmpty(ageCategoryTitle) || string.IsNullOrEmpty(levelText))
                continue;

            // Пошук або створення стилю
            var style = await _context.Styles.FirstOrDefaultAsync(s => s.Name == styleName, cancellationToken);
            if (style == null)
            {
                style = new Style { Name = styleName };
                _context.Styles.Add(style);
            }

            // Пошук або створення вікової категорії
            var ageCategory = await _context.AgeCategories.FirstOrDefaultAsync(ac => ac.Title == ageCategoryTitle, cancellationToken);
            if (ageCategory == null)
            {
                ageCategory = new AgeCategory { Title = ageCategoryTitle };
                _context.AgeCategories.Add(ageCategory);
            }

            // Пошук або створення тренера
            var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.FirstName == coachFirstName && c.LastName == coachLastName, cancellationToken);
            if (coach == null)
            {
                coach = new Coach { FirstName = coachFirstName, LastName = coachLastName };
                _context.Coaches.Add(coach);
            }

            // Рівень: перетворюємо текст у число
            int level = levelText switch
            {
                "Початківець" => 0,
                "Середній" => 1,
                "Просунутий" => 2,
                _ => 0
            };

            var group = new Group
            {
                Style = style,
                AgeCategory = ageCategory,
                Level = level,
                Coach = coach
            };

            _context.Groups.Add(group);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}