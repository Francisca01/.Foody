using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foody.Models;

namespace Foody.Utils
{
    //ficheiro que efetua a ligação entre a base de dados e a API
    public class DbHelper : DbContext
    {
        public DbHelper()
        {

        }

        //Definição das Tabelas (BD) / Models (API)
        public DbSet<User> user { get; set; }
        public DbSet<Order> order { get; set; }
        public DbSet<OrderProduct> orderProduct { get; set; }
        public DbSet<Delivery> delivery { get; set; }
        public DbSet<Product> product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //ligação à Base de Dados
            optionsBuilder.UseSqlite("Data Source= db/foody.db");
        }

        //configuração das Keys (BD) para chaves primárias das entidades (API)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().HasKey(u => new { u.idUser });
            //modelBuilder.Entity<Order>().HasKey(e => new { e.idClient, e.idOrderProduct });
            modelBuilder.Entity<OrderProduct>().HasKey(ep => new { ep.idOrder, ep.idProduct });
            //modelBuilder.Entity<Delivery>().HasKey(ent => new { ent.idCondutor, ent.idEncomenda });
            //modelBuilder.Entity<Product>().HasKey(p => new { p.idProduto });
        }
    }
}
