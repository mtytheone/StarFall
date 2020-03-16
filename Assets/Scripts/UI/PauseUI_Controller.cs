#region What's this?
//ポーズ画面を制御するためのスクリプト。状態遷移や色制御、シーン遷移をする。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace StarFall
{
    public class PauseUI_Controller : MonoBehaviour
    {
        private GameManager _gameManager;

        [SerializeField] private Image[] _ButtonImage = new Image[2];

        private int _currentSelect = 0;

        //GC対策のカラー
        private Color _ActiveButtonColor = new Color(1f, 1f, 1f);
        private Color _DisableButtonColor = new Color(0.4f, 0.4f, 0.4f);

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
                if (_currentSelect == 1)
                {
                    _currentSelect = 0;
                    _ButtonImage[0].color = _ActiveButtonColor;
                    _ButtonImage[1].color = _DisableButtonColor;
                }
            }

            for (int i = 0; i < _ButtonImage.Length; i++)  //色の設定で、現在選択中のボタンは白く、それ以外は薄暗くする
            {
                if (i == _currentSelect) _ButtonImage[i].color = _ActiveButtonColor;
                else _ButtonImage[i].color = _DisableButtonColor;
            }

            if (Input.GetKeyDown(KeyCode.Z))  //Zキーを押すと、現在選択中のボタンと難易度を確定させる
            {
                if (_currentSelect == 0) _gameManager.SetPlayingState();
                else if (_currentSelect == 1) BackTitle();
            }

            /*--------------------------------------------------------------------*/
        }

        private void RevertGameSettingProcess(Scene next, LoadSceneMode mode)  //ゲームシーンに切り替えるときに、諸処理をする
        {
            _gameManager.RevertScore();  //スコアを破棄する
            _gameManager.SetTitleState();  //ゲームの状態をタイトル状態にする
            Time.timeScale = 1;  //時の流れを戻す
            SceneManager.sceneLoaded -= RevertGameSettingProcess;
        }

        void BackTitle()  //タイトルに戻る処理
        {
            SceneManager.sceneLoaded += RevertGameSettingProcess;
            SceneManager.LoadScene("Title");
        }
    }
}