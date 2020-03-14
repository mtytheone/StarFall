#region What's this?
//ゲームオーバー画面を制御するためのスクリプト。
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace StarFall
{
    public class GameOver_UI : MonoBehaviour
    {
        private GameManager _gameManager;

        [SerializeField] private GameObject _gameoverUIObject;
        [SerializeField] private GameObject _background;

        private Material _backgroundImageColor;

        [SerializeField] private List<Text> _genealText = new List<Text>();
        [SerializeField] private Text _1stText;
        [SerializeField] private Text _2ndText;
        [SerializeField] private Text _3rdText;
        [SerializeField] private Text _AdditiveText;
        [SerializeField] private Text _OpenExtraMessage;
        [SerializeField] private Text _TweetMessage;
        [SerializeField] private GameObject _TweetButton;

        private float _prevTime;
        private float _time;

        private bool _isOpenExtra = false;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
            _backgroundImageColor = _background.GetComponent<Image>().material;
            Color prevColor = _backgroundImageColor.GetColor("_Color");
            _backgroundImageColor.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 0));
            _time = 0;  //初期化
        }

        void Update()
        {
            int gameState = _gameManager.GetState();  //ゲームの状態を取得
            float deltaTime = Time.realtimeSinceStartup - _prevTime;  //時間変位

            if (gameState == 3)  //ゲームがゲームオーバー状態の処理
            {
                _gameoverUIObject.SetActive(true);  //Canvas表示
                _background.SetActive(true);  //背景表示
                _gameManager.AdjustScore();  //スコア調整

                //背景のフェードイン
                Color prevColor = _backgroundImageColor.GetColor("_Color");
                _backgroundImageColor.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, Mathf.Clamp(prevColor.a + deltaTime, 0, 0.7f)));

                _isOpenExtra = _gameManager.GetOpenExtra();  //Extraの解放条件を満たしたかどうかを取得

                _time += deltaTime;

                /*--------------------------ランキング表示処理----------------------------*/

                if (_time >= 1.0f)
                {
                    foreach (var generaltext in _genealText) generaltext.enabled = true;  //テキストを表示
                    _gameManager.UpdateHiScore();  //ここでハイスコアランキングを更新
                }

                if (_time >= 2.0f) _3rdText.enabled = true;
                if (_time >= 3.0f) _2ndText.enabled = true;  //ハイスコアランキングを3位から順に発表
                if (_time >= 4.0f) _1stText.enabled = true;

                if (_time >= 5.0f)
                {
                    if(_isOpenExtra) _OpenExtraMessage.enabled = true;  //初めてExtra条件を満たしたらそのメッセージを表示
                }

                if (_time >= 5.5f)
                {
                    _AdditiveText.enabled = true;
                    _TweetMessage.enabled = true;  //タイトル戻る文面とツイートボタンを表示
                    _TweetButton.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        _gameManager.SaveHiScore();  //ハイスコアのセーブをする
                        _gameManager.AcceptReturn();  //タイトルに戻る許可を出す
                    }
                }

                if (_time >= 8.0f)
                {
                    if (_isOpenExtra) _OpenExtraMessage.enabled = false;  //Extra解放メッセージを消す
                }

                /*------------------------------------------------------------------------*/
            }
            /*else if (gameState == 1 || gameState == 2)  //仮にゲームがゲームオーバーじゃなかったときの処理
            {
                _gameoverUIObject.SetActive(false);
                Time.timeScale = 1;
            }*/

            _prevTime = Time.realtimeSinceStartup;
        }
    }
}