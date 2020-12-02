using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foody.Models;

namespace Foody.Utils
{
    public class DbHelper : DbContext
    {
        public DbHelper()
        {

        }

        public DbSet<Cavalo> cavalos { get; set; }
        public DbSet<Classific> classifics { get; set; }
        public DbSet<Coudelaria> coudelarias { get; set; }
        public DbSet<Criador> criadores { get; set; }
        public DbSet<Prova> provas{ get; set; }
        public DbSet<Utilizador> utilizadores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source= coudelaria_dwm.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Classific>().HasKey(c => new { c.cod_prova, c.cod_cavalo});
        }
    }
}
