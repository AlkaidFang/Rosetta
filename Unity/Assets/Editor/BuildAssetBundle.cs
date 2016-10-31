using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildAssetBundle {

    public class Ast
    {
        public string pathName;
        public int count;
    }



    [MenuItem("Alkaid/Build AssetBundle(Auto)")]
    public static void BuildAuto()
    {
        string path = Application.dataPath + "/AssetBundles/";
        Object[] selects = Selection.objects;

        Dictionary<string, Ast> assetMap = new Dictionary<string, Ast>();
        Dictionary<string, Ast> temp = new Dictionary<string, Ast>();
        for (int i = 0; i < selects.Length; ++i)
        {
            Ast a = new Ast();
            a.pathName = AssetDatabase.GetAssetPath(selects[i]);
            a.count = 0;
            assetMap.Add(a.pathName, a);
            temp.Add(a.pathName, a);
        }


        // 生成所有的依赖项
        List<Ast> list = new List<Ast>();
        list.AddRange(assetMap.Values);
        for (int i = 0; i < list.Count; ++i)
        {
            Ast a = list[i];
            string [] depends = AssetDatabase.GetDependencies(a.pathName, true);
            for (int j = 0; j < depends.Length; ++j)
            {
                if (depends[j] == a.pathName)
                {
                    a.count++;
                }
                else
                {
                    Ast d;
                    if (temp.ContainsKey(depends[j]))
                    {
                        d = temp[depends[j]];
                    }
                    else
                    {
                        d = new Ast();
                        d.pathName = depends[j];
                        d.count = 0;
                        temp.Add(d.pathName, d);
                    }

                    d.count++;
                    if (!list.Contains(d))
                        list.Add(d);
                }
            }
        }

        // 此时temp中有所有的资源对象
        List<Ast> list2 = new List<Ast>();
        list2.AddRange(assetMap.Values);
        for (int i = 0; i < list2.Count; ++i)
        {
            Ast a = list2[i];
            List<Ast> array = new List<Ast>();
            array.Add(a);
            CalDepends(a, array, list2, temp);

            // 此时array中包含能够打包在一起的对象。那么开始设置bundlename
            string bundleName = list2[i].pathName;
            for (int j = 0; j < array.Count; ++j)
            {
                AssetImporter ai = AssetImporter.GetAtPath(array[j].pathName);
                Debug.LogFormat("set bundlename for:{0}", array[j].pathName);

                if (string.IsNullOrEmpty(ai.assetBundleName))
                {
                    ai.assetBundleName = bundleName;
                }
                else
                {
                    Debug.LogErrorFormat("already packet, old:{0}, new:{1}", ai.assetBundleName, bundleName);
                }
            }
        }


        if (!System.IO.File.Exists(path))
        {
            System.IO.File.Create(path);
        }

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.Android);



        //AssetDatabase.GetDependencies()
    }

    [MenuItem("Alkaid/Build AssetBundle(Named)")]
    public static void BuildNamed()
    {

    }


    private static void CalDepends(Ast a, List<Ast> array, List<Ast> global, Dictionary<string, Ast> all)
    {
        string[] depends = AssetDatabase.GetDependencies(a.pathName, false);
        for (int i = 0; i < depends.Length; ++i)
        {
            if (depends[i] != a.pathName)
            {
                Ast d = all[depends[i]];
                if (d.count == a.count + 1)
                {
                    array.Add(d);
                	CalDepends(d, array, global, all);
                }
                else
                {
                	if (!global.Contains(d))
                    	global.Add(d);
                }
            }
        }
    }

    
}
