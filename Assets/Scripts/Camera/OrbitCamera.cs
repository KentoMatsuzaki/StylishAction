using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    /// <summary>三人称視点カメラ</summary>
    public class OrbitCamera : MonoBehaviour
    {
        [Header("ターゲット")] 
        [SerializeField] private Transform player;

        [Header("カメラ設定")] 
        [SerializeField] private float height;
        [SerializeField] private float offsetY;
        [SerializeField] private float offsetZ;
        [SerializeField] private float sensitivityX;
        [SerializeField] private float sensitivityY;
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;

        private float _yaw = 180f;
        private float _pitch = -17.5f;
        private Vector2 _lookInput;
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void LateUpdate()
        {
            _yaw += _lookInput.x * sensitivityX;
            _pitch -= _lookInput.y * sensitivityY;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            var targetPosition = player.position + Vector3.up * height;
            var rot = Quaternion.Euler(_pitch, _yaw, 0f);
            var offset = rot * new Vector3(0f, offsetY, offsetZ);
            
            transform.position = targetPosition + offset;
            transform.LookAt(targetPosition);
        }
        
        //-------------------------------------------------------------------------------
        // 視点移動のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnLook(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
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
            
        }

        /// <summary>カメラの位置を調整する</summary>
        private void AdjustCameraPosition()
        {
            // var directionToCamera = (transform.position - player.position).normalized;
            // if (Physics.Raycast(player.position, directionToCamera, out var hit, maxDistance))
            // {
            //     transform.position = hit.point;
            // }
        }
    }
}