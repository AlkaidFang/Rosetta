using System;
using System.Collections.Generic;
using Alkaid;

namespace Rosetta
{
    public class Rosetta : InstanceTemplate<Rosetta>, Lifecycle
    {
        public bool Init()
        {
            RosettaSetup.Instance.Init();

            RunLogic();

            return true;
        }

        public void Tick()
        {

        }

        public void Destroy()
        {
            StopLogic();
        }

        public void RunLogic()
        {
            Framework.Instance.Start();
        }

        public void StopLogic()
        {
            Framework.Instance.Stop();
        }























        public int GetRandomNum100()
        {
            Random r = new Random(DateTime.Now.Second);
            return r.Next(100);
        }

        public int GetRandomNum1000()
        {
            MyRandom r = new MyRandom();
            return r.GetRandomNum1000();
        }
    }
}



