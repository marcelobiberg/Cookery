using CookeryApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CookeryApp.Models
{
    public class AppContextDB : DbContext
    {
        public AppContextDB() : base("defaultConnection")
        {
        }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //remover convention de plural nas tabelas do banco
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //remove delete em cascata 1 to N
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //remove delete em cascata N to N
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}