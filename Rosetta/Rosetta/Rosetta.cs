/**
 * Rosetta视为RenderThread
 * Framework视为LogicThread
 * */
using System;
using System.Collections.Generic;
using Alkaid;

namespace Rosetta
{
    public class Rosetta : Singleton<Rosetta>, Lifecycle
    {
        public void Init(Callback setupWithUnity)
        {
            if (setupWithUnity == null)
            {
                setupWithUnity = RosettaSetup.Instance.SetupWithUnity;
            }
            FrameworkSetup.Instance.RegisterSetupFromUnity(setupWithUnity);
            FrameworkSetup.Instance.RegisterSetupFromPorject(RosettaSetup.Instance.SetupWithProject);

            Init();
        }

        public void RenderTick(float interval)
        {
        }

        public bool Init()
        {
            Framework.Instance.Start();

            return true;
        }

        public void Tick(float interval)
        {
            // 事件系统需要tick
            EventSystem.Instance.Tick(interval);
        }

        public void Destroy()
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



