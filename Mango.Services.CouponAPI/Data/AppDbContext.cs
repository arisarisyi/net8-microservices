using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Coupon>().HasData(new Coupon
        {
            CouponId = 1,
            CouponCode = "COUPON_CODE",
            DiscountAmount = 10,
            MinAmount = 20
        });

        builder.Entity<Coupon>().HasData(new Coupon
        {
            CouponId = 2,
            CouponCode = "COUPON_CODE22",
            DiscountAmount = 20,
            MinAmount = 30
        });
    }
}