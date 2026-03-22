using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class AgeCategory
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    // Одна вікова категорія може бути в багатьох групах (1 : M)
    public virtual ICollection<Group>? Groups { get; set; }
}
