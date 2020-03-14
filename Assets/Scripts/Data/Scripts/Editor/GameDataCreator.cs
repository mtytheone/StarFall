#region What's this?
//GameDataのSciptableObjectをアセットとして生成するためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StarFall
{
    //ScriptableObject（GameData）の生成プログラム
    public class GameDataCreator
    {
        [MenuItem("Window/CreateScriptableObject/GameData")]
        private static void Create()
        {
            //GameDataのScriptableObjectを作成
            GameData gameData = ScriptableObject.CreateInstance<GameData>();

            //DifficultyStatusのクラスを作成
            DifficultyStatus status1 = new DifficultyStatus(), status2 = new DifficultyStatus(), status3 = new DifficultyStatus(), status4 = new DifficultyStatus();

            //難易度ごとに初期値を設定
            status1.name = "Easy";
            status1.scoreMultiplier = 0.8f;
            status1.rockfallInterval = 2.0f;
            status1.bottomInterval = 0.1f;
            status1.displayColor = Color.green;

            status2.name = "Normal";
            status2.scoreMultiplier = 1.0f;
            status2.rockfallInterval = 1.0f;
            status2.bottomInterval = 0.02f;
            status2.displayColor = Color.white;

            status3.name = "Hard";
            status3.scoreMultiplier = 1.2f;
            status3.rockfallInterval = 0.6f;
            status3.bottomInterval = 0f;
            status3.displayColor = Color.red;

            status4.name = "Extra";
            status4.scoreMultiplier = 2.0f;
            status4.rockfallInterval = 0.1f;
            status4.bottomInterval = 0f;
            status4.displayColor = Color.yellow;


            //各ステータスをScriptableObjectのリストに追加
            gameData.difficultyStatus.Add(status1);
            gameData.difficultyStatus.Add(status2);
            gameData.difficultyStatus.Add(status3);
            gameData.difficultyStatus.Add(status4);

            //Assetとして出力
            AssetDatabase.CreateAsset(gameData, "Assets/GameData.asset");
        }
    }
}
