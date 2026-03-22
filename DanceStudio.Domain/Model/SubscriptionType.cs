using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class SubscriptionType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Amount { get; set; }

    public string? Description { get; set; }

    // Навігаційна властивість
    public virtual ICollection<Subscription>? Subscriptions { get; set; }
}
