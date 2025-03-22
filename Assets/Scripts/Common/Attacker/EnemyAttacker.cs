using Const;
using Enemy;
using Enum.Enemy;
using Player;
using UnityEngine;

namespace Common.Attacker
{
    /// <summary>敵の攻撃クラス</summary>
    public class EnemyAttacker : AttackerBase
    {
        /// <summary>攻撃の種類</summary>
        public EnemyEnum.EnemyAttackType type;
        
        /// <summary>敵の制御クラス</summary>
        private EnemyController _controller;
        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponentInParent<EnemyController>();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(InGameConst.PlayerTag))
            {
                // プレイヤーの制御クラスを取得する
                var playerController = other.GetComponent<PlayerController>();
                // プレイヤーの被ダメージ処理を呼ぶ
                _controller.CurrentBehaviourTree.OnHitPlayer(playerController);
            }
        }
    }
}