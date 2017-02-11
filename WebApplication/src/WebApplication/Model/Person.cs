using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIS
{
    public class Person
    {
        private static int _id = 0;

        public Person()
        {
            Id = ++_id;
        }
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Van { get; set; }
    }
}
