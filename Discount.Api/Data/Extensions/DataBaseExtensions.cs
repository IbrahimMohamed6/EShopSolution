using Discount.Api.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Extensions;

public static class DataBaseExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();

        return app;
    }
}