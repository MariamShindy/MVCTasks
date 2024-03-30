using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskThree.DA.Models;

namespace TaskThree.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        IEnumerable<T> GetAll(); //As no tracking
        T Get(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
