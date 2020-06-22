
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;   //contient les classes de base
using System.Collections.Generic;
using WebApp.Model;

namespace WebApp.Controllers{
    public class TestController : Controller{       //tout controleur hérite de celui-ci d'ou la necessité d'inclure la dépendance tel un import
            public IActionResult Index(){
                    IList<string> items = new List<string>();       //soit une liste de chaine de caractère
                    items.Add("aymene");   items.Add("maryam");    items.Add("saadia");
                    ViewData["email"] = "melbabili@gmail.com";      //correspond à un mini Model ici, c'est le premier moyen de transmettre des données à la vue
                    ViewBag.tel = "0616223344";          //autre moyen de récupérer les données dans la vue
                    return View(items);                  //cela suppose qu'il existe une vue correspondant à Index cad index.cshtml
            }                 //encore un moyen de transmettre des données à notre vue
            // NB : ViewData & ViewBag sont hérités de Controller !

            public IActionResult List(){
                    IList<Student> students = new List<Student>();
                    students.Add(new Student{Id = 1,Name = "aymene",Score = 18});
                    students.Add(new Student{Id = 2,Name = "maryam",Score = 17});
                    students.Add(new Student{Id = 3,Name = "saadia",Score = 19});
                    return View(students);                  
            }

            public IActionResult Ex(){
                    return View("Exo");             //plusieurs vues peuvent renvoyer vers le même fichier ...            
            }
    }
}