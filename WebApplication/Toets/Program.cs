using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toets
{
    class Program
    {
        static void Main(string[] args)
        {
            string a = "20";
            int x = 3;
            int y = 6;
            int z = x * y + Int32.Parse(a);
            Console.WriteLine(a);
            Console.WriteLine(z.ToString());
            Console.ReadLine();
        }
    }
}
