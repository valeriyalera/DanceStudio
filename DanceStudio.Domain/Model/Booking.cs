using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;
public partial class Booking
{
    public int Id { get; set; }

    public int User_ID { get; set; }

    public int Schedule_ID { get; set; }

    public string Status { get; set; } = string.Empty; // 0-заплановано, 1-підтверджено, 2-скасовано, 3-відвідано

    public int Coach_ID { get; set; }

    public DateTime Date { get; set; }

    // Навігаційні властивості
    public virtual User? User { get; set; }
    public virtual Schedule? Schedule { get; set; }
    public virtual Coach? Coach { get; set; }
}

