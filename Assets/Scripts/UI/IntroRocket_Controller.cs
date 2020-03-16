using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarFall
{
    public class IntroRocket_Controller : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private PlayerData _playerData;

        private RectTransform _rect;

        [SerializeField] private Text[] _arrow;  //Up, Down, Left, Right

        private float _slowSpeed;
        private float _fastSpeed;

        private float _rectMultiplier;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
            _rect = this.GetComponent<RectTransform>();
            _slowSpeed = _playerData.slowSpeed;  //PlayerData.assetから低速移動時の速さを取得
            _fastSpeed = _playerData.fastSpeed;  //PlayerData.assetから高速移動時の速さを取得
            _rectMultiplier = 32;  //RectTransformとTransformで移動速度が異なるのを解消するための疑似的な係数
        }

        void Update()
        {
            /*-------------------------------------------------------上下左右の移動処理----------------------------------------------------------*/

            if (_gameManager.GetState() == 0)  //ゲームの状態がプレイ状態なら
            {
                float deltaTime = Time.deltaTime;  //フレームの変化を操作に反映するために必要

                foreach (var color in _arrow) color.color = Color.white;

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))  //左移動操作
                {
                    //_arrow[2].material.SetColor("_Color", Color.yellow);
                    _arrow[2].color = Color.yellow;

                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) _rect.anchoredPosition += new Vector2(-_slowSpeed * deltaTime * _rectMultiplier, 0);
                    else _rect.anchoredPosition += new Vector2(-_fastSpeed * deltaTime * _rectMultiplier, 0);
                }
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))  //右移動操作
                {
                    _arrow[3].color = Color.yellow;

                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) _rect.anchoredPosition += new Vector2(_slowSpeed * deltaTime * _rectMultiplier, 0);
                    else _rect.anchoredPosition += new Vector2(_fastSpeed * deltaTime * _rectMultiplier, 0);
                }
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))  //上移動操作
                {
                    _arrow[0].color = Color.yellow;

                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) _rect.anchoredPosition += new Vector2(0, _slowSpeed * deltaTime * _rectMultiplier);
                    else _rect.anchoredPosition += new Vector2(0, _fastSpeed * deltaTime * _rectMultiplier);
                }
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))  //下移動操作
                {
                    _arrow[1].color = Color.yellow;

                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) _rect.anchoredPosition += new Vector2(0, -_slowSpeed * deltaTime * _rectMultiplier);
                    else _rect.anchoredPosition += new Vector2(0, -_fastSpeed * deltaTime * _rectMultiplier);
                }
            }

            /*-----------------------------------------------------------------------------------------------------------------------------------*/
        }

        private void FixedUpdate()  //固定フレームレート
        {
            /*--------------------------------移動範囲制限設定--------------------------------*/

            Vector2 pos = _rect.anchoredPosition;
            pos.y -= _playerData.fallSpeed * Time.deltaTime * _rectMultiplier;  //落下処理
            pos.x = Mathf.Clamp(pos.x, -403, 403);
            pos.y = Mathf.Clamp(pos.y, -186.4f, 194);
            _rect.anchoredPosition = pos;

            /*--------------------------------------------------------------------------------*/
        }
    }
}
