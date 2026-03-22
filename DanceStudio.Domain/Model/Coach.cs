using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class Coach
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int Group_ID { get; set; }




    // Навігаційна властивість
    public virtual ICollection<Group>? Groups { get; set; }
    public virtual ICollection<Booking>? Bookings { get; set; }   // зв'язок з Bookings (1:M)
    public virtual ICollection<Cancellation>? Cancellations { get; set; } // зв'язок з Cancellations (1:M)

    
}

