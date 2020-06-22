using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Model;

// l'idée ici est de spécifier à Entity framework de créer les classes à ma place

namespace WebApp
{
    public class MyDbContext : DbContext//: IdentityDbContext<IdentityUser>     //convention obligatoire, héritage de DbContext
    {
        //public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {} 
        
        public DbSet<Student> Students{get;set;}    //la collection Students est une collection de students d'ou get et set
   
        public DbSet<Product> Products{get;set;}

        public DbSet<Category> Categories{get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
            optionsBuilder.UseMySql("Server=localhost;Database=shopnet;User=root;Password=maryam;"); //ToDO utiliser un fichier de config
    }
}