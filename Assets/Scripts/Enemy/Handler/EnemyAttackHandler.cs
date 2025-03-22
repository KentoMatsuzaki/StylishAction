using Common.Attacker;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>敵の攻撃を制御するクラス</summary>
    public class EnemyAttackHandler : MonoBehaviour
    {
        [Header("武器による攻撃の派生クラス"), SerializeField] private EnemyWeaponAttacker weaponAttacker;
        
        /// <summary>コライダーを有効化する</summary>
        public void EnableCollider()
        {
            weaponAttacker.EnableCollider();
        }

        /// <summary>コライダーを無効化する</summary>
        public void DisableCollider()
        {
            weaponAttacker.DisableCollider();
        }
    }
}