#region What's this?
//ゲームプレイ時に難易度を表示するためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarFall
{
    public class Display_Difficulty : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private GameData _gameData;

        private int _difficulty;
        private Text _difficultyText;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
            _difficultyText = GetComponent<Text>();  //テキストコンポーネントの取得
            _difficulty = _gameManager.GetDifficultyID();  //難易度の取得

            //難易度に応じた色とテキストを設定（元は下のような書き方をしてた）
            _difficultyText.text = _gameData.difficultyStatus[(int)_difficulty].name;
            _difficultyText.color = _gameData.difficultyStatus[(int)_difficulty].displayColor;

            /*switch ((int)_difficulty)
            {
                case 0:
                    _Text.text = "Easy";
                    _Text.color = Color.green;
                    break;
                case 1:
                    _Text.text = "Normal";
                    _Text.color = Color.white;
                    break;
                case 2:
                    _Text.text = "Hard";
                    _Text.color = Color.red;
                    break;
                case 3:
                    _Text.text = "Extra";
                    _Text.color = Color.yellow;
                    break;
            }*/
        }

        void Update()
        {

        }
    }
}
