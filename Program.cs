using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApp.Controllers;
using WebApp.Model;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //GenerateDb();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /* public static void GenerateDb(){
                using (var db = new MyDbContext()){
                db.Database.EnsureDeleted();    
                db.Database.EnsureCreated();
            }
            Console.WriteLine("generation de la bdd");
        } */
    }
}
