using UnityEngine;
using Const;
using Player;

namespace Enemy.Handler
{
    /// <summary>敵の攻撃を制御するクラス</summary>
    public class EnemyAttackHandler : MonoBehaviour
    {
        /// <summary>敵の制御クラス</summary>
        private EnemyController _controller;
        
        /// <summary>攻撃の当たり判定</summary>
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
            if (other.CompareTag(InGameConst.PlayerTagName))
            {
                var player = other.GetComponent<PlayerController>();
                _controller.CurrentBehaviourTree.OnHitPlayer();
            }
        }
        
        /// <summary>攻撃の当たり判定を有効にする</summary>
        public void EnableAttackCollider() => _collider.enabled = true;
        
        /// <summary>攻撃の当たり判定を無効にする</summary>
        public void DisableAttackCollider() => _collider.enabled = false;
    }
}
