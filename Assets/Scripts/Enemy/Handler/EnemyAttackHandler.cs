using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Attacker;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>敵の攻撃を制御するクラス</summary>
    public class EnemyAttackHandler : MonoBehaviour
    {
        [Header("攻撃クラスのリスト"), SerializeField] private List<EnemyAttacker> attackerList;
        
        /// <summary>コライダーを有効化する</summary>
        /// <param name="attackName">攻撃の種類</param>
        public void EnableCollider(string attackName)
        {
            attackerList.FirstOrDefault(a => a.type.ToString() == attackName)?.EnableCollider();
        }

        /// <summary>コライダーを無効化する</summary>
        /// <param name="attackName">攻撃の種類</param>
        public void DisableCollider(string attackName)
        {
            attackerList.FirstOrDefault(a => a.type.ToString() == attackName)?.DisableCollider();
        }
        
        //-------------------------------------------------------------------------------
        // アニメーションイベント
        //-------------------------------------------------------------------------------
        
        /// <summary>Meteorのアニメーションイベントから呼ばれる</summary>
        public void EnableMeteorAndDisableMeteorColliderWithDelay()
        {
            StartCoroutine(EnableAndDisableMeteorColliderRoutine());
        }
        
        private IEnumerator EnableAndDisableMeteorColliderRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            EnableCollider("Meteor");
            yield return new WaitForSeconds(0.1f);
            DisableCollider("Meteor");
        }
    }
}