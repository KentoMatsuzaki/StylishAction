using UnityEngine;
using Const;
using Player;

namespace Enemy.Handler
{
    /// <summary>敵の攻撃クラス</summary>
    public class EnemyAttacker : MonoBehaviour
    {
        /// <summary>敵の制御クラス</summary>
        private EnemyController _controller;
        
        /// <summary>攻撃コライダー</summary>
        private Collider _collider;

        private void Start()
        {
            _controller = GetComponentInParent<EnemyController>();
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(InGameConst.PlayerTag))
            {
                var player = other.GetComponent<PlayerController>();
                _controller.Bt.OnHitPlayer(player);
            }
        }
        
        /// <summary>攻撃コライダーを有効にする</summary>
        public void EnableAttackCollider() => _collider.enabled = true;
        
        /// <summary>攻撃コライダーを無効にする</summary>
        public void DisableAttackCollider() => _collider.enabled = false;
    }
}
