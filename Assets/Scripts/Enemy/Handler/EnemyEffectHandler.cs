using Effect;
using Enum.Enemy;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>敵のエフェクトを制御するクラス</summary>
    public class EnemyEffectHandler : MonoBehaviour
    {
        /// <summary>攻撃エフェクトを有効化する</summary>
        public void PlayAttackEffect(string skillName)
        {
            if (System.Enum.TryParse(skillName, out EnemyEnum.EnemySkillType type))
            {
                EffectManager.Instance.PlayEnemyAttackEffect(type);
            }
            else
            {
                Debug.LogWarning($"Invalid Skill Name : {skillName}");
            }
        }
    }
}