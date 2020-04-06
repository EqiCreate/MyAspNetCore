using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyASP.Model;
namespace MyASP.Service
{
    public class RepostyStudents : ImodelEntity<Student>
    {
        private List<Student> _students;
        public Student Add(Student newmodel)
        {
            var currentmaxid = _students.Max(x => x.id);
            newmodel.id = currentmaxid + 1;
            _students?.Add(newmodel);
            return newmodel;
        }
        public RepostyStudents()
        {
            _students = new List<Student>()
            {
                 new Student()
                {
                    id = 1,
                    firstname = "dd0",
                    lastname = "ddd0",
                },
                new Student()
                {
                    id = 1,
                    firstname = "dd",
                    lastname = "ddd1",
                },
                 new Student()
                {
                    id = 2,
                    firstname = "dd2",
                    lastname = "ddd2",
                },
            };
        }
        public IEnumerable<Student> getall()
        {
            return _students;
        }

        public Student getbyid(int id)
        {
            return _students.FirstOrDefault(x=>x.id==id);
        }
    }
}
