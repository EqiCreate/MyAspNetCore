using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace MyASP.Model
{
    public class Student
    {
        public int id { get; set; }
       
        public string firstname { get; set; }

       
        public string lastname { get; set; }

        public DateTime birthdate { get; set; }

      
        public Gender gender { get; set; }
    }
}
