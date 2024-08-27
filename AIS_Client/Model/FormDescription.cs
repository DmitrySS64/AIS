using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Client.Model
{
    public class FormDescription
    {
        public string ClassName { get; set; }
        public List<FormField> Fields { get; set; }
    }
}
