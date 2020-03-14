#region What's this?
//星を消去するためのスクリプト。消去時に点数を加算する。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarFall
{
    public class Delete_Star : MonoBehaviour
    {
        private GameManager _gameManager;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
        }

        void Update()
        {

        }

        //ロケットに衝突したとき
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player")) Destroy(this.gameObject);
        }

        //岩が落下しきった時、ゲームがプレイ状態なら点数を加算する
        private void OnBecameInvisible()
        {
            if(_gameManager.GetState() == 1) _gameManager.AddScore(10.0f);
            Destroy(this.gameObject);
        }
    }
}
