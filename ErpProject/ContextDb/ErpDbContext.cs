using System;
using Microsoft.EntityFrameworkCore;

namespace ErpProject.ContextDb;

public class ErpDbContext: DbContext
{
    public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
