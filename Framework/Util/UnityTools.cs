using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Alkaid
{
    public class UnityTools
    {
        public static GameObject AddChild(GameObject parent, GameObject prefab)
        {
            GameObject go = GameObject.Instantiate(prefab) as GameObject;

            if (null != go && null != parent)
            {
                Transform t = go.transform;
                t.parent = parent.transform;
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }

            return go;
        }

        

    }

}
