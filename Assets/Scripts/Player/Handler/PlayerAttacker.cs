using Const;
using Enemy;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーの攻撃クラス</summary>
    public class PlayerAttacker : MonoBehaviour
    {
        [Header("ダメージ量"), SerializeField] private float damageAmount;
        
        /// <summary>攻撃コライダー</summary>
        private Collider _attackCollider;

        private void Awake()
        {
            _attackCollider = GetComponent<Collider>();
            _attackCollider.enabled = false;
            _attackCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(InGameConst.EnemyTag))
            {
                var enemy = other.GetComponent<EnemyController>();
                var hitPosition = other.ClosestPoint(transform.position);
                // 敵の被ダメージ処理を呼ぶ
                enemy.CurrentBehaviourTree.OnHitByPlayer(damageAmount, hitPosition);
            }
        }
        
        /// <summary>攻撃コライダーを有効化する</summary>
        public void EnableAttackCollider() => _attackCollider.enabled = true;
        
        /// <summary>攻撃コライダーを無効化する</summary>
        public void DisableAttackCollider() => _attackCollider.enabled = false;
    }
}
