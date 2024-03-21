using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskThree.DA.Models;

namespace TaskThree.BLL.Interfaces
{
    internal interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll(); //As no tracking
        Department Get(int id); 
        int Add(Department entity);
        int Update(Department entity);
        int Delete(Department entity);
    }
}
