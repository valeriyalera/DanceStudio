using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class User
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<Booking>? Bookings { get; set; }
    
    // Зв'язок з Subscriptions (один користувач → багато абонементів)
    public virtual ICollection<Subscription>? Subscriptions { get; set; }

}
