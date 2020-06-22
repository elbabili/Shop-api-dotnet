using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Model;

namespace WebApp{
    public class CategoriesRestController : Controller 
    {
        private MyDbContext myDbService {get ; set ;}

        public CategoriesRestController(MyDbContext myDbContext){
                this.myDbService = myDbContext;
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("/categories")]
        public IEnumerable<Category> getListCategories(){
            return this.myDbService.Categories;
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("/categories/{id}")]
        public Category getOneCategory(int id){                       
            return this.myDbService.Categories.FirstOrDefault(c=>c.Id == id); 
        } 

        [HttpPost]
        public Category postCategory([FromBody]Category category){
            this.myDbService.Add(category);
            this.myDbService.SaveChanges();     
            return category;
        }

        [HttpDelete("{id}")]
        public void delOneCategory(int id){            
            Category category = this.myDbService.Categories.FirstOrDefault(c=>c.Id == id);     
            this.myDbService.Remove(category);
            this.myDbService.SaveChanges();
        }

        [HttpPut("{id}")]
        public Category updateCategory(int id, [FromBody]Category category){ 
            category.Id = id;           
            this.myDbService.Update(category);
            this.myDbService.SaveChanges();
            return category; 
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("/categories/{id}/products")]     //nous souhaitons tous les produits d'une catégorie
        public IEnumerable<Product> getProductsByCat(int id){    
            //je veux récupérer pour une catégorie donnée, tous les produits associés    
            ICollection<Product> products = this.myDbService.Categories.Where(c=>c.Id == id)
                                                .Include(c=>c.products).FirstOrDefault().products;  
            return products;
        }
    }
}