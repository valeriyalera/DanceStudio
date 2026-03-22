
using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class Group
{
    public int Id { get; set; }

    public int Style_ID { get; set; }

    public int AgeCategories_ID { get; set; }

    public string Level { get; set; } = "Початківець";

    public int Coach_ID { get; set; }

    // Навігаційні властивості
    public virtual Style? Style { get; set; }
    public virtual AgeCategory? AgeCategory { get; set; }
    public virtual Coach? Coach { get; set; }  // ← багато груп → один тренер
    public virtual ICollection<Schedule>? Schedules { get; set; }
}
