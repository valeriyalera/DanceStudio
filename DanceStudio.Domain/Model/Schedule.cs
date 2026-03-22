
using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class Schedule
{
    public int Id { get; set; }

    public int DayOfWeek { get; set; }

    //public TimeSpan StartTime { get; set; }
    public string StartTime { get; set; } = string.Empty;  // ← зміни на string

    public int RoomNumber { get; set; }

    public int Group_ID { get; set; }

    // Навігаційні властивості
    public virtual Group? Group { get; set; }
    public virtual ICollection<Booking>? Bookings { get; set; }        // 1 : M з Booking
    public virtual ICollection<Cancellation>? Cancellations { get; set; } // 1 : M з Cancellation
}

