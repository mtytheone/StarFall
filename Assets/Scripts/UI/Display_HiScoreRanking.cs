#region What's this?
//ゲームオーバー時のハイスコアランキングを取得して表示するためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarFall
{
    public class Display_HiScoreRanking : MonoBehaviour
    {
        private GameManager _gameManager;

        [SerializeField] private Text _1stText;
        [SerializeField] private Text _2ndText;
        [SerializeField] private Text _3rdText;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
        }

        void Update()
        {
            if (_gameManager.GetState() == 3)  //ゲーム状態はゲームオーバーだったら
            {
                _1stText.text = _gameManager.GetHiScore1st().ToString();
                _2ndText.text = _gameManager.GetHiScore2nd().ToString();  //ハイスコアを取得してセット
                _3rdText.text = _gameManager.GetHiScore3rd().ToString();
            }
        }
    }
}