using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskThree.BLL.Interfaces;
using TaskThree.DA.Data;
using TaskThree.DA.Models;

namespace TaskThree.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {

        private protected readonly ApplicationDBContext _dbContext;
        public GenericRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;//Ask CLR to create the dbcontext object 
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public T Get(int id)
        {
            return _dbContext.Find<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)_dbContext.Employees.Include(E => E.Department).AsNoTracking().ToList();
            }
            else
            {
                return _dbContext.Set<T>().AsNoTracking().ToList();
            }
        }

        public void Update(T entity)
        {
                _dbContext.Set<T>().Update(entity);
        }
    }
}
