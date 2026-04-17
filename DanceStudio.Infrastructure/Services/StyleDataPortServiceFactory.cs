
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Services;

public class StyleDataPortServiceFactory : IDataPortServiceFactory<Style>
{
    private readonly DanceStudioContext _context;

    public StyleDataPortServiceFactory(DanceStudioContext context)
    {
        _context = context;
    }

    public IImportService<Style> GetImportService(string contentType)
    {
        if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            return new StyleImportService(_context);

        throw new NotImplementedException($"Імпорт для типу {contentType} не реалізовано");
    }

    public IExportService<Style> GetExportService(string contentType)
    {
        if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            return new StyleExportService(_context);

        throw new NotImplementedException($"Експорт для типу {contentType} не реалізовано");
    }
}
