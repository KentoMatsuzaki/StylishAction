using Const;
using Enemy;
using Player;
using UnityEngine;

namespace Common.Attacker
{
    /// <summary>武器による敵の攻撃の派生クラス</summary>
    public class EnemyWeaponAttacker : WeaponAttackerBase
    {
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
                // 制御クラスの攻撃命中処理を呼ぶ
                _controller.Bt.OnHitPlayer(playerController);
            }
        }
    }
}