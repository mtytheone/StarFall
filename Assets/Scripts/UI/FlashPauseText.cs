#region What's this?
//Pauseのテキストを点滅させるためだけのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarFall
{
    public class FlashPauseText : MonoBehaviour
    {
        private Text _text;

        [SerializeField] private float _interval;

        private float _prevTime;

        void Start()
        {
            _text = this.GetComponent<Text>();
            _prevTime = Time.realtimeSinceStartup;
        }

        void Update()
        {
            if (Time.realtimeSinceStartup - _prevTime >= _interval)
            {
                _text.enabled = !_text.enabled;
                _prevTime = Time.realtimeSinceStartup;
            }
        }
    }
}
