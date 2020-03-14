#region What's this?
//プレイヤーのデータの初期値を格納するScriptableObjectを定義するためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StarFall
{
    public class PlayerData : ScriptableObject
    {
        //高速移動速度
        [SerializeField] private float _fastSpeed = 0.2f;
        public float fastSpeed
        {
            get { return _fastSpeed; }
            #if UNITY_EDITOR
                set { _fastSpeed = value; }
            #endif
        }

        //低速移動速度
        [SerializeField] private float _slowSpeed = 0.12f;
        public float slowSpeed
        {
            get { return _slowSpeed; }
            #if UNITY_EDITOR
                set { _slowSpeed = value; }
            #endif
        }

        //落下速度
        [SerializeField] private float _fallSpeed = 0.04f;
        public float fallSpeed
        {
            get { return _fallSpeed; }
            #if UNITY_EDITOR
                set { _fallSpeed = value; }
            #endif
        }

        //ゲームオーバー判定のY座標
        [SerializeField] private float _DeathY = -6.0f;
        public float DeathY
        {
            get { return _DeathY; }
            #if UNITY_EDITOR
                set { _DeathY = value; }
            #endif
        }

        //ロケットの移動制限
        [SerializeField]
        [Header("Element0はleftとright、Element1はdownとtop")]
        private Vector2[] _clampMove = { new Vector2(-10, 3.3f), new Vector2(-7, 6) };
        public Vector2[] clampMove
        {
            get { return _clampMove; }
            #if UNITY_EDITOR
                set { _clampMove = value; }
            #endif
        }
    }
}
