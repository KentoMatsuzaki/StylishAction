using Definitions.Enum;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    /// <summary>ターゲットを背後から追従するカメラ挙動を制御するクラス</summary>
    public class FollowCamera : MonoBehaviour
    {
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
        private bool _isLockOn = false;

        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void LateUpdate()
        {
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
            if (_isLockOn) return;
            _lookInput = context.ReadValue<Vector2>();
        }
        
        //-------------------------------------------------------------------------------
        // ロックオンのコールバック
        //-------------------------------------------------------------------------------

        public void OnLockOn(InputAction.CallbackContext context)
        {
            LockOnEnemy();
        }
        
        //-------------------------------------------------------------------------------
        // カメラに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>カメラの焦点を取得する</summary>
        private Vector3 GetFocusPosition()
        {
            return _isLockOn
                ? GameManager.Instance.Enemy.transform.position + Vector3.up * enemyFocusHeight
                : GameManager.Instance.Player.transform.position + Vector3.up * playerFocusHeight;
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
            if (_isLockOn)
            {
                GameManager.Instance.Player.transform.LookAt(GameManager.Instance.Enemy.transform.position);
                transform.localPosition = new Vector3(0, offsetY, offsetZ);
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                var rot = Quaternion.Euler(_pitch, _yaw, 0f);
                var offset = rot * new Vector3(0f, offsetY, offsetZ);
                transform.position = GameManager.Instance.Player.transform.position + offset;
            }
        }
        
        /// <summary>カメラの回転を更新する</summary>
        private void UpdateCameraRotation()
        {
            transform.LookAt(GetFocusPosition());
        }

        private void LockOnEnemy()
        {
            _isLockOn = !_isLockOn;

            if (_isLockOn)
            {
                transform.SetParent(GameManager.Instance.Player.transform);
            }
            else
            {
                transform.SetParent(null, true);
            }
        }
    }
}