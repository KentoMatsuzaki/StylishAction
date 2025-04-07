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
        public void MoveForward(float moveSpeed)
        {
            transform.Translate(Time.deltaTime * moveSpeed * transform.forward, Space.World);
        }

        /// <summary>カメラの向きを基準に、入力方向へ回転させる</summary>
        /// <param name="moveInput">入力方向</param>
        /// <param name="cameraTransform">カメラの位置</param>
        public void RotateToInputRelativeToCamera(Vector2 moveInput, Transform cameraTransform)
        {
            // カメラの正面方向を取得する
            var cameraForward = cameraTransform.forward.normalized;
            cameraForward.y = 0;
            // カメラの右方向を取得する
            var cameraRight = cameraTransform.right.normalized;
            cameraRight.y = 0;
            // 回転方向を計算する
            var moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        /// <summary>正面方向にダッシュさせる</summary>
        /// <param name="dashSpeed">ダッシュ速度</param>
        public void DashForward(float dashSpeed)
        {
            _rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
        }
    }
}
