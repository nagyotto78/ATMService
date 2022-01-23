using ATMService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ATMService.DAL
{

    /// <summary>
    /// Database context for ATM handling
    /// </summary>
    public class ATMDbContext : DbContext
    {
        /// <summary>
        /// Appsettings connection string key
        /// </summary>
        public const string CONNECTION_STRING_KEY = "ATMDbConnection";

        public DbSet<MoneyDenomination> MoneyDenominations { get; set; }

        public ATMDbContext(DbContextOptions<ATMDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Model creation settings
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoneyDenomination>()
                .HasIndex(i => new { i.Value })
                .IsUnique(true);

            modelBuilder.Entity<MoneyDenomination>()
            .HasData(
                new MoneyDenomination()
                {
                    Id = 1,
                    Key = "1000",
                    Value = 1000
                },
                new MoneyDenomination()
                {
                    Id = 2,
                    Key = "2000",
                    Value = 2000
                },
                new MoneyDenomination()
                {
                    Id = 3,
                    Key = "5000",
                    Value = 5000
                },
                new MoneyDenomination()
                {
                    Id = 4,
                    Key = "10000",
                    Value = 10000
                },
                new MoneyDenomination()
                {
                    Id = 5,
                    Key = "20000",
                    Value = 20000
                });

            modelBuilder.Entity<MoneyStorage>()
                .HasIndex(i => new { i.MoneyDenominationId })
                .IsUnique(true);

        }

    }
}
