using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using System.IO;

namespace HenryTool
{

    public class FileSystem : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void DeleteFolder(string _path)
        {
            //bool result = false;
            string[] files = Directory.GetFiles(_path);
            string[] dirs = Directory.GetDirectories(_path);
           
            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteFolder(dir);
            }

            Directory.Delete(_path, false);
            //return result;
        }



    }



}

