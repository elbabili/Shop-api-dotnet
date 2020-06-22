using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp{
    [Route("/api/students")]
    public class StudentsRestService : Controller {
        private MyDbContext myDbService;

        public StudentsRestService(MyDbContext myDbContext){
                this.myDbService = myDbContext;
        }

        [HttpGet]
        public IEnumerable<Student> getListStudents(){
            return this.myDbService.Students;
        }

        [HttpGet("{id}")]
        public Student getOneStudent(long id){            
            return this.myDbService.Students.FirstOrDefault(s=>s.Id == id);   
        }

        [HttpPost]
        public Student postStudent([FromBody]Student student){
            this.myDbService.Add(student);
            this.myDbService.SaveChanges();     //afin de valider les changements/transactions
            return student;
        }

        [HttpDelete("{id}")]
        public void delOneStudent(long id){            
            Student student = this.myDbService.Students.FirstOrDefault(s=>s.Id == id);     
            this.myDbService.Remove(student);
            this.myDbService.SaveChanges();
        }

        [HttpPut("{id}")]
        public Student updateStudent(long id, [FromBody]Student student){ 
            student.Id = id;           
            this.myDbService.Update(student);
            this.myDbService.SaveChanges();
            return student; 
        }
    }
}