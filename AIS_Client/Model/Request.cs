using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Client.Model
{
    public class Request
    {
        public string ClassName { get; set; }
        public string Command { get; set; }
        public int Id { get; set; }
        public string Payload { get; set; }
    }
}
