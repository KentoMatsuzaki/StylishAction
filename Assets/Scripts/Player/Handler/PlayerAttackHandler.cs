using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enemy.AI;
using Enum;
using SO.Player;

namespace Player.Handler
{
    /// <summary>プレイヤーの攻撃を制御するクラス</summary>
    public class PlayerAttackHandler : MonoBehaviour
    {
        /// <summary>攻撃情報のリスト</summary>
        [SerializeField] private List<PlayerAttackStats> attackStatsList;

        /// <summary>攻撃情報のマップ</summary>
        private Dictionary<PlayerEnums.AttackType, PlayerAttackStats> _attackStatsMap;

        [SerializeField] private PlayerAttackInvoker attackExtraInvoker; 
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            // 攻撃情報をマッピングする
            _attackStatsMap = attackStatsList.ToDictionary(k => k.attackType, v => v);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃の種類を指定して攻撃情報を取得する</summary>
        public PlayerAttackStats GetAttackStats(PlayerEnums.AttackType attackType)
        {
            return _attackStatsMap[attackType];
        }

        /// <summary>Y座標を無視して敵の方向へ即座に回転させる</summary>
        public void RotateTowardsEnemyInstantly(EnemyAIBase enemy)
        {
            var enemyPos = enemy.transform.position;
            enemyPos.y = transform.position.y;
            transform.LookAt(enemyPos);
        }

        public void EnableAttackExtraCollider() => attackExtraInvoker.EnableCollider();
        public void DisableAttackExtraCollider() => attackExtraInvoker.DisableCollider();
    }
}
