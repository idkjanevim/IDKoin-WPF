using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMIDKoin.Models
{
    public class PersonModel
    {
        public PersonModel(string name,int koins,byte[] im)
        {
            Name = name;
            Koins = koins;
            Img = im;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _koins;

        public int Koins
        {
            get { return _koins; }
            set 
            {
                _koins = value;
            }
        }

        private byte[] _img;

        public byte[] Img
        {
            get { return _img; }
            set { _img = value; }
        }



    }
}
