using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkaid;
using System.Threading;

class Program
{
    static NetStream ns = new NetStream(1024);

    static void push()
    {
        byte[] b = new byte[100];
        byte index = 0;
        while(true)
        {
            for(int i = 0; i < b.Length; ++i)
            {
                b[i] = (byte)(index + i);
            }
            index = (byte)(index + b.Length);

            Array.Copy(b, ns.AsyncPipeIn, b.Length);
            ns.FinishedIn(b.Length);
            //Thread.Sleep(1);
        }
    }

    static void get()
    {
        int length = 0;
        while (true)
        {
            length = 0;
            if (ns.InStreamLength > 0)
            {
                for (int i = 0; i < ns.InStreamLength; ++i)
                {
                    Console.Out.Write(ns.InStream[i]);
                    Console.Out.Write(" ");
                    if (ns.InStream[i] % 10 == 0)
                    {
                        ns.PopInStream(i + 1);
                        break;
                    }

                }
            }

            //Thread.Sleep(1000);
        }
    }

    static void Main(string[] args)
    {
        Thread t = new Thread(new ThreadStart(push));
        t.Start();
        Thread t1 = new Thread(new ThreadStart(get));
        t1.Start();




        /*JRandom r = new JRandom(-1234);
        for (int i = 0; i < 10000; ++ i)
        {
            Console.Out.Write("" + r.nextGaussian() + "  ");

        }
        Console.Out.WriteLine();
        Console.Out.WriteLine("sss " + ((1L << 48) - 1));
        Console.In.ReadLine();*/
        //Alkaid.
    }
}
