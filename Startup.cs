using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore.InMemory;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace WebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); // 1 ajout MVC pour la gestion des controleurs et de leurs vues

           /*  services.AddDbContext<MyDbContext>(optionsAction=>{
                optionsAction.UseInMemoryDatabase("DbStudents");    //nom de la base de donnée virtuelle
            }); */ // 12 bis -> SGBD
            
            // afin de pouvoir assurer le mécanisme d'injection des dépendances, de faire de MyDbContext un service, il faut ajouter ces 2 services
            services.AddEntityFrameworkMySql(); 
            
            services.AddDbContext<MyDbContext>();           
 
            /* services.AddIdentity<IdentityUser, IdentityRole>(config => {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultTokenProviders();  */

            services.AddCors(options=>
            {
                options.AddPolicy("CorsPolicy", 
                    builder => builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
            }); 

            /* services.AddAuthentication("CookieAuth")
                        .AddCookie("CookieAuth" , config=>
                        {
                             config.Cookie.Name = "Grandma.cookie";
                             config.LoginPath = "/Home/Authenticate";
                        }); */

            /* services.ConfigureApplicationCookie(config => 
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            }); */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 2 configurer l'utilisation des fichiers statiques css js html
            // Pour ce faire, il faut ajouter un dossier wwwroot à la racine du projet
            // puis ajouter les fichiers en question permettant au nav l'accès ici sans passer par un controleur : http://localhost:5000/index.html
            app.UseStaticFiles();

            app.UseRouting();

            //Question : Qui êtes vous ?
            //app.UseAuthentication();

            //Question : êtes vous autorisé ?
            //app.UseAuthorization();     // entre useRouting et useEndPoints obligatoirement

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                /* endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                }); */
                
                // 5 -> spécifier le système de routage par défaut permettant de mapper les url avec les actions
                app.UseEndpoints(endpoints =>
                {
                        endpoints.MapControllerRoute(                            
                            name    : "default",   
                            pattern : "{controller}/{action}/{id?}"                                                     
                        );
                });
            });
        }
    }
}
