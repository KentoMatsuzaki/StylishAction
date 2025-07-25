using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    /// <summary>ターゲットを背後から追従するカメラ挙動を制御するクラス</summary>
    public class FollowCamera : MonoBehaviour
    {
        [Header("カメラ設定")] 
        [SerializeField] private float playerFocusHeight; // プレイヤーをロックオンする際の高さのオフセット
        [SerializeField] private float enemyFocusHeight;  // 敵をロックオンする際の高さのオフセット
        [SerializeField] private float offsetY; // カメラのY座標のオフセット
        [SerializeField] private float offsetZ; // カメラのZ座標のオフセット
        [SerializeField] private float sensitivityX; // カメラの水平操作感度
        [SerializeField] private float sensitivityY; // カメラの垂直操作感度
        [SerializeField] private float minPitch; // カメラの最小ピッチ角度
        [SerializeField] private float maxPitch; // カメラの最大ピッチ角度

        private float _yaw = 180f;
        private float _pitch;
        private Vector2 _lookInput;
        private bool _isLockOnEnemy;

        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void LateUpdate()
        {
            UpdateCameraAngles();
            UpdateCameraPosition();
            UpdateCameraRotation();
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
            if (!_isLockOnEnemy)
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
        
        /// <summary>カメラの焦点を取得する</summary>
        private Vector3 GetFocusPosition()
        {
            return _isLockOnEnemy
                ? GameManager.Instance.Enemy.transform.position + Vector3.up * enemyFocusHeight
                : GameManager.Instance.Player.transform.position + Vector3.up * playerFocusHeight;
        }
        
        //-------------------------------------------------------------------------------
        // 視点移動のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnLook(InputAction.CallbackContext context)
        {
            if (!_isLockOnEnemy)
            {
                _lookInput = context.ReadValue<Vector2>();
            }
        }
        
        //-------------------------------------------------------------------------------
        // ロックオンのコールバック
        //-------------------------------------------------------------------------------

        public void OnLockOn(InputAction.CallbackContext context)
        {
            // 入力が実行されたときの処理
            if (context.performed)
            {
                LockOnEnemy();
            }
        }
        
        //-------------------------------------------------------------------------------
        // ロックオンに関する処理
        //-------------------------------------------------------------------------------
        
        private void LockOnEnemy()
        {
            _isLockOnEnemy = !_isLockOnEnemy;

            if (_isLockOnEnemy)
            {
                transform.SetParent(GameManager.Instance.Player.transform);
                transform.localPosition = new Vector3(0, offsetY, offsetZ);
                transform.localRotation = Quaternion.identity;
                GameManager.Instance.Player.isLockOnEnemy = true;
            }
            else
            {
                transform.SetParent(null, true);
                GameManager.Instance.Player.isLockOnEnemy = false;
            }
        }
    }
}