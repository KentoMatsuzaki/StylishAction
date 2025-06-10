using Enum;
using UnityEngine;
using UnityEngine.Serialization;

namespace SO.Enemy
{
    /// <summary>敵の攻撃情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "EnemyAttackStats", menuName = "ScriptableObjects/EnemyAttackStats")]
    public class EnemyAttackStats : ScriptableObject
    {
        /// <summary>攻撃の種類</summary>
        public EnemyEnums.AttackType attackType;

        /// <summary>ダメージ量</summary>
        public float attackDamage;

        /// <summary>最小有効射程</summary>
        public float minAttackRange;

        /// <summary>最大有効射程</summary>
        public float maxAttackRange;

        /// <summary>有効角度</summary>
        public float attackAngle;

        /// <summary>クールタイム</summary>
        public float attackCooldown;
    }
}