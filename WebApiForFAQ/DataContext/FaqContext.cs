using Microsoft.EntityFrameworkCore;
using WebApiForFAQ.Models;

namespace WebApiForFAQ.DataContext
{
    public class FaqContext : DbContext
    {
        public DbSet<FaqQuestion> FaqQuestions { get; set; }
        public DbSet<FaqGroup> FaqGroups { get; set; }

        public FaqContext(DbContextOptions<FaqContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var fg1 = new FaqGroup { FaqGroupId = 1, Title = "Introduction" };
            var fg2 = new FaqGroup { FaqGroupId = 2, Title = "Who is this for?" };

            var fq1 = new FaqQuestion
            {
                Id = 1,
                Question = "Why is Red Hat making .NET Core 2.0 available?",
                Answer = ".NET Core on Red Hat platforms complements Java EE in the enterprise.",
                FaqGroupId = 1
            };
            var fq2 = new FaqQuestion
            {
                Id = 2,
                Question = "How does .NET Core 2.0 fit into Red Hat plans/portfolio?",
                Answer = ".NET Core is an important addition to the massive and growing list of " +
                "open source development tools and platforms.",
                FaqGroupId = 1
            };
            var fq3 = new FaqQuestion
            {
                Id = 3,
                Question = "Why would I want to run my .NET apps on Red Hat platforms?",
                Answer = "Containers and microservices are the main reasons. Red Hat will be able " +
                "to differentiate .NET Core to run natively on containers that support an open " +
                "hybrid cloud strategy.",
                FaqGroupId = 2
            };

            modelBuilder.Entity<FaqGroup>().HasData(fg1, fg2);

            modelBuilder.Entity<FaqQuestion>(entity =>
            {
                entity.HasOne(d => d.FaqGroup)
                    .WithMany(p => p.Questions)
                    .HasForeignKey("FaqGroupId");
            });

            modelBuilder.Entity<FaqQuestion>().HasData(fq1, fq2, fq3);

            base.OnModelCreating(modelBuilder);
        }
    }
}
