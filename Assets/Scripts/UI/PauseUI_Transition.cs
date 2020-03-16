#region What's this?
//ポーズを実装するためのスクリプト。
#endregion

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace StarFall
{
    public class PauseUI_Transition : MonoBehaviour
    {
        private GameManager _gameManager;

        [SerializeField] private GameObject _pauseUIObject;
        [SerializeField] private GameObject _background;

        private Material _backgroundImageColor;

        private float _prevTime;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
            _backgroundImageColor = _background.GetComponent<Image>().material;  //背景のマテリアルを取得
            _prevTime = Time.realtimeSinceStartup;
        }

        void Update()
        {
            int gameState = _gameManager.GetState();  //ゲームの状態を取得

            float deltaTime = (Time.realtimeSinceStartup - _prevTime) * 2.8f;

            if (Input.GetKeyDown(KeyCode.Escape) && gameState == 1) _gameManager.SetPauseState();  //ゲームがプレイ状態ならEscキーでポーズ状態にする

            if (gameState == 2)  //ゲームがポーズ状態の処理
            {
                _pauseUIObject.SetActive(true);

                _background.SetActive(true);
                Color prevColor = _backgroundImageColor.GetColor("_Color");
                _backgroundImageColor.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, Mathf.Clamp(prevColor.a + deltaTime, 0, 0.7f)));

                Time.timeScale = 0;
            }
            else if (gameState == 1)  //ゲームがプレイ状態になったときの処理
            {
                Color prevColor = _backgroundImageColor.GetColor("_Color");
                _backgroundImageColor.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 0));
                _background.SetActive(false);
                _pauseUIObject.SetActive(false);

                Time.timeScale = 1;
            }

            _prevTime = Time.realtimeSinceStartup;
        }
    }
}