using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace  StarFall
{
    public class Introduction_UI : MonoBehaviour
    {
        private GameManager _gameManager;

        [SerializeField] private GameObject _rocket;

        [SerializeField] private Text _title;
        [SerializeField] private List<Text> _control;
        [SerializeField] private Text _addtional;
        [SerializeField] private Text _start;

        private bool _isMovePlayScene;

        private float _prevTime;
        private float _time;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
            _isMovePlayScene = false;
            _prevTime = Time.realtimeSinceStartup;
            _time = 0;  //初期化
        }

        void Update()
        {
            float deltaTime = Time.realtimeSinceStartup - _prevTime;  //時間変位
            _time += deltaTime;

            if (_time >= 2.0f) _title.enabled = true;
            if (_time >= 5.0f)
            {
                foreach (var content in _control) content.enabled = true;
                _rocket.SetActive(true);
            }
            if (_time >= 10.0f) _addtional.enabled = true;
            if (_time >= 15.0f)
            {
                _start.enabled = true;
                _isMovePlayScene = true;
            }

            if(_isMovePlayScene)
            {
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    SceneManager.sceneLoaded += InitializingGameSettingProcess;
                    SceneManager.LoadScene("PlayScene");
                }
            }

            _prevTime = Time.realtimeSinceStartup;
        }

        private void InitializingGameSettingProcess(Scene next, LoadSceneMode mode)  //ゲームシーンに切り替えるときに、諸処理をする
        {
            _gameManager.SetPlayingState();  //ゲームの状態をプレイ状態にする
            _gameManager.DisableInitialMessage();  //二回目以降は表示しないよ！
            SceneManager.sceneLoaded -= InitializingGameSettingProcess;
        }
    }
}