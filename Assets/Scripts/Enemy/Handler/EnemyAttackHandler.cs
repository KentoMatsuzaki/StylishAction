using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.Handler
{
    /// <summary>敵の攻撃を制御するクラス</summary>
    public class EnemyAttackHandler : MonoBehaviour
    {
        [Header("鎌の攻撃クラス"), SerializeField] private EnemyAttacker scytheAttacker;
        
        /// <summary>鎌の攻撃コライダーを有効化する</summary>
        public void EnableScytheCollider()
        {
            scytheAttacker.EnableAttackCollider();
        }

        /// <summary>鎌の攻撃コライダーを無効化する</summary>
        public void DisableScytheCollider()
        {
            scytheAttacker.DisableAttackCollider();
        }
    }
}