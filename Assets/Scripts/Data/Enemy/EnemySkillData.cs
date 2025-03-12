using UnityEngine;

namespace Data.Enemy
{
    /// <summary>敵のスキル情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "EnemySkillData", menuName = "ScriptableObjects/CreateEnemySkillAsset")]
    public class EnemySkillData : ScriptableObject
    {
        /// <summary>スキルの固有番号</summary>
        public int skillNumber;

        /// <summary>スキルが与えるダメージ量</summary>
        public float damageAmount;

        /// <summary>スキルの攻撃範囲</summary>
        public float attackRange;

        /// <summary>スキルの攻撃角度</summary>
        public float attackAngle;
    }
}