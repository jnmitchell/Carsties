using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions options) : base(options)
        {
        }

     //   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     //=> optionsBuilder.UseNpgsql("DefaultConnection");

     //   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     //   {
     //       var cns = optionsBuilder.Options.GetConnectionString("DefaultConnection")
     //   }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var xx = "Host=localhost; Port=5433; User Id=postgres; Password=postgresspw; Database=auctions";
        //    optionsBuilder.UseNpgsql(xx);
        //}
        public DbSet<Auction> Auctions { get; set; }

    }
}
