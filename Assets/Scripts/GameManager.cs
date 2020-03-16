#region What's this?
//ゲームの統括をするスクリプト。難易度やゲーム状態、スコアといったデータを保持したり、ゲーム全体の流れの処理をしたりする。
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using hatuxes.Saves;

namespace StarFall
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        [SerializeField] private GameData _gameData;  //ゲームの初期値データ

        enum Difficulty { Easy = 0, Normal = 1, Hard = 2, Extra = 3 }
        private Difficulty _difficulty;  //ゲームの難易度

        //enum State { Playing=0, GameOver=1, Pause=2, Title=3 }
        enum State { Title = 0, Playing = 1, Pause = 2, GameOver = 3 }
        private State _state;  //ゲームの現在状態

        private HiScoreData _hiScoreData;
        private float _hiScore1st;
        private float _hiScore2nd;
        private float _hiScore3rd;
        private float _score;  //スコア

        private bool _isOpenExtra;  //Extraステージを開放するかどうかのフラグ
        private bool _isReturn;  //タイトル画面に戻るかどうかを判定するフラグ
        private float _difficultyMultiplier;  //難易度倍率

        private bool _isUpdateHiScore;  //ゲーム中に1位のハイスコアを更新するかどうかのフラグ

        void Awake()
        {
            //instance変数が空だったらこのGameManagerをDontDestroyOnLoadに設定
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            //instance変数に何か代入されていたらこれは必要ないため削除
            else Destroy(this.gameObject);

            _hiScoreData = new HiScoreData(-1);  //HiScoreDataを新規作成

            HiScoreData LoadData = SaveSystem.Load(0);
            _hiScoreData.wasOpenExtra = LoadData.wasOpenExtra;  //Extra達成条件済フラグをロード
            _hiScoreData.initialMessage = LoadData.initialMessage;  //操作画面を出すかどうかのフラグをロード
        }

        void Start()
        {

            /*-----------------------------------------初期値設定-----------------------------------------*/

            _state = (State)0;  //ゲームの状態をTitle状態に
            _hiScore1st = -1;
            _hiScore2nd = -1;  //ハイスコアは初期値-1にしておく（ロードで0にするのをわかりやすくするため）
            _hiScore3rd = -1;
            _score = 0;
            _isOpenExtra = false;  //Extra解放フラグは初期値false
            _isReturn = false;  //タイトルシーンに戻るのを許可するフラグの初期値false
            _isUpdateHiScore = true;  //ハイスコア更新許可フラグの初期値はtrueで

            /*--------------------------------------------------------------------------------------------*/
        }

        void Update()
        {
            /*--------------------------------------倍率設定--------------------------------------*/

            //難易度に応じたスコア倍率を設定（元は下のような書き方をしてた）
            _difficultyMultiplier = _gameData.difficultyStatus[(int)_difficulty].scoreMultiplier;

            /*switch ((int)_difficulty)
            {
                case 0:
                    _difficultyMultiplier = 0.8f;
                    break;
                case 1:
                    _difficultyMultiplier = 1.0f;
                    break;
                case 2:
                    _difficultyMultiplier = 1.2f;
                    break;
                case 3:
                    _difficultyMultiplier = 2.0f;
                    break;
            }*/

            /*------------------------------------------------------------------------------------*/





            /*--------------------------------------スコア処理--------------------------------------*/

            //スコアがハイスコアを超えたらハイスコアにも同じ点数を代入
            if ((int)_state == 1 && _hiScore1st <= _score)
            {
                if (_isUpdateHiScore)  //最初の一回はハイスコアを更新する
                {
                    _hiScore3rd = _hiScore2nd;
                    _hiScore2nd = _hiScore1st;  //ハイスコアの順位をずらす
                    _hiScore1st = _score;

                    _isUpdateHiScore = false;
                }
                else _hiScore1st = _score;  //ハイスコアの順位をずらしたらあとは1位を更新し続ける
            }

            /*--------------------------------------------------------------------------------------*/





            /*----------------------------------Extra開放処理----------------------------------*/

            if (!_hiScoreData.wasOpenExtra)  //まだExtraを解放していないなら
            {
                if ((int)_difficulty == 1 && _score >= 2000)  //Normal、スコア2000以上で解放
                {
                    _isOpenExtra = true;
                    _hiScoreData.wasOpenExtra = true;
                }
                else if ((int)_difficulty == 2 && _score >= 2800)  //Hard、スコア2800以上で解放
                {
                    _isOpenExtra = true;
                    _hiScoreData.wasOpenExtra = true;
                }
            }

            /*---------------------------------------------------------------------------------*/





            /*--------------------------------シーン遷移設定--------------------------------*/

            //もしGameOverにステータスが設定されたら、後処理をして約2秒後にタイトルに移動する
            if ((int)_state == 3)
            {
                if(_isReturn)  //GameOverUIに戻ることを許可されたらタイトルシーンに戻る
                {
                    SceneManager.sceneLoaded += GameOverProcess;
                    SceneManager.LoadScene("Title");
                }
            }

            /*------------------------------------------------------------------------------*/
        }

        public void SaveHiScore() //ハイスコアのセーブ
        {
            //ハイスコアをHiScoreDataに適応する
            _hiScoreData._1stHiScoreLists[(int)_difficulty] = _hiScore1st;
            _hiScoreData._2ndHiScoreLists[(int)_difficulty] = _hiScore2nd;
            _hiScoreData._3rdHiScoreLists[(int)_difficulty] = _hiScore3rd;

            SaveSystem.Save(_hiScoreData);  //セーブ
        }

        private void GameOverProcess(Scene next, LoadSceneMode mode)  //ゲームオーバー時の諸処理
        {
            /*---------------全てのデータを元に戻す---------------*/

            _state = (State)3;
            _score = 0;
            _hiScore1st = -1;
            _hiScore2nd = -1;
            _hiScore3rd = -1;
            Time.timeScale = 1;
            _isReturn = false;
            _isOpenExtra = false;
            _isUpdateHiScore = true;

            /*----------------------------------------------------*/

            SceneManager.sceneLoaded -= GameOverProcess;
        }


        /*-----------------------------------------------------スコア関係（Public関数）-----------------------------------------------------*/

        public float GetScore()  //スコアの取得（外部使用）
        {
            return _score;
        }

        public float GetHiScore1st()  //1位のハイスコアの取得（外部使用）
        {
            return _hiScore1st;
        }

        public float GetHiScore2nd()  //2位のハイスコアの取得（外部使用）
        {
            return _hiScore2nd;
        }

        public float GetHiScore3rd()  //3位のハイスコアの取得（外部使用）
        {
            return _hiScore3rd;
        }

        public void AddScore(float addscore)  //スコアの追加（外部使用）
        {
            _score += addscore * _difficultyMultiplier;
        }

        public void InitializeScore()  //スコアの初期化（外部使用）
        {
            _hiScoreData = SaveSystem.Load(0);  //ハイスコアを取得

            //GameManagerのHiScoreDataに適応
            _hiScore1st = _hiScoreData._1stHiScoreLists[(int)_difficulty];
            _hiScore2nd = _hiScoreData._2ndHiScoreLists[(int)_difficulty];
            _hiScore3rd = _hiScoreData._3rdHiScoreLists[(int)_difficulty];

            _score = 0;  //スコアの初期化
        }

        public void RevertScore()  //スコアの破棄・初期化（外部使用）
        {
            _hiScore1st = -1;
            _hiScore2nd = -1;
            _hiScore3rd = -1;
            _score = 0;
        }

        public void UpdateHiScore()  //ハイスコアの更新（外部使用）
        {
            if (_hiScore2nd < _score && _score < _hiScore1st)  //2位の処理
            {
                _hiScore3rd = _hiScore2nd;
                _hiScore2nd = _score;
                return;
            }
            if (_hiScore3rd < _score && _score < _hiScore2nd)  //3位の処理
            {
                _hiScore3rd = _score;
                return;
            }
        }

        public void AdjustScore()  //スコアはハイスコアよりもフレーム処理速度が追い付かなくて高く表示されてしまった時の調整用関数（外部使用）
        {
            if (_score > _hiScore1st) _score = _hiScore1st;
        }

        /*----------------------------------------------------------------------------------------------------------------------------------*/


        /*---------------------------------難易度関係（Public関数）---------------------------------*/

        public string GetDifficultyName()  //難易度の名前を取得（外部使用）
        {
            return _difficulty.ToString();
        }

        public int GetDifficultyID()  //難易度番号の取得（外部使用）
        {
            return (int)_difficulty;
        }

        public void SetDifficulty(int setdifficulty) //難易度の設定（外部使用）
        {
            _difficulty = (Difficulty)setdifficulty;
        }

        /*------------------------------------------------------------------------------------------*/


        /*---------------------------------ゲーム状態関係（Public関数）---------------------------------*/


        public int GetState()  //現在のゲームの状態を取得（外部使用）
        {
            return (int)_state;
        }

        public void SetTitleState()  //ゲームをタイトル状態に変更（外部使用）
        {
            _state = (State)0;
        }

        public void SetPlayingState()  //ゲームをプレイ状態に変更（外部使用）
        {
            _state = (State)1;
        }

        public void SetPauseState()  //ゲームをゲームオーバー状態に変更（外部使用）
        {
            _state = GetState() == 2 ? (State)1 : (State)2;
        }

        public void SetGameOverState()  //ゲームをゲームオーバー状態に変更（外部使用）
        {
            _state = (State)3;
        }

        /*----------------------------------------------------------------------------------------------*/


        /*---------------------------------Extra関係（Public関数）---------------------------------*/

        public bool GetOpenExtra()  //Extraステージが解放されたかどうかを判断するフラグを取得（外部使用）
        {
            return _isOpenExtra;
        }

        public bool GetOpenedExtra()  //Extraステージが既に解放されたかどうかを判断するフラグを取得（外部使用）
        {
            return _hiScoreData.wasOpenExtra;
        }

        /*-----------------------------------------------------------------------------------------*/


        /*-----------------------------初期メッセージ関係（Public関数）-----------------------------*/

        public bool GetInitialMessage()
        {
            return _hiScoreData.initialMessage;
        }

        public void DisableInitialMessage()
        {
            _hiScoreData.initialMessage = false;
        }

        /*------------------------------------------------------------------------------------------*/


        public void AcceptReturn()  //タイトルシーンに戻るのを許可する関数（外部使用）
        {
            _isReturn = true;
        }
    }
}
