using Chat.Domain.Entities;
using Chat.Infra.Data.DataBases.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Chat.Infra.Data.DataBases.Context
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        ///     Used for migration.
        /// </summary>
        public ApplicationDbContext() : base(new DbContextOptionsBuilder()
                .UseSqlServer("Server=localhost,1433;Database=ChatDB;Integrated Security=False;User Id=sa;Password=Chat_123;MultipleActiveResultSets=true", optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name))
                .Options)
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<ChatMessage> ChatMessage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChatMessageMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}