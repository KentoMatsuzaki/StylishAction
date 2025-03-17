using UnityEngine;

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
        
        private void FixedUpdate()
        {
            // 位置を更新
            UpdateCameraPosition();
            // 
            UpdateCameraRotation();
            
            //AdjustCameraPosition();
        }

        /// <summary>カメラの位置を更新する</summary>
        private void UpdateCameraPosition()
        {
            var targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        /// <summary>カメラの回転を更新する</summary>
        private void UpdateCameraRotation()
        {
            var targetPosition = new Vector3(enemy.position.x, enemy.position.y + 1f, enemy.position.z);
            var targetDirection = (targetPosition - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
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