using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.Handler
{
    /// <summary>敵の攻撃を制御するクラス</summary>
    public class EnemyAttackHandler : MonoBehaviour
    {
        [Header("敵の攻撃クラスのリスト"), SerializeField] private List<EnemyAttacker> attackerList = new();

        /// <summary>攻撃コライダーを有効化する</summary>
        /// <param name="skillNumber">対応するスキルの番号</param>
        public void EnableAttackCollider(int skillNumber)
        {
            // スキルの番号が正しくない場合は処理を抜ける
            if (!IsValidSkillNumber(skillNumber)) return;
            // 攻撃コライダーを有効化する
            attackerList[skillNumber - 1].EnableAttackCollider();
        }

        /// <summary>攻撃コライダーを無効化する</summary>
        /// <param name="skillNumber">対応するスキルの番号</param>
        public void DisableAttackCollider(int skillNumber)
        {
            // スキルの番号が正しくない場合は処理を抜ける
            if (!IsValidSkillNumber(skillNumber)) return;
            // 攻撃コライダーを無効化する
            attackerList[skillNumber - 1].DisableAttackCollider();
        }

        /// <summary>スキルの番号が正常であるか</summary>
        private bool IsValidSkillNumber(int skillNumber)
        {
            if (skillNumber < 1 || skillNumber > attackerList.Count)
            {
                Debug.LogWarning($"Invalid Skill Number : {skillNumber}"); return false;
            }
            return true;
        }
    }
}