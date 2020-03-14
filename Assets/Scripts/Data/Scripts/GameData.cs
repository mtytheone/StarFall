#region What's this?
//ゲーム全体のデータの初期値を格納するScriptableObjectを定義するためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace StarFall
{
    public class GameData : ScriptableObject
    {
        public List<DifficultyStatus> difficultyStatus = new List<DifficultyStatus>();  //難易度ごとに設定するので、リスト化する
    }



    [System.Serializable]
    public class DifficultyStatus
    {
        //名前
        [SerializeField] private string _name = "";
        public string name
        {
            get { return _name; }
            #if UNITY_EDITOR
                set { _name = value; }
            #endif
        }

        //スコア倍率
        [SerializeField] private float _scoreMultiplier = 1.0f;
        public float scoreMultiplier
        {
            get { return _scoreMultiplier; }
            #if UNITY_EDITOR
                set { _scoreMultiplier = value; }
            #endif
        }

        //岩の落下インターバル
        [SerializeField] private float _rockfallInterval = 1.0f;
        public float rockfallInterval
        {
            get { return _rockfallInterval; }
            #if UNITY_EDITOR
                set { _rockfallInterval = value; }
            #endif
        }

        //インターバルの最小値
        [SerializeField] private float _bottomInterval = 0.02f;
        public float bottomInterval
        {
            get { return _bottomInterval; }
            #if UNITY_EDITOR
                set { _bottomInterval = value; }
            #endif
        }

        //難易度表示の文字の色
        [SerializeField] private Color _displayColor = Color.white;
        public Color displayColor
        {
            get { return _displayColor; }
            #if UNITY_EDITOR
                set { _displayColor = value; }
            #endif
        }
    }
}
