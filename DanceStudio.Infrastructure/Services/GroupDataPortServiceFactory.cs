using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Services;

public class GroupDataPortServiceFactory : IDataPortServiceFactory<Group>
{
    private readonly DanceStudioContext _context;

    public GroupDataPortServiceFactory(DanceStudioContext context)
    {
        _context = context;
    }

    public IImportService<Group> GetImportService(string contentType)
    {
        if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            return new GroupImportService(_context);

        throw new NotImplementedException($"Імпорт для типу {contentType} не реалізовано");
    }

    public IExportService<Group> GetExportService(string contentType)
    {
        if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            return new GroupExportService(_context);

        throw new NotImplementedException($"Експорт для типу {contentType} не реалізовано");
    }
}