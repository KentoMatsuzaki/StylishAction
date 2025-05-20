using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    /// <summary>ターゲットを背後から追従するカメラ挙動を制御するクラス</summary>
    public class FollowCamera : MonoBehaviour
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
        private float _pitch;
        private Vector2 _lookInput;
        private Transform _lockOnTarget;
        
        public static FollowCamera Instance { get; private set; }
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
        
        //-------------------------------------------------------------------------------
        // ロックオンに関する処理
        //-------------------------------------------------------------------------------
        
        public void SwitchLockOnTarget()
        {
            _lockOnTarget = _lockOnTarget == player ? enemy : player;
        }
    }
}