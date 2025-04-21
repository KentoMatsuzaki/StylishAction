using Enum;
using UnityEngine;

namespace SO.Enemy
{
    /// <summary>敵のスキル情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "EnemySkillData", menuName = "ScriptableObjects/CreateEnemySkillAsset")]
    public class EnemySkillData : ScriptableObject
    {
        /// <summary>スキルの種類</summary>
        public InGameEnum.EnemyAttackType type;

        /// <summary>スキルが与えるダメージ量</summary>
        public float damageAmount;

        /// <summary>スキルの最小射程</summary>
        public float minAttackRange;

        /// <summary>スキルの最大射程</summary>
        public float maxAttackRange;

        /// <summary>スキルの最大角度</summary>
        public float maxAttackAngle;
    }
}