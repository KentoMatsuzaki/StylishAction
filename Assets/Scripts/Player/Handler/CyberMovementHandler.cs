using Managers;
using Player.Interface;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>
    /// Cyber（プレイヤー）の移動を制御するクラス
    /// </summary>
    public class CyberMovementHandler : MonoBehaviour, IPlayerMovementHandler
    {
        private Rigidbody _rb;
        
        public Vector3 MoveDirection { get; private set; } // 移動方向
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        
        //-------------------------------------------------------------------------------
        // 移動に関する処理
        //-------------------------------------------------------------------------------

        public void SetMoveDirection(Vector2 inputDirection)
        {
            MoveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        }

        public void ResetMoveDirection()
        {
            MoveDirection = Vector3.zero;
        }

        public void MoveForward(float moveForce)
        {
            if (MoveDirection != Vector3.zero) 
                _rb.AddForce(transform.forward * moveForce, ForceMode.Force);
        }

        public void MoveStrafe(float moveForce)
        {
            if (MoveDirection != Vector3.zero)
            {
                Vector3 worldDirection = transform.TransformDirection(MoveDirection.normalized);
                _rb.AddForce(worldDirection * moveForce, ForceMode.Force);
            }
        }

        public void ApplyRootMotion(Vector3 deltaPosition)
        {
            _rb.MovePosition(_rb.position + deltaPosition);
        }
        
        //-------------------------------------------------------------------------------
        // 回転に関する処理
        //-------------------------------------------------------------------------------

        public void RotateTowardsCameraRelativeDir(Transform cameraTransform)
        {
            // カメラの正面方向・右方向を取得する
            var cameraForward = cameraTransform.forward.normalized; 
            var cameraRight = cameraTransform.right.normalized; 
            cameraForward.y = 0; 
            cameraRight.y = 0;
            
            // 入力方向をカメラ基準に変換
            var dir = cameraForward * MoveDirection.z + cameraRight * MoveDirection.x; 
            
            // 求めた方向へ回転させる
            if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir); 
        }

        public void RotateTowardsEnemyInstantly()
        {
            var enemyPos = GameManager.Instance.Enemy.transform.position;
            var dir = new Vector3(enemyPos.x, 0, enemyPos.z) - new Vector3(transform.position.x,
                0, transform.position.z);
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}