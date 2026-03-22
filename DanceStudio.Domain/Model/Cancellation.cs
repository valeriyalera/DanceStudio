using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class Cancellation
{
    public int Id { get; set; }

    public int Coach_ID { get; set; }

    public int Schedule_ID { get; set; }

    public int Reason { get; set; } // 1-хвороба, 2-особисті обставини, 3-технічні причини, etc.

    // Навігаційні властивості
    public virtual Coach? Coach { get; set; }
    public virtual Schedule? Schedule { get; set; }
}
