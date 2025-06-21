using Definitions.Enum;
using UnityEngine;
using UnityEngine.Serialization;

namespace Definitions.Data
{
    /// <summary>
    /// 敵の攻撃パラメーター（攻撃の種類,ダメージ量,射程など）を保持するスクリプタブルオブジェクト
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyAttackStats", menuName = "ScriptableObjects/Enemy/AttackStats", order = 1)]
    public class EnemyAttackStats : ScriptableObject
    {
        /// <summary>攻撃の種類</summary>
        public InGameEnums.EnemyAttackType attackType;

        /// <summary>ダメージ量</summary>
        public float damage;

        /// <summary>最小有効射程</summary>
        public float minRange;

        /// <summary>最大有効射程</summary>
        public float maxRange;

        /// <summary>最大有効角度</summary>
        public float maxAngle;

        /// <summary>クールタイム</summary>
        public float cooldown;
    }
}