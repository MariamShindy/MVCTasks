using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskThree.BLL.Interfaces;
using TaskThree.BLL.Repositories;
using TaskThree.DA.Data;

namespace TaskThree.BLL
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly ApplicationDBContext applicationDBContext;

        public IEmployeeRepository EmployeeRepository { get; set; } = null;
        public IDepartmentRepository DepartmentRepository { get; set; } = null;
        public UnitOfWork(ApplicationDBContext applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
            EmployeeRepository = new EmployeeRepository(applicationDBContext);
            DepartmentRepository = new DepartmentRepository(applicationDBContext);
        }

        public int Complete()
        {
            return applicationDBContext.SaveChanges();
        }
        public void Dispose()
        {
            applicationDBContext.Dispose();
        }
    }
}
