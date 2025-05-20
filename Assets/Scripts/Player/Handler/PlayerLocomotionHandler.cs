using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Handler
{
    /// <summary>プレイヤーの移動と回転を制御するクラス</summary>
    public class PlayerLocomotionHandler : MonoBehaviour
    {
        /// <summary>移動方向</summary>
        public Vector3 MoveDirection { get; set; }
        
        private Rigidbody _rb;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        
        //-------------------------------------------------------------------------------
        // 移動処理
        //-------------------------------------------------------------------------------
        
        /// <summary>移動方向を設定する</summary>
        /// <param name="moveInput">移動の入力値</param>
        public void SetMoveDirection(Vector2 moveInput)
        {
            // 移動の入力値をVector3に変換して代入する
            MoveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        }

        /// <summary>入力に基づいて計算された移動方向に、指定された速度で移動させる</summary>
        /// <param name="moveSpeed">移動速度</param>
        public void MoveInputDirection(float moveSpeed)
        {
            transform.Translate(Time.deltaTime * moveSpeed * MoveDirection, Space.Self);
        }

        /// <summary>正面方向に、指定された速度で移動させる</summary>
        public void MoveForward(float moveSpeed)
        {
            transform.Translate(Time.deltaTime * moveSpeed * transform.forward, Space.World);
        }
        
        //-------------------------------------------------------------------------------
        // 回転処理
        //-------------------------------------------------------------------------------

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

        /// <summary>カメラを基準とした入力方向へ回転させる</summary>
        /// <param name="cameraTransform">カメラの位置</param>
        public void RotateTowardsCameraRelativeDirection(Transform cameraTransform)
        {
            // カメラの正面方向を取得する
            var cameraForward = cameraTransform.forward.normalized;
            // 高さを無視する
            cameraForward.y = 0;
            // カメラの右方向を取得する
            var cameraRight = cameraTransform.right.normalized;
            // 高さを無視する
            cameraRight.y = 0;
            // 回転方向を計算する
            var rotation = cameraForward * MoveDirection.z + cameraRight * MoveDirection.x;
            // 回転させる
            transform.rotation = Quaternion.LookRotation(rotation);
        }
    }
}
