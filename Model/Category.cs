using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApp.Model
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }     // par convention, il faut que le nom de l'attribut corresponde à l'att de product(clé étrangère) sans quoi il faudra ajouter des mécanismes d'annotations suplémentaire...

        [StringLength(25)]    
        [Required,MinLength(4)]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product> products { get; set; }  //virtual pour lazy loading
    }
}
