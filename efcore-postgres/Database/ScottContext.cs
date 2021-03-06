﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using efcore_postgres.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace efcore_postgres
{
    public class ScottContext : DbContext, IScottContext
    {
        public ScottContext(DbContextOptions<ScottContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }

        public Task<List<Employee>> GetAllAsync() {
            return Employees.ToListAsync();
        }

        public Task<Employee> GetAsync(int id) {
            return Employees.FindAsync(id);
        }

        public void AddRec(Employee employee) {
            Employees.Add(employee);
        }

        public void RemoveRec(Employee employee)
        {
            Employees.Remove(employee);
        }

        public Task<int> SaveAsync()
        {
            return SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Salary)
                    .HasColumnName("salary")
                    .HasColumnType("numeric(18,2)");
            });
        }
    }
}
