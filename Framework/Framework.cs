using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public class MyRandom
    {
        public int GetRandomNum1000()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            return r.Next(1000);
        }

        public UnityEngine.GameObject CreateOne()
        {
            UnityEngine.GameObject go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
            go.transform.localPosition = UnityEngine.Vector3.zero;
            return go;
        }
    }
}
