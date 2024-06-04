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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {

        //private readonly ApplicationDBContext _dbContext;
        //public EmployeeRepository(ApplicationDBContext dbContext)
        //{
        //    _dbContext = dbContext;//Ask CLR to create the dbcontext object 
        //}
        //public int Add(Employee entity)
        //{
        //    _dbContext.Employees.Add(entity);
        //    return _dbContext.SaveChanges();
        //}

        //public int Delete(Employee entity)
        //{
        //    _dbContext.Employees.Remove(entity);
        //    return _dbContext.SaveChanges();
        //}

        //public Employee Get(int id)
        //{
        //    return _dbContext.Find<Employee>(id);
        //}

        //public IEnumerable<Employee> GetAll()
        //{
        //    return _dbContext.Employees.AsNoTracking().ToList();
        //}

        //public int Update(Employee entity)
        //{
        //    _dbContext.Employees.Update(entity);
        //    return _dbContext.SaveChanges();
        //}

        public EmployeeRepository(ApplicationDBContext dBContext):base(dBContext)
        {
        }
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dbContext.Employees.Where(E => E.Address.ToLower() == address.ToLower());
        }

		public IEnumerable<Employee> SearchByName(string name)
		{
            return _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name));
		}
	}
}
