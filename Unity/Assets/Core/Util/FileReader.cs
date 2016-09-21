using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Alkaid
{
    public class FileReader
    {
        private static int _line_ptr = 0;
        private static List<string> _line_array = new List<string>();
        private static int _element_ptr = 0;
        private static string[] _element_array = null;
        private static List<string> lines_temp = new List<string>();

        private static void _reset()
        {
            _line_ptr = 0;
            _line_array.Clear();
            lines_temp.Clear();
            _element_ptr = 0;
            _element_array = null;
        }

        private static string _next_element()
        {
            return _element_array[_element_ptr++];
        }

        public static bool LoadPath(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string text = string.Empty;
                try
                {
                    if (filePath.Contains("://"))
                    {
                        WWW www = new WWW(filePath);
                        while (!www.isDone) ;
                        text = www.text;
                    }
                    else
                    {
                        if (File.Exists(filePath))
                        {
                            StreamReader file = File.OpenText(filePath);
                            text = file.ReadToEnd();
                            file.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogErrorFormat("Error Loading File [{0}]", filePath);
                    return false;
                }

                return LoadText(text);
            }

            return false;
        }

        public static bool LoadText(string text)
        {
            _reset();

            if (!string.IsNullOrEmpty(text))
            {
                string[] lines = text.Split('\n');
                string temp;
                for (int i = 0; i < lines.Length; ++i)
                {
                    temp = lines[i];
                    if (temp.EndsWith("\r"))
                    {
                        temp = temp.Substring(0, temp.Length - 1);
                    }
                    lines_temp.Add(temp);
                }

                DeleteComments();

                return true;
            }

            return false;
        }

        public static void UnLoad()
        {
            _reset();
        }

        /// <summary>
        /// 删除#开始的注释行
        /// </summary>
        private static void DeleteComments()
        {
            foreach (var line in lines_temp)
            {
                if (!line.StartsWith("#") && !string.IsNullOrEmpty(line))
                {
        	        _line_array.Add(line);
                }
            }
        
            lines_temp.Clear();
        }

        public static bool IsEnd()
        {
            if (_line_ptr >= _line_array.Count)
                return true;

            string str = _line_array[_line_ptr];
            if (str == null || string.IsNullOrEmpty(str) || str.StartsWith("\t"))
                return true;

            return false;
        }

        public static void ReadLine()
        {
            _element_ptr = 0;
            _element_array = _line_array[_line_ptr].Split('\t');
            _line_ptr++;
        }

        public static int ReadInt()
        {
            string n = _next_element();

            return int.Parse(n);
        }

        public static string ReadString()
        {
            string n = _next_element();
            return n;
        }

        public static float ReadFloat()
        {
            string n = _next_element();
            return float.Parse(n);
        }

        public static bool ReadBoolean()
        {
            string n = _next_element().ToLowerInvariant();
            if (n == "1" || n == "true")
            {
                return true;
            }

            return false;
        }

    }
}
