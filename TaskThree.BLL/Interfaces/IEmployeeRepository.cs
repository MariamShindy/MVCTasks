using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskThree.DA.Models;

namespace TaskThree.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable <Employee> GetEmployeesByAddress (string address);
		public IEnumerable<Employee> SearchByName (string name);

		//IEnumerable<Employee> GetAll(); //As no tracking
		//Employee Get(int id);
		//int Add(Employee entity);
		//int Update(Employee entity);
		//int Delete(Employee entity);
	}
}
