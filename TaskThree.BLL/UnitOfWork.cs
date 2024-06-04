using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskThree.BLL.Interfaces;
using TaskThree.BLL.Repositories;
using TaskThree.DA.Data;
using TaskThree.DA.Models;

namespace TaskThree.BLL
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly ApplicationDBContext applicationDBContext;
        private Hashtable _repositories;

      
        public UnitOfWork(ApplicationDBContext applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
            _repositories = new Hashtable();
        }

        public int Complete()
        {
            return applicationDBContext.SaveChanges();
        }
        public void Dispose()
        {
            applicationDBContext.Dispose();
        }

        public IGenericRepository<T> Repository<T>() where T : ModelBase
        {
            var key = typeof(T).Name;
            if (!_repositories.ContainsKey(key))
            {
                if (key == nameof(Employee))
                {
                    var repository = new EmployeeRepository(applicationDBContext);
                    _repositories.Add(key, repository);
                }
                else
                {
                   var repository = new GenericRepository<T>(applicationDBContext);
                    _repositories.Add(key, repository);
                }
            }
            return _repositories[key] as IGenericRepository<T>;
        }
    }
}
