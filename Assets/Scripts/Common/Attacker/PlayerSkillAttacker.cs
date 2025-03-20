using Const;
using Enemy;
using SO.Player;
using UnityEngine;

namespace Common.Attacker
{
    /// <summary>スキルによるプレイヤーの攻撃の派生クラス</summary>
    public class PlayerSkillAttacker : SkillAttackerBase
    {
        /// <summary>攻撃のデータ</summary>
        [SerializeField] private PlayerAttackData data;
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(InGameConst.EnemyTag))
            {
                // 敵の制御クラスを取得する
                var enemyController = other.GetComponent<EnemyController>();
                // 攻撃の命中座標を取得する
                var attackHitPosition = other.ClosestPoint(transform.position);
                // 敵の被ダメージ処理を呼ぶ
                enemyController.Bt.OnHit(data.damageAmount, attackHitPosition);
            }
        }
    }
}