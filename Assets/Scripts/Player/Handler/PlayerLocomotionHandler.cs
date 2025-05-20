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

        /// <summary>カメラの正面方向へ回転させる</summary>
        /// <param name="cameraTransform">カメラの位置</param>
        public void RotateTowardsCameraForward(Transform cameraTransform)
        {
            // カメラの正面方向を取得する
            var cameraForward = cameraTransform.forward.normalized;
            // 高さを無視する
            cameraForward.y = 0;
            // カメラの正面方向に回転させる
            transform.rotation = Quaternion.LookRotation(cameraForward);
        }

        /// <summary>正面方向にダッシュさせる</summary>
        /// <param name="dashSpeed">ダッシュ速度</param>
        public void DashForward(float dashSpeed)
        {
            _rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
        }
    }
}
