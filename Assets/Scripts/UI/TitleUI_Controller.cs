#region What's this?
//タイトル画面を制御するためのスクリプト。状態遷移や色制御、シーン遷移や終了処理をする。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace StarFall
{
    public class TitleUI_Controller : MonoBehaviour
    {
        private GameManager _gameManager;

        [SerializeField] private Image[] _MenuImage = new Image[2];
        [SerializeField] private Text[] _DifficultyText = new Text[4];
        
        private int _currentSelect = 0;
        private int _currentDifficulty = 1;

        private bool _isOpenExtra = false;

        //GC対策のカラー
        private Color _ActiveButtonColor = new Color(0.8f, 0.8f, 0.8f);
        private Color _DisableButtonColor = new Color(1f, 1f, 1f);

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
        }

        void Update()
        {
            /*--------------------------メニュー画面処理--------------------------*/

            if (Input.GetKeyDown(KeyCode.DownArrow))  //↓キーを押したら下のボタンに現在選択してるボタンを移す
            {
                if (_currentSelect < 1) _currentSelect++;  //一番下だったら何もしない
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))  //↑キーを押したら上のボタンに現在選択してるボタンを移す
            {
                if (_currentSelect > 0) _currentSelect--;  //一番上だったら何もしない
            }

            if (Input.GetKeyDown(KeyCode.Escape))  //Escキーを押したらQuitのボタンに移動、二回押すとアプリを終了する
            {
                if (_currentSelect == 1) Quit();
                else
                {
                    _currentSelect = 1;
                    _MenuImage[0].color = _DisableButtonColor;
                    _MenuImage[1].color = _ActiveButtonColor;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))  //→キーを押すと難しい方に難易度を設定する
            {
                int maxValue = 2;
                _isOpenExtra = _gameManager.GetOpenedExtra();

                if (_isOpenExtra) maxValue = 3;

                if (_currentDifficulty < maxValue)  //既に最大難易度だったら何もしない
                {
                    _DifficultyText[_currentDifficulty].enabled = false;
                    _currentDifficulty++;
                    _DifficultyText[_currentDifficulty].enabled = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))  //←キーを押すと易しい方に難易度を設定する
            {
                if (_currentDifficulty > 0)  //既に最小難易度だったら何もしない
                {
                    _DifficultyText[_currentDifficulty].enabled = false;
                    _currentDifficulty--;
                    _DifficultyText[_currentDifficulty].enabled = true;
                }
            }

            for (int i = 0; i < _MenuImage.Length; i++)  //色の設定で、現在選択中のボタンは白く、それ以外は薄暗くする
            {
                if (i == _currentSelect) _MenuImage[i].color = _ActiveButtonColor;
                else _MenuImage[i].color = _DisableButtonColor;
            }

            if (Input.GetKeyDown(KeyCode.Z))  //Zキーを押すと、現在選択中のボタンと難易度を確定させる
            {
                if (_currentSelect == 0)
                {
                    SceneManager.sceneLoaded += InitializingGameSettingProcess;
                    SceneManager.LoadScene("PlayScene");
                }
                else if (_currentSelect == 1) Quit();
            }

            /*--------------------------------------------------------------------*/
        }

        private void InitializingGameSettingProcess(Scene next, LoadSceneMode mode)  //ゲームシーンに切り替えるときに、諸処理をする
        {
            _gameManager.SetDifficulty(_currentDifficulty);  //難易度を設定
            _gameManager.InitializeScore();  //スコアを初期化
            _gameManager.SetPlayingState();  //ゲームの状態をプレイ状態にする
            SceneManager.sceneLoaded -= InitializingGameSettingProcess;
        }

        void Quit()  //アプリを閉じる処理
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
            #endif
        }
    }
}