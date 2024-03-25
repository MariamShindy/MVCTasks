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
    public class DepartmentRepository :GenericRepository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(ApplicationDBContext dBContext):base(dBContext)
        {
        }
        //private readonly ApplicationDBContext _dbContext;
        //public DepartmentRepository(ApplicationDBContext dbContext)
        //{
        //    _dbContext = dbContext;//Ask CLR to create the dbcontext object 
        //    /*new ApplicationDBContext(new DbContextOptions<ApplicationDBContext>())*/
        //}
        //public int Add(Department entity)
        //{
        //    _dbContext.Departments.Add(entity);
        //    return _dbContext.SaveChanges();
        //}

        //public int Delete(Department entity)
        //{
        //    _dbContext.Departments.Remove(entity);
        //    return _dbContext.SaveChanges();
        //}

        //public Department Get(int id)
        //{
        //    //return _dbContext.Departments.Find(id);
        //    return _dbContext.Find<Department>(id);
        //}

        //public IEnumerable<Department> GetAll()
        //{
        //    return _dbContext.Departments.AsNoTracking().ToList();
        //}

        //public int Update(Department entity)
        //{
        //    _dbContext.Departments.Update(entity);
        //    return _dbContext.SaveChanges();
        //}
    }
}
