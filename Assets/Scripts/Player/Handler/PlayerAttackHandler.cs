using UnityEngine;
using System.Collections.Generic;

namespace Player.Handler
{
    /// <summary>プレイヤーの攻撃を制御するクラス</summary>
    public class PlayerAttackHandler : MonoBehaviour
    {
        [Header("攻撃クラスのリスト"), SerializeField] private List<PlayerAttacker> attackerList = new();

        /// <summary>攻撃コライダーを有効化する</summary>
        /// <param name="attackNumber">1から始まる、有効化する攻撃の番号</param>
        public void EnableAttackCollider(int attackNumber)
        {
            // 攻撃の番号が正常でない場合は処理を抜ける
            if (!IsValidAttackNumber(attackNumber)) return;
            // 攻撃コライダーを有効化する
            attackerList[attackNumber - 1].EnableAttackCollider();
        }

        /// <summary>攻撃コライダーを無効化する</summary>
        /// <param name="attackNumber">1から始まる、無効化する攻撃の番号</param>
        public void DisableAttackCollider(int attackNumber)
        {
            // 攻撃の番号が正常出ない場合は処理を抜ける
            if (!IsValidAttackNumber(attackNumber)) return;
            // 攻撃コライダーを無効化する
            attackerList[attackNumber - 1].DisableAttackCollider();
        }

        /// <summary>攻撃の番号が正常であるか判定する</summary>
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
