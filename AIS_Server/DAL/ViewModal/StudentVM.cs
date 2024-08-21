using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Server.DAL.ViewModal
{
    public class StudentVM
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [Range(0, 120)]
        public byte Age { get; set; }
        [Required]
        public bool IsStudent { get; set; }
    }
}
