using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateSolution.Repository.Domain;

namespace TemplateSolution.Repository
{
    public class TemplateDBContext : DbContext
    {
        public TemplateDBContext(DbContextOptions<TemplateDBContext> options)
            : base(options)
        { }

        /*     protected override void OnModelCreating(ModelBuilder modelBuilder)
             {
                 modelBuilder.Entity<DataPrivilegeRule>()
                     .HasKey(c => new { c.Id });
             }*/

        public virtual DbSet<RawData> rawData { get; set; }

    }
}
