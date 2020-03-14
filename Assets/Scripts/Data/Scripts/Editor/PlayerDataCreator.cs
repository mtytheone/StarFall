#region What's this?
//PlayerDataのSciptableObjectをアセットとして生成するためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StarFall
{
    //ScriptableObject（PlayerData）の生成プログラム
    public class PlayerDataCreator
    {
        [MenuItem("Window/CreateScriptableObject/PlayerData")]
        private static void Create()
        {
            //ScriptableObjectを生成
            PlayerData _playerData = ScriptableObject.CreateInstance<PlayerData>();

            //Assetとして出力
            AssetDatabase.CreateAsset(_playerData, "Assets/PlayerData.asset");
        }
    }
}
