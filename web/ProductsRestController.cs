using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebApp.Model;

namespace WebApp
{
    //[Route("/products")]
    public class ProductsRestController : Controller 
    {       
        private MyDbContext myDbService {get ; set ;}

        [Obsolete]
        private IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public ProductsRestController(MyDbContext myDbContext , IHostingEnvironment hostingEnvironment){
                this.myDbService = myDbContext;
                _hostingEnvironment = hostingEnvironment;
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("/products")]
        public IEnumerable<Product> GetListProducts(){
            return this.myDbService.Products;
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("/products/{id}")]
        public Product GetOneProduct(int id){            
            return this.myDbService.Products.FirstOrDefault(p=>p.Id == id); 
        } 

        [HttpPost("/products")]
        public Product PostProduct([FromBody]Product product){
            this.myDbService.Add(product);
            this.myDbService.SaveChanges();     //afin de valider les changements/transactions
            return product;
        }

        [HttpDelete("/products/{id}")]
        public void DelOneProduct(int id){            
            Product product = this.myDbService.Products.FirstOrDefault(p=>p.Id == id);     
            this.myDbService.Remove(product);
            this.myDbService.SaveChanges();
        }
     
        [EnableCors("CorsPolicy")]
        [HttpPut("/products/{id}")]
        public async Task<Product> UpdateProductAsync(int id, [FromBody]Product product){               
            Product p = await this.myDbService.Products.FirstOrDefaultAsync(p=>p.Id == id);   
            if(p == null)    return null;
            else {                    
                p.name = product.name;
                p.description = product.description;
                p.currentPrice = product.currentPrice;
                p.promotion = product.promotion;
                p.available = product.available;
                p.selected = product.selected;

                this.myDbService.Update(p);
                this.myDbService.SaveChanges();
            }
            return p;
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("/selectedProducts")]
        public IEnumerable<Product> GetSelectedProducts(){
            return this.myDbService.Products.Where(p=>p.selected==true);
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("/promotionProducts")]
        public IEnumerable<Product> GetPromotionProducts(){
            return this.myDbService.Products.Where(p=>p.promotion==true);
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("/available")]
        public IEnumerable<Product> GetAvailableProducts(){
            return this.myDbService.Products.Where(p=>p.available==true);
        }

        [EnableCors("CorsPolicy")]
        [HttpPost("/uploadPhoto/{id}"), DisableRequestSizeLimit]
        [Obsolete]
        public ActionResult UploadPicture(int id)
        {
            try
            {                
                //récupérer le fichier
                var file = Request.Form.Files[0];                

                if (file.Length < 0)    return Json("Upload Failed : pb with length ");

                //chemin vers utilisateur
                string userPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);   // c:\Users\moi
                //Console.WriteLine("the user directory is {0}",userPath);

                //obtention du chemin complet
                string productsFolder = @"ecom\products";     
                string completePath = Path.Combine(userPath, productsFolder);
                //Console.WriteLine("the completePath directory is {0}",completePath);

                //verif si le chemin n'existe pas, le creer
                if (!Directory.Exists(completePath))
                {
                    Directory.CreateDirectory(completePath);
                }

                //copie du fichier vers la destination                
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');  //nom du fichier
                string fullPath = Path.Combine(completePath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);        //ToDO si le fichier existe déjà
                    //changer le nom de l'image correspondant à ce produit en base de donné
                    var product = GetOneProduct(id);
                    product.photoName = fileName;   
                    this.myDbService.Update(product);
                    this.myDbService.SaveChanges();                             
                }                
                return Json("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }

        [EnableCors("CorsPolicy")]
        [HttpGet("/photoProduct/{id}"), DisableRequestSizeLimit]
        public async Task<ActionResult> DownloadPictureAsync(int id)
        {
            //récupérer le nom de l'image en base via l'id du produit
            Product product = GetOneProduct(id);
            if(product == null)    return Json("Product not found !" );
            var photoName = product.photoName;

            //se positionner sur le chemin correspondant
            string userPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);   // c:\Users\moi
            string productsFolder = @"ecom\products";     
            string completePath = Path.Combine(userPath, productsFolder);

            string fullPath = Path.Combine(completePath, photoName);    //chemin complet vers le fichier
            //Console.WriteLine("the fullPath directory is {0}",fullPath);
            
            //préparation puis envoi du fichier
            var memory = new MemoryStream();
            await using(var stream = new FileStream(fullPath,FileMode.Open))
            {
                await stream.CopyToAsync(memory);        
            }
            memory.Position = 0;            
            return File(memory, GetContentType(fullPath));            
        }
        private string GetContentType(string path)  {  
            var types = GetMimeTypes();  
            var ext = Path.GetExtension(path).ToLowerInvariant();  
            return types[ext];  
        }     
        private Dictionary<string, string> GetMimeTypes() {  
            return new Dictionary<string, string>  {  
                {".png", "image/png"},  
                {".jpg", "image/jpeg"},  
                {".jpeg", "image/jpeg"},  
            };  
        } 
 
        [HttpGet("/generate")]
        public string GenerateDb(){           
            using (var db = new MyDbContext()){
                db.Database.EnsureDeleted();    
                db.Database.EnsureCreated();

                /* Student aymene = new Student { Name = "El babili", Score=19};
                Console.WriteLine(db.Students.Add(aymene)); */

                Category pc = new Category { Name = "PC"};
                Category smartphone = new Category { Name = "Smart Phone"};
                Category printer = new Category { Name = "Printer"};
                db.Categories.Add(pc); db.Categories.Add(smartphone); db.Categories.Add(printer);
                                
                Product asus = new Product { name = "Asus", description=" ", currentPrice = 500, selected=true,promotion=true,available=true, photoName="unknown.png",quantity=0,Category = pc};
                Product dell = new Product { name = "Dell", description=" ", currentPrice = 400, selected=true,promotion=true,available=true, photoName="unknown.png",quantity=0,Category = pc};
                Product s8 = new Product { name = "S8", description=" ", currentPrice = 200, selected=true,promotion=true,available=true, photoName="unknown.png",quantity=0,Category = smartphone};
                Product s9 = new Product { name = "S9", description=" ", currentPrice = 350, selected=true,promotion=true,available=true, photoName="unknown.png",quantity=0,Category = smartphone};
                Product iphone = new Product { name = "Iphone", description=" ", currentPrice = 500, selected=true,promotion=true,available=true, photoName="unknown.png",quantity=0,Category = smartphone};
                Product canon = new Product { name = "Canon", description=" ", currentPrice = 50, selected=true,promotion=true,available=true, photoName="unknown.png",quantity=0,Category = printer};       
                db.Products.Add(asus);db.Products.Add(dell);db.Products.Add(s8);db.Products.Add(s9);
                db.Products.Add(iphone);db.Products.Add(canon);
                db.SaveChanges();
                return "db generated";
            }    
        }
    }

}