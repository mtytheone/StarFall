﻿#region What's this?
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
        
        private int _currentSelect;
        private int _currentDifficulty;

        private bool _isOpenExtra = false;
        private bool _initialMessage;

        //GC対策のカラー
        private Color _ActiveButtonColor = new Color(1f, 1f, 1f);
        private Color _DisableButtonColor = new Color(0.4f, 0.4f, 0.4f);

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得

            _currentDifficulty = _gameManager.GetDifficultyID();  //難易度を取得
            for (int i = 0; i < _DifficultyText.Length; i++)  //前回選択した難易度にする
            {
                if (i == _currentDifficulty) _DifficultyText[i].enabled = true;
                else _DifficultyText[i].enabled = false;
            }

            _currentSelect = 0;
            _initialMessage = _gameManager.GetInitialMessage();  //操作説明画面を出すかどうかのフラグを取得
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
                    if (_initialMessage)
                    {
                        SceneManager.sceneLoaded += IntroductionProcess;
                        SceneManager.LoadScene("Introduction");
                    }
                    else
                    {
                        SceneManager.sceneLoaded += InitializingGameSettingProcess;
                        SceneManager.LoadScene("PlayScene");
                    }
                }
                else if (_currentSelect == 1) Quit();
            }

            /*--------------------------------------------------------------------*/
        }

        private void IntroductionProcess(Scene next, LoadSceneMode mode)  //操作説明シーンに切り替えるときに、諸処理をする
        {
            _gameManager.SetDifficulty(_currentDifficulty);  //難易度を設定
            _gameManager.InitializeScore();  //スコアを初期化
            SceneManager.sceneLoaded -= IntroductionProcess;
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