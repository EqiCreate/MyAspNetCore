using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyASP.Model;

namespace MyASP.Service
{
    public interface ImodelEntity<T> where T:class
    {
        IEnumerable<T> getall();
        T Add(T newmodel);
        T getbyid(int id);
    }
}
