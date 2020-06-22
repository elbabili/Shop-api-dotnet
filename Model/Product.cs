using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Model
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(25)]    //lorsqu'on souhaite spéficier la taille d'une chaine en base
        [Required]            //requis ou not null
        [MinLength(5)]        //taille minimum  
        public string name { get; set; }

        [StringLength(50)]
        public string description { get ; set; }
        public double currentPrice { get; set; }

        public Boolean promotion { get; set; }
        public Boolean selected { get; set; }        
        public Boolean available { get; set; }

        [StringLength(25)]
        public string photoName { get; set; }

        public double quantity { get; set; }

        //ManyToOne
        public int CategoryId { get; set; }                 // PROPRIETE DE CLE : clé étrangère / cela empeche entityframework de l'ajouter !
        public virtual Category Category { get; set; }      // PROPRIETE DE NAVIGATION car à partir d'un produit je peux avoir des infos sur la catégorie...
        //les attributs d'association doivent être virtuel car les frameworks utilisent le mode Lazy Loading afin de ne récupérer que les données pertinentes ici
        //la catégorie d'un produit n'est necessaire que si on en fait la demande ici donc à entityframework
    }
}
