using AutoMapper;
using TaskThree.DA.Models;
using TaskThree.PL.ViewModels;

namespace TaskThree.PL.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles() 
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            /*.ForMember(d => d.Name , o => o.MapFrom(s => s.EmpName))*/
            CreateMap<DepartmentViewModel, Department>().ReverseMap();

        }

    }
}
