using System;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーの移動を制御するクラス</summary>
    public class PlayerMoveHandler : MonoBehaviour
    {
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        /// <summary>正面方向に移動させる</summary>
        /// <param name="moveSpeed">移動速度</param>
        public void HandleMoveForward(float moveSpeed)
        {
            transform.Translate(Time.deltaTime * moveSpeed * transform.forward, Space.World);
        }

        /// <summary>移動方向へ回転させる</summary>
        /// <param name="moveDirection">移動方向</param>
        public void HandleRotateTowardsMovement(Vector2 moveDirection)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(-moveDirection.x, 0, -moveDirection.y));
        }
    }
}
