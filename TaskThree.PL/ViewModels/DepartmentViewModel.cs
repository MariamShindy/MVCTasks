using System.Collections.Generic;
using System.ComponentModel;
using System;
using TaskThree.DA.Models;
using System.ComponentModel.DataAnnotations;

namespace TaskThree.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="code is reuqired here !!")]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Date of creation")]
        public DateTime DateOfCreation { get; set; }
        //Navigational property
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
