#region What's this?
//タイトル画面の黒幕のフェードインをするためだけのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarFall
{
    public class Fadein_TitleUI : MonoBehaviour
    {
        private Material _image;

        [SerializeField, Range(0, 2)] private float _multiplier;

        void Start()
        {
            _image = this.GetComponent<Image>().material;
            Color currentColor = _image.GetColor("_Color");
            _image.SetColor("_Color", new Color(currentColor.r, currentColor.g, currentColor.b, 1));
        }

        void Update()
        {
            Color currentColor = _image.GetColor("_Color");
            float deltaTime = Time.deltaTime * _multiplier;

            if (currentColor.a <= 0) this.gameObject.SetActive(false);

            _image.SetColor("_Color", new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a -= deltaTime));
        }
    }
}
