#region What's this?
//セーブの実際の処理をするためのスクリプト。
#endregion

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace hatuxes.Saves
{
    [System.Serializable]
    public class HiScoreData
    {
        public float[] _1stHiScoreLists;
        public float[] _2ndHiScoreLists;
        public float[] _3rdHiScoreLists;

        public bool wasOpenExtra;

        public bool initialMessage;

        public HiScoreData(float defaultValue)
        {
            _1stHiScoreLists = new float[4] { defaultValue, defaultValue, defaultValue, defaultValue };
            _2ndHiScoreLists = new float[4] { defaultValue, defaultValue, defaultValue, defaultValue };
            _3rdHiScoreLists = new float[4] { defaultValue, defaultValue, defaultValue, defaultValue };

            wasOpenExtra = false;

            initialMessage = true;
        }
    }

    public class SaveSystem
    {
        public static void Save(HiScoreData data)

        {
            BinaryFormatter formatter = new BinaryFormatter();

            string pathFolder = Application.persistentDataPath + "\\Data";
            string path = pathFolder + "\\ScoreInformation.hax";

            if (!File.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);

            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);

            stream.Close();
        }

        public static HiScoreData Load(float defaultValue)
        {
            string path = Application.persistentDataPath + "\\Data\\ScoreInformation.hax";

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                HiScoreData data = formatter.Deserialize(stream) as HiScoreData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save file not found in" + path);
                var data = new HiScoreData(defaultValue);
                return data;
            }
        }
    }
}