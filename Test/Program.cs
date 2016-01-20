using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkaid;

class Program
{
    static void Main(string[] args)
    {
        JRandom r = new JRandom(-1234);
        for (int i = 0; i < 10000; ++ i)
        {
            Console.Out.Write("" + r.nextGaussian() + "  ");

        }
        Console.Out.WriteLine();
        Console.Out.WriteLine("sss " + ((1L << 48) - 1));
        Console.In.ReadLine();
        //Alkaid.
    }
}
