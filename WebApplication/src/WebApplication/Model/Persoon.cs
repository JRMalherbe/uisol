using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public class Persoon
    {
        private static int _id = 0;

        public Persoon()
        {
            Id = ++_id;
        }
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Van { get; set; }
    }
}
