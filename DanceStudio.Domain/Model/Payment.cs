
using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class Payment
{
    public int Id { get; set; }

    public int Booking_ID { get; set; }  // ← Booking_ID, а не BookingId

    public decimal Amount { get; set; }

    public string? PaymentMethod { get; set; }

public string Status { get; set; } = string.Empty;

    public DateTime PaymentDate { get; set; }

    public int Subscription_ID { get; set; }  // ← Subscription_ID

    public virtual Subscription? Subscription { get; set; }
}
