using Microsoft.EntityFrameworkCore;
using NationEase.Models;

namespace NationEase.Data;

public class NationDbContext : DbContext
{
    public NationDbContext(
        DbContextOptions<NationDbContext> options)
        : base(options)
    {
    }

    public DbSet<State> States => Set<State>();

    public DbSet<City> Cities => Set<City>();

    public DbSet<NationCity> NationCities => Set<NationCity>();
}