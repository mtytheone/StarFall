#region What's this?
//ゲームプレイ時にスコアを表示するためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarFall
{
    public class Display_Score : MonoBehaviour
    {
        private GameManager _gameManager;

        [SerializeField] private Text _hiScoreText;
        [SerializeField] private Text _scoreText;

        private float _hiScore;
        private float _score;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
            GetScoreData();  //スコア・ハイスコアを取得
        }

        void Update()
        {
            GetScoreData();  //スコア・ハイスコアを取得
            SetScoreData();  //スコア・ハイスコアをテキストに設定
        }

        void GetScoreData()  //スコア・ハイスコアを取得
        {
            _hiScore = _gameManager.GetHiScore1st();
            _score = _gameManager.GetScore();
        }

        void SetScoreData()  //スコア・ハイスコアをテキストに設定
        {
            _hiScoreText.text = _hiScore.ToString();
            _scoreText.text = _score.ToString();
        }
    }
}
