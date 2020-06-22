using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Model{
    [Table("Students")]
    public class Student{
        [Key]
        public long Id { get; set; }    //Key n'est pas obligatoire à condition que la clé primaire contienne Id ex: StudentId...
        [Required,StringLength(25)]
        public string Name { get; set; }
        public int Score { get; set; }
    }
}