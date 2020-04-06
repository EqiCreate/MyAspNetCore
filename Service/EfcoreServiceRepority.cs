using MyASP.Data;
using MyASP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyASP.Service
{

    /*
     1 使用appsettingjson 里面的connectionstrings 内容配置+startup注入scoped实例
     2 使用Add-Migration 和update-database 迁移代码和实体对象创建 数据库表
     3 继承dbcontext 实现dbset<T> 对象属性，并且注入构造函数参数给base
     */
    public class EfcoreServiceRepority : ImodelEntity<Student>
    {
        private readonly DataContext _datacontext;

        public EfcoreServiceRepority(DataContext datacontext)
        {
            this._datacontext = datacontext;
        }
        public Student Add(Student newmodel)
        {
            _datacontext.Add(newmodel);
            _datacontext.SaveChanges();
            return newmodel;
        }

        public IEnumerable<Student> getall()
        {
            return this._datacontext.Students.ToList();
        }

        public Student getbyid(int id)
        {
            return this._datacontext.Students.Find(id); 
        }
    }
}
