using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest
{
    class Person : ICloneable
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }

        public Person(int age)
        {
            Age = age;
        }

        public Person(string name)
        {
            Name = name;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Address : ICloneable
    {
        public string Num1 { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
