#region What's this?
//星を生成するためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarFall
{
    public class Spawn_Star : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField]
        private GameData _gameData;

        [SerializeField]
        private GameObject[] _Prefab;

        private int _difficulty;
        private float _interval;
        private float _bottomInterbal;

        private float _prevTime;

        void Start()
        {
            _gameManager = GameManager.instance;  //staticなGameManagerを取得
            _difficulty = _gameManager.GetDifficultyID();  //難易度の取得

            _interval = _gameData.difficultyStatus[_difficulty].rockfallInterval;  //難易度に応じた岩の降るインターバルを取得（元は下のような書き方してた）
            _bottomInterbal = _gameData.difficultyStatus[_difficulty].bottomInterval;  //難易度に応じたインターバルの最小値を取得（元は下のような書き方してた）

            /*switch(_difficulty)
            {
                case 0:
                    _interbal = 2.0f;
                    _bottomInterbal = 0.1f;
                    break;
                case 1:
                    _interbal = 1.0f;
                    _bottomInterbal = 0.02f;
                    break;
                case 2:
                    _interbal = 0.6f;
                    _bottomInterbal = 0;
                    break;
                case 3:
                    _interbal = 0;
                    _bottomInterbal = 0;
                    break;
            }}*/

            _prevTime = 0;
        }

        void Update()
        {
            if (_gameManager.GetState() == 1)  //一定周期で
            {
                if (Time.realtimeSinceStartup - _prevTime >= _interval)  //一定周期で
                {
                    int random_star = Random.Range(0, _Prefab.Length);  //インデックス番号をランダムに指定
                    Vector3 coodinate = new Vector3(Random.Range(-10.0f, 4.15f), this.transform.position.y, 0);  //座標をランダムに指定
                    float rotation_offset = Random.Range(0, 30);  //初期角度を指定

                    GameObject star = Instantiate(_Prefab[random_star], coodinate, Quaternion.Euler(0, 0, rotation_offset), this.transform);  //星を生成
                    star.transform.localScale *= Random.Range(0.7f, 1f);  //サイズを指定

                    _prevTime = Time.realtimeSinceStartup;  //前の時間を今の時間にする
                }
                _interval -= _interval >= _bottomInterbal ? 0.0002f : 0;  //インターバルを徐々に短くしていく
            }
        }
    }
}
