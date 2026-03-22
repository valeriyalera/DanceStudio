using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class Subscription
{
    public int Id { get; set; }

    public int User_ID { get; set; }

    public int TotalLessons { get; set; }

    public int RemainingLessons { get; set; }

    public int Payment_ID { get; set; }

    public int Subscriptions_type { get; set; } // 1-разовий, 2-абонемент, 3-місячний

    // Навігаційні властивості


      public virtual User? User { get; set; }
    public virtual Payment? Payment { get; set; }
    public virtual SubscriptionType? SubscriptionType { get; set; }
}

