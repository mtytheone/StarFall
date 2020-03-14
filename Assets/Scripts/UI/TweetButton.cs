#region What's this?
//ツイートボタンの処理を実装するためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace StarFall
{
    public class TweetButton : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
        }

        public void TweetScore()  //Tweet処理
        {
            string text = UnityWebRequest.EscapeURL("私は" + _gameManager.GetDifficultyName() + "モードでスコア \"" + _gameManager.GetScore().ToString() + "\" でした！\nあなたもやってみる？\nhttps://github.com/mtytheone/StarFall/releases");
            string hashtag = UnityWebRequest.EscapeURL("StarFall");

            string url = "https://twitter.com/intent/tweet?text=" + text + "&hashtags=" + hashtag;

            Application.OpenURL(url);
        }
    }
}