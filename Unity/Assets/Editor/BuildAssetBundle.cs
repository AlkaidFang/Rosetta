using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildAssetBundle {

    public class Ast
    {
        public string pathName;
        public int count;
    }

    private static string savePath = "Assets/AssetBundles/";

    [MenuItem("Alkaid/Build AssetBundle(Auto)")]
    public static void BuildAutoName()
    {
        Object[] selects = Selection.objects;

        Dictionary<string, Ast> selectAssetMap = new Dictionary<string, Ast> ();
        Dictionary<string, Ast> allAssetMap = new Dictionary<string, Ast> ();
        for (int i = 0; i < selects.Length; ++i) {
            Ast a = new Ast ();
            a.pathName = AssetDatabase.GetAssetPath (selects [i]);
            a.count = 0;
            selectAssetMap.Add (a.pathName, a);
            allAssetMap.Add (a.pathName, a);
        }

        // 生成所有的依赖项
        List<Ast> list = new List<Ast>();
        list.AddRange (selectAssetMap.Values);
        for (int i = 0; i < list.Count; ++i) {
            Ast a = list [i];
            // 获取所有依赖,如果是自身，则只引用增加；如果是其他，则新建或者获取，引用增加。
            string[] depends = AssetDatabase.GetDependencies (a.pathName, true);
            for (int j = 0; j < depends.Length; ++j) {
                if (depends [j] == a.pathName) {
                    a.count++;
                } else {
                    Ast d;
                    if (allAssetMap.ContainsKey (depends [j])) {
                        d = allAssetMap [depends [j]];
                    } else {
                        d = new Ast ();
                        d.pathName = depends [j];
                        d.count = 0;
                        allAssetMap.Add (d.pathName, d);
                    }

                    d.count++;
                    // 如果遍历的队列中没有这个依赖，则加在后面
                    if (!list.Contains (d)) {
                        list.Add (d);
                    }
                }
            }
        
        }

        // 此时allAssetMap中包含所有这些资源和他们的依赖
        List<Ast> list2 = new List<Ast>();
        list2.AddRange (selectAssetMap.Values);
        for (int i = 0; i < list2.Count; ++i) {
            Ast now = list2 [i];
            // 获取专属依赖，打包在一起
            List<Ast> array = new List<Ast> ();
            array.Add (now);
            CalDepends (now, array, list2, allAssetMap);

            // 次数array中包含所有专属资源，应该打包为同一个ab
            string bundleName = now.pathName + ".ab";
            for (int j = 0; j < array.Count; ++j) {
                AssetImporter ai = AssetImporter.GetAtPath (array [j].pathName);
                Debug.LogFormat ("set bundlename : {0} to asset : {1}", bundleName, array [j].pathName);
                ai.assetBundleName = bundleName;
            }
        }

        if (!System.IO.Directory.Exists(savePath)) {
            System.IO.Directory.CreateDirectory (savePath);
        }

        BuildPipeline.BuildAssetBundles (savePath, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    // 递归获取所有a资源的专属依赖，加入array，不是专属则加入global
    public static void CalDepends(Ast a, List<Ast> array, List<Ast> global, Dictionary<string, Ast> all)
    {
        string[] depends = AssetDatabase.GetDependencies (a.pathName, false);
        for (int i = 0; i < depends.Length; ++i) {
            if (depends [i] != a.pathName) {
                Ast d = all [depends [i]];
                if (d.count == a.count + 1) {
                    array.Add (d);
                    CalDepends (d, array, global, all);
                } else {
                    if (!global.Contains (d)) {
                        global.Add (d);
                    }
                }
            }
        }
    }

}
