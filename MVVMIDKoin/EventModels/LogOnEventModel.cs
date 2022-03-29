using MVVMIDKoin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMIDKoin.EventModels
{
    public class LogOnEventModel
    {
        public PersonModel person { get; set; }
        public LogOnEventModel(PersonModel personModel)
        {
            this.person = personModel;
        }
    }
}
