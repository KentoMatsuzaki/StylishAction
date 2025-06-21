using Enemy.Interface;
using Managers;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>Death（敵）の移動を制御するクラス</summary>
    public class DeathMovementHandler : MonoBehaviour, IEnemyMovementHandler
    {
        private Rigidbody _rigidbody;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        //-------------------------------------------------------------------------------
        // 回転に関する処理
        //-------------------------------------------------------------------------------

        public void RotateTowardsPlayer(float rotateSpeed)
        {
            var rotation = Quaternion.LookRotation(GetHorizontalDirectionToPlayer());
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }

        /// <summary>プレイヤーへの方向を求める（Y座標を無視する）</summary>
        private Vector3 GetHorizontalDirectionToPlayer()
        {
            var playerPos = GameManager.Instance.Player.transform.position;
            return (new Vector3(playerPos.x, 0, playerPos.z) - new Vector3(transform.position.x, 0, transform.position.z));
        }
        
        //-------------------------------------------------------------------------------
        // 移動に関する処理
        //-------------------------------------------------------------------------------

        public void MoveTowardsPlayer(float moveForce)
        {
            _rigidbody.AddForce(GetHorizontalDirectionToPlayer() * moveForce, ForceMode.Force);
        }

        public void MoveAwayFromPlayer(float moveForce)
        {
            _rigidbody.AddForce(GetHorizontalDirectionToPlayer() * -moveForce, ForceMode.Force);
        }
        
        /// <summary>プレイヤーとの距離を求める（Y座標を無視する）</summary>
        private float GetHorizontalDistanceToPlayer()
        {
            var playerPos = GameManager.Instance.Player.transform.position;
            return Vector3.Distance(new Vector3(playerPos.x, 0, playerPos.z), new Vector3(transform.position.x, 0, transform.position.z));
        }
    }
}