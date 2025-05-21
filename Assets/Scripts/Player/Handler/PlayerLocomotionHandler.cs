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

        /// <summary>正面方向に、指定された速度で移動させる</summary>
        public void MoveForward(float moveSpeed)
        {
            transform.Translate(Time.deltaTime * moveSpeed * transform.forward, Space.World);
        }

        /// <summary>
        /// RootMotionによって移動したモデルの位置にプレイヤー本体を同期させ、
        /// モデルのローカル位置をリセットして位置のズレを解消する
        /// </summary>
        public void SyncWithModelPosition(Transform modelTransform)
        {
            // プレイヤー本体の位置をモデルの位置に同期させる
            transform.position = modelTransform.position;
            // モデルのローカル位置を初期化する
            modelTransform.localPosition = Vector3.zero;
        }

        /// <summary>回避アニメーションに合わせて、前方に力を加える</summary>
        public void ApplyDodgeForce(float dodgePower)
        {
            _rb.AddForce(transform.forward * dodgePower, ForceMode.Impulse);
        }
        
        //-------------------------------------------------------------------------------
        // 回転処理
        //-------------------------------------------------------------------------------

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

        /// <summary>Y軸の回転を固定する</summary>
        public void FreezeRotationY()
        {
            _rb.constraints |= RigidbodyConstraints.FreezeRotationY;
        }

        /// <summary>Y軸の回転の固定を解除する</summary>
        public void UnfreezeRotationY()
        {
            _rb.constraints &= ~RigidbodyConstraints.FreezeRotationY;
        }
    }
}
