#region What's this?
//プレイヤーに関する様々な処理をするためのスクリプト。移動と死亡判定をする。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarFall
{
    public class Player_Controller : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private PlayerData _playerData;

        private float _slowSpeed;
        private float _fastSpeed;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
            _slowSpeed = _playerData.slowSpeed;  //PlayerData.assetから低速移動時の速さを取得
            _fastSpeed = _playerData.fastSpeed;  //PlayerData.assetから高速移動時の速さを取得
        }

        void Update()
        {
            /*-------------------------------------------------------上下左右の移動処理----------------------------------------------------------*/

            if (_gameManager.GetState() == 1)  //ゲームの状態がプレイ状態なら
            {
                float deltaTime = Time.deltaTime;  //フレームの変化を操作に反映するために必要

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))  //左移動操作
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) transform.Translate(-_slowSpeed * deltaTime, 0, 0);
                    else transform.Translate(-_fastSpeed * deltaTime, 0, 0);
                }
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))  //右移動操作
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) transform.Translate(_slowSpeed * deltaTime, 0, 0);
                    else transform.Translate(_fastSpeed * deltaTime, 0, 0);
                }
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))  //上移動操作
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) transform.Translate(0, _slowSpeed * deltaTime, 0);
                    else transform.Translate(0, _fastSpeed * deltaTime, 0);
                }
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))  //下移動操作
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) transform.Translate(0, -_slowSpeed * deltaTime, 0);
                    else transform.Translate(0, -_fastSpeed * deltaTime, 0);
                }
            }

            /*-----------------------------------------------------------------------------------------------------------------------------------*/





            /*--------------------------------画面外に出たときの処理--------------------------------*/

            if (transform.position.y <= _playerData.DeathY)  //規定Y座標よりも下に行ったら
            {
                _gameManager.SetGameOverState();  //ゲーム状態をゲームオーバーにして
                Destroy(this.gameObject);  //ロケットは消去
            }

            /*--------------------------------------------------------------------------------------*/
        }

        private void FixedUpdate()  //固定フレームレート
        {
            /*--------------------------------移動範囲制限設定--------------------------------*/

            Vector2 pos = transform.position;
            pos.y -= _playerData.fallSpeed * Time.deltaTime;  //落下処理
            pos.x = Mathf.Clamp(pos.x, _playerData.clampMove[0].x, _playerData.clampMove[0].y);
            pos.y = Mathf.Clamp(pos.y, _playerData.clampMove[1].x, _playerData.clampMove[1].y);
            transform.position = pos;

            /*--------------------------------------------------------------------------------*/
        }


        private void OnTriggerEnter2D(Collider2D collision)  //星に衝突した時の処理
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _gameManager.SetGameOverState();  //ゲームオーバーにする
                Destroy(this.gameObject);  //ロケットは消去
            }
        }
    }
}