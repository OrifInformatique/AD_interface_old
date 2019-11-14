using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerShellUI1
{
    class Application
    {
        public string abreviation { get; set; }
        public string name { get; set; }

        public Application(string abreviation, string name)
        {
            this.abreviation = abreviation;
            this.name = name;
        }
    }
}
