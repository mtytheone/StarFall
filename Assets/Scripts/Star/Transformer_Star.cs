#region What's this?
//星を回転させるためのスクリプト。
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarFall
{
    public class Transformer_Star : MonoBehaviour
    {
        private GameManager _gameManager;

        private Transform _transform;

        private float _rotationSpeed;

        [SerializeField]
        private float _Speed;

        void Start()
        {
            _gameManager = GameManager.instance;
            _transform = this.GetComponent<Transform>();
            _rotationSpeed = Random.Range(-8, 8);
        }


        void Update()
        {
            if (_gameManager.GetState() == 1 || _gameManager.GetState() == 3) _transform.Rotate(0, 0, _rotationSpeed * _Speed);
        }
    }
}
