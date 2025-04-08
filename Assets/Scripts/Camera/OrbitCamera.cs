using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Camera
{
    /// <summary>三人称視点カメラ</summary>
    public class OrbitCamera : MonoBehaviour
    {
        [Header("ターゲット")] 
        [SerializeField] private Transform player;
        [SerializeField] private Transform enemy;
        
        [Header("カメラ設定")] 
        [SerializeField] private float playerFocusHeight;
        [SerializeField] private float enemyFocusHeight;
        [SerializeField] private float offsetY;
        [SerializeField] private float offsetZ;
        [SerializeField] private float sensitivityX;
        [SerializeField] private float sensitivityY;
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;

        private float _yaw = 180f;
        private float _pitch = -17.5f;
        private Vector2 _lookInput;
        private Transform _lockOnTarget;
        
        public static OrbitCamera Instance { get; private set; }
        public bool IsLockingOnEnemy => _lockOnTarget == enemy;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            _lockOnTarget = player;
        }

        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void LateUpdate()
        {
            if (_lockOnTarget != player && !enemy.gameObject.activeInHierarchy)
            {
                SwitchLockOnTarget();
            }
            
            UpdateCameraAngles();
            UpdateCameraPosition();
            UpdateCameraRotation();
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

        /// <summary>カメラの焦点を取得する</summary>
        private Vector3 GetLockOnPosition()
        {
            return _lockOnTarget == player ? 
                player.position + Vector3.up * playerFocusHeight : enemy.position + Vector3.up * enemyFocusHeight;
        }

        /// <summary>カメラの角度を更新する</summary>
        private void UpdateCameraAngles()
        {
            _yaw += _lookInput.x * sensitivityX;
            _pitch -= _lookInput.y * sensitivityY;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
        }
        
        /// <summary>カメラの位置を更新する</summary>
        private void UpdateCameraPosition()
        {
            var rot = Quaternion.Euler(_pitch, _yaw, 0f);
            var offset = rot * new Vector3(0f, offsetY, offsetZ);
            transform.position = player.position + offset;
        }
        
        /// <summary>カメラの回転を更新する</summary>
        private void UpdateCameraRotation()
        {
            transform.LookAt(GetLockOnPosition());
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
        
        //-------------------------------------------------------------------------------
        // ロックオンに関する処理
        //-------------------------------------------------------------------------------
        
        public void SwitchLockOnTarget()
        {
            _lockOnTarget = _lockOnTarget == player ? enemy : player;
        }
    }
}