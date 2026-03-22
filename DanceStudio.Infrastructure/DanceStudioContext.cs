
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;

namespace DanceStudio.Infrastructure;

public partial class DanceStudioContext : DbContext
{
    public DanceStudioContext()
    {
    }

    public DanceStudioContext(DbContextOptions<DanceStudioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgeCategory> AgeCategories { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Cancellation> Cancellations { get; set; }
    public virtual DbSet<Coach> Coaches { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Schedule> Schedules { get; set; }
    public virtual DbSet<Style> Styles { get; set; }
    public virtual DbSet<Subscription> Subscriptions { get; set; }
    public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DanceStudio;Username=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // AgeCategory
        modelBuilder.Entity<AgeCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AgeCategories_pkey");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title).HasColumnName("Title");
            entity.Property(e => e.MinAge).HasColumnName("MinAge");
            entity.Property(e => e.MaxAge).HasColumnName("MaxAge");
        });

        // Booking
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Booking_pkey");
            entity.ToTable("Booking");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.User_ID).HasColumnName("User_ID");
            entity.Property(e => e.Schedule_ID).HasColumnName("Schedule_ID");
            entity.Property(e => e.Coach_ID).HasColumnName("Coach_ID");
            entity.Property(e => e.Status).HasColumnName("Status");
            entity.Property(e => e.Date).HasColumnName("Date");
        });

        // Cancellation
        modelBuilder.Entity<Cancellation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Cancellations_pkey");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Coach_ID).HasColumnName("Coach_ID");
            entity.Property(e => e.Schedule_ID).HasColumnName("Schedule_ID");
            entity.Property(e => e.Reason).HasColumnName("Reason");
        });

        // Coach
        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Coaches_pkey");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasColumnName("FirstName");
            entity.Property(e => e.LastName).HasColumnName("LastName");
            entity.Property(e => e.Group_ID).HasColumnName("Group_ID");
        });

        // Group
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Groups_pkey");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Style_ID).HasColumnName("Style_ID");
            entity.Property(e => e.AgeCategories_ID).HasColumnName("AgeCategories_ID");
            entity.Property(e => e.Level).HasColumnName("Level");
            entity.Property(e => e.Coach_ID).HasColumnName("Coach_ID");
        });

        // Payment
modelBuilder.Entity<Payment>(entity =>
{
    entity.HasKey(e => e.Id).HasName("Payment_pkey");
    entity.ToTable("Payments");  // ← назва таблиці в БД
    entity.Property(e => e.Id).HasColumnName("ID");
    entity.Property(e => e.Booking_ID).HasColumnName("Booking_ID");  // ← як в БД
    entity.Property(e => e.Amount).HasColumnName("Amount");
    entity.Property(e => e.PaymentMethod).HasColumnName("PaymentMethod");
    entity.Property(e => e.Status).HasColumnName("Status");
    entity.Property(e => e.PaymentDate).HasColumnName("PaymentDate");
    entity.Property(e => e.Subscription_ID).HasColumnName("Subscription_ID");
});

        // Schedule
        // Schedule
        // modelBuilder.Entity<Schedule>(entity =>
        // {
        //     entity.HasKey(e => e.Id).HasName("Schedules_pkey");
        //     entity.ToTable("Schedules");  // ← ЗМІНИ на Schedules
        //     entity.Property(e => e.Id).HasColumnName("ID");
        //     entity.Property(e => e.DayOfWeek).HasColumnName("DayOfWeek");
        //     entity.Property(e => e.StartTime)
        //         .HasConversion(
        //             v => v.ToString(),
        //             v => TimeSpan.Parse(v))
        //         .HasColumnName("StartTime");
        //     entity.Property(e => e.RoomNumber).HasColumnName("RoomNumber");
        //     entity.Property(e => e.Group_ID).HasColumnName("Group_ID");
        // });

// Schedule
modelBuilder.Entity<Schedule>(entity =>
{
    entity.HasKey(e => e.Id).HasName("Schedules_pkey");
    entity.ToTable("Schedules");
    entity.Property(e => e.Id).HasColumnName("ID");
    entity.Property(e => e.DayOfWeek).HasColumnName("DayOfWeek");
    entity.Property(e => e.StartTime).HasColumnName("StartTime");  // ← без конвертера
    entity.Property(e => e.RoomNumber).HasColumnName("RoomNumber");
    entity.Property(e => e.Group_ID).HasColumnName("Group_ID");
});

        // Style
        modelBuilder.Entity<Style>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Styles_pkey");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnName("Name");
        });

        // Subscription
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Subscription_pkey");
            entity.ToTable("Subscriptions");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.User_ID).HasColumnName("User_ID");
            entity.Property(e => e.TotalLessons).HasColumnName("TotalLessons");
            entity.Property(e => e.RemainingLessons).HasColumnName("RemainingLessons");
            entity.Property(e => e.Payment_ID).HasColumnName("Payment_ID");
            entity.Property(e => e.Subscriptions_type).HasColumnName("Subscription_type");
        });

        // SubscriptionType
        modelBuilder.Entity<SubscriptionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Subscription_type_pkey");
            entity.ToTable("Subscriptions_type");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnName("Name");
            entity.Property(e => e.Amount).HasColumnName("Amount");
            entity.Property(e => e.Description).HasColumnName("Description");
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FullName).HasColumnName("FullName");
            entity.Property(e => e.BirthDate).HasColumnName("BirthDate");
            entity.Property(e => e.LastName).HasColumnName("LastName");
        });

        // ========== НАЛАШТУВАННЯ ЗВ'ЯЗКІВ ==========

        // Payment → Subscription (1:1)
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Subscription)
            .WithOne(s => s.Payment)
            .HasForeignKey<Payment>(p => p.Subscription_ID);

        // Coach → Groups (1:M)
        modelBuilder.Entity<Coach>()
            .HasMany(c => c.Groups)
            .WithOne(g => g.Coach)
            .HasForeignKey(g => g.Coach_ID);

        // Group → Style (M:1)
        modelBuilder.Entity<Group>()
            .HasOne(g => g.Style)
            .WithMany(s => s.Groups)
            .HasForeignKey(g => g.Style_ID);

        // Group → AgeCategory (M:1)
        modelBuilder.Entity<Group>()
            .HasOne(g => g.AgeCategory)
            .WithMany(a => a.Groups)
            .HasForeignKey(g => g.AgeCategories_ID);

        // Schedule → Group (M:1)
        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Group)
            .WithMany(g => g.Schedules)
            .HasForeignKey(s => s.Group_ID);

        // Booking → Schedule (M:1)
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Schedule)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.Schedule_ID);

        // Booking → User (M:1)
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.User_ID);

        // Booking → Coach (M:1)
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Coach)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.Coach_ID);

        // Cancellation → Schedule (M:1)
        modelBuilder.Entity<Cancellation>()
            .HasOne(c => c.Schedule)
            .WithMany(s => s.Cancellations)
            .HasForeignKey(c => c.Schedule_ID);

        // Cancellation → Coach (M:1)
        modelBuilder.Entity<Cancellation>()
            .HasOne(c => c.Coach)
            .WithMany(co => co.Cancellations)
            .HasForeignKey(c => c.Coach_ID);

        // Subscription → User (M:1)
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.User_ID);

        // Subscription → SubscriptionType (M:1)
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.SubscriptionType)
            .WithMany(st => st.Subscriptions)
            .HasForeignKey(s => s.Subscriptions_type);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
