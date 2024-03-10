using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Utility.SaveLoad
{
    public static class SaveLoadAPI
    {
        public static readonly string FOLDER_PATH = Application.dataPath + "/Save Files/";
        public static readonly string SAVE_FILE_NAME = "Save.txt";

        private static void Initialize()
        {
            if (Directory.Exists(FOLDER_PATH)) return;

            Directory.CreateDirectory(FOLDER_PATH);
        }

        public static void Save(string dataString)
        {
            Initialize();

            string path = FOLDER_PATH + SAVE_FILE_NAME;
            File.WriteAllText(path, dataString);
        }

        public static string Load()
        {
            Initialize();

            string path = FOLDER_PATH + SAVE_FILE_NAME;
            if (!File.Exists(path)) return null;

            string loadedString = File.ReadAllText(path);
            return loadedString;
        }
    }
}