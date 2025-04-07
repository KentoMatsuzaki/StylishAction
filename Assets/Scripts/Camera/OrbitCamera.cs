using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    /// <summary>三人称視点カメラ</summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("ターゲット設定")] 
        public Transform player;  // プレイヤー
        public Transform enemy;   // 敵
        
        [Header("カメラ設定")]
        public Vector3 offset;　　 // オフセット
        public float moveSpeed;   // 移動速度
        public float rotateSpeed; // 回転速度
        public float maxDistance; // 障害物を判定する最長距離

        private Vector2 _mouseDelta;
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void FixedUpdate()
        {
            // 位置を更新
            //UpdateCameraPosition();
            // 回転を更新
            //UpdateCameraRotation();
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, _mouseDelta.x * rotateSpeed, Space.World);
        }
        
        //-------------------------------------------------------------------------------
        // 視点移動のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnLook(InputAction.CallbackContext context)
        {
            _mouseDelta = context.ReadValue<Vector2>();
        }
        
        //-------------------------------------------------------------------------------
        // カメラに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>カメラの位置を更新する</summary>
        private void UpdateCameraPosition()
        {
            
        }

        /// <summary>カメラの回転を更新する</summary>
        private void UpdateCameraRotation()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, Time.deltaTime * rotateSpeed);
        }

        /// <summary>カメラの位置を調整する</summary>
        private void AdjustCameraPosition()
        {
            var directionToCamera = (transform.position - player.position).normalized;
            if (Physics.Raycast(player.position, directionToCamera, out var hit, maxDistance))
            {
                transform.position = hit.point;
            }
        }
    }
}