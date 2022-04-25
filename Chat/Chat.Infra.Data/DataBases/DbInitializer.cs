using Chat.Infra.Data.DataBases.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Chat.Infra.Data.DataBases
{
    [ExcludeFromCodeCoverage]
    public static class DbInitializer
    {
        public static async Task SeedData(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();
        }        
    }
}