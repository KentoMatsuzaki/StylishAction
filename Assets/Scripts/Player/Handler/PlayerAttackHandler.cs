using UnityEngine;
using System.Collections.Generic;
using Common.Attacker;
using UnityEngine.Serialization;

namespace Player.Handler
{
    /// <summary>プレイヤーの攻撃を制御するクラス</summary>
    public class PlayerAttackHandler : MonoBehaviour
    {
        [Header("攻撃クラスのリスト"), SerializeField] 
        private List<PlayerAttacker> attackerList;

        /// <summary>コライダーを有効化する</summary>
        /// <param name="attackNumber">攻撃の番号(1~)</param>
        public void EnableCollider(int attackNumber)
        {
            // 攻撃の番号が正常でない場合は処理を抜ける
            if (!IsValidAttackNumber(attackNumber)) return;
            // 攻撃コライダーを有効化する
            attackerList[attackNumber - 1].EnableCollider();
        }

        /// <summary>コライダーを無効化する</summary>
        /// <param name="attackNumber">攻撃の番号(1~)</param>
        public void DisableCollider(int attackNumber)
        {
            // 攻撃の番号が正常出ない場合は処理を抜ける
            if (!IsValidAttackNumber(attackNumber)) return;
            // 攻撃コライダーを無効化する
            attackerList[attackNumber - 1].DisableCollider();
        }

        /// <summary>正しい攻撃の番号であるか</summary>
        private bool IsValidAttackNumber(int attackNumber)
        {
            if (attackNumber < 1 || attackNumber > attackerList.Count)
            {
                Debug.LogWarning($"Invalid Attack Number : {attackNumber}"); return false;
            }
            return true;
        }
    }
}
