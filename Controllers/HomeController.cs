
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;   //contient les classes de base
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Model;

namespace WebApp.Controllers{ 
    public class HomeController : Controller{       //tout controleur hérite de celui-ci d'ou la necessité d'inclure la dépendance tel un import
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;        
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager){
                _userManager = userManager;
                _roleManager = roleManager;
                _signInManager  = signInManager;
            }
            public IActionResult Index(){
                return View();
            }

            [Authorize]
            public IActionResult Secret(){
                return View();
            }

            [Authorize(Roles = "Admin")]
            public IActionResult admin(){
                return View();
            }

            [Authorize(Roles = "User,Admin")]
            public IActionResult user(){
                return View();
            }

            public IActionResult Authenticate(){
                var grandmaClaims = new List<Claim>()   //l'authorité ici est la grand mère et elle est respecté et écouté
                {                                       //voilà ce qu'elle nous rapporte à votre sujet
                     new Claim(ClaimTypes.Name,"mo"),
                     new Claim(ClaimTypes.Email,"mo@gmail.com"),
                     new Claim("Grandma.says","he is ok")
                };

                var licenseClaims = new List<Claim>()   //l'autorité ici est la préfecture qui attribue le permis de conduire
                {
                     new Claim(ClaimTypes.Name,"mohamed el babili"),
                     new Claim("DrivingLicense ","B")   
                };

                //Ensuite il faut créer une identité basée sur les revendications ou claims
                var grandmaIdentity = new ClaimsIdentity(grandmaClaims,"Grandma Identity");
                var licenseIdentity = new ClaimsIdentity(licenseClaims,"Prefecture");

                //Il s'agit ensuite de créer un utilisateur à partir d'une ou plusieurs autorités : Grandma, Prefecture, Sécu
                var userPrincipal = new ClaimsPrincipal(new [] { grandmaIdentity,licenseIdentity } );

                HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("Index");
            }



            public IActionResult Login()        
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Login(string username, string password)    //Vérifier si un utilisateur existe
            {
                var user = await _userManager.FindByNameAsync(username);    

                if(user != null)    //donnée saisie
                {
                    var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
                    if(result.Succeeded)    //login ok
                    {
                        return RedirectToAction("Index");    
                    }
                }        
                return RedirectToAction("Login");       
            }

            public IActionResult Register()     
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Register(string username, string password)   //Crée un utilisateur
            {
                try
                {
                    ViewBag.Message = "user created already registered";
                    var user = new IdentityUser
                    {
                        UserName = username,
                        Email = ""     // pas obligatiore mais on peut envisager un ensemble de donnée dans un formulaire...   
                        // de même ici, nous pourrions spécifier un algo pour notre password sinon EF le fait à notre place  
                    };

                    var result = await _userManager.CreateAsync(user,password);

                    if(result.Succeeded)    //si la création de l'utilisateur est ok, on se connecte
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);
                        if(signInResult.Succeeded)   
                        {
                            ViewBag.Message = "user created and connected";
                            return RedirectToAction("Index");    
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;                    
                }
                return RedirectToAction("Register");
            }

            public async Task<IActionResult> LogOut(){
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login"); 
            }

             public async Task<IActionResult> addUsersRoles(){
                //ajout d'utilisateurs
                var macron = new IdentityUser{ UserName = "macron" };
                var brigitte = new IdentityUser{ UserName = "brigitte" };
                var resMacron = await _userManager.CreateAsync(macron,"1234");
                var resBrigitte = await _userManager.CreateAsync(brigitte,"1234");
                
                //création puis ajout de rôles à nos utilisateurs
                IdentityResult admin = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                IdentityResult user = await _roleManager.CreateAsync(new IdentityRole("User"));  

                var res = await _userManager.AddToRoleAsync(macron, "Admin");
                Console.WriteLine("--------------------------------->>>>>>>>>>>" + res);
                //await _userManager.AddToRoleAsync(macron, "USER");
                res = await _userManager.AddToRoleAsync(brigitte, "User");
                Console.WriteLine("--------------------------------->>>>>>>>>>>" + res);
                return RedirectToAction("Login"); 
            }             
    }
}