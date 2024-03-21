using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree.DA.Models
{
    public class Department
    {
        public int Id { get; set; }
        //[Required(ErrorMessage ="code is reuqired here !!")]
        public string Code { get; set; }
        //[Required]
        public string Name { get; set; }
        [DisplayName("Date of creation")]
        public DateTime DateOfCreation { get; set; }
    }
}
