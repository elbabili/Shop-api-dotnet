
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;   
using System.Collections.Generic;
using System.Linq;
using WebApp.Model;

namespace WebApp.Controllers {
    [Route("Category")]
    public class CategoryController : Controller{    

            //Pour injecter une dépendance, vous pouvez soit le faire via le constructeur, soit via l'annotation
            public MyDbContext db {get;set;}   
            // privilégier l'injection des dépendances plutot que l'instanciation donc ajouter le service dans Startup/ConfigureService
            //ToDO FromService ne fonctionne pas !
            public CategoryController(MyDbContext db){
                this.db = db;
            }

            [Route("all")]
            public IActionResult ListCatsWithProds(){ // liste des catégories avec les produits associés
                IEnumerable<Category> categories = db.Categories.Include(c=>c.products);   //nous spécifions ici notre souhait de récupérer la liste des produits au passage
                                                                    //en effet, lors de la déclaration, nous avons souhaité utilisé le mode lazy loading via virtual
                                                                    //afin de ne pas injecter systhématiquement la liste des produits d'une catégorie 
                return View("ListAll" , categories);
            }      

            [Route("productsCat")]   
            public IActionResult ListProdsByCat(int catId=-1){  // liste des produits par catégorie
                IEnumerable<Category> categories = db.Categories;   //récupération de la liste des catégories
                List<SelectListItem> items = new List<SelectListItem>();
                foreach(var c in categories){       //préparation des données à afficher côté vue                    
                    items.Add(new SelectListItem{
                         Text = c.Name,
                         Value = c.Id.ToString()   
                    });    
                }
                Category category;
                if(catId == -1) category = new Category();
                else category = this.db.Categories.Where(c=>c.Id == catId)
                                                .Include(c=>c.products).FirstOrDefault();//Renvoi tous les produits d'une catégorie
                ViewBag.categories = items; // liste des catégories
                return View("ListProdsByCat" , category);   // liste produits par cat
            }
    }
}