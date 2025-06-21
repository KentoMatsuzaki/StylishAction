using System.Collections.Generic;
using Definitions.Data;
using Enemy.Interface;
using Managers;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>Death（敵）の攻撃を制御するクラス</summary>
    public class DeathAttackHandler : MonoBehaviour, IEnemyAttackHandler
    {
        public EnemyAttackStats CurrentAttackStats { get; private set; }

        [SerializeField] private List<EnemyAttackStats> attackStatsList = new();
        
        //-------------------------------------------------------------------------------
        // 攻撃の制御に関する処理
        //-------------------------------------------------------------------------------

        public bool CanAttackPlayer()
        {
            return IsPlayerInRange() && IsPlayerInAngle();
        }

        /// <summary>プレイヤーが有効射程内にいるか</summary>
        private bool IsPlayerInRange()
        {
            return !IsPlayerTooClose() && !IsPlayerTooFar();
        }

        public bool IsPlayerTooClose()
        {
            return GetHorizontalDistanceToPlayer() < CurrentAttackStats.minRange;
        }

        public bool IsPlayerTooFar()
        {
            return GetHorizontalDistanceToPlayer() > CurrentAttackStats.maxRange;
        }

        /// <summary>プレイヤーが有効角度内にいるか</summary>
        public bool IsPlayerInAngle()
        {
            var angle = Vector3.Angle(transform.forward, GetHorizontalDirectionToPlayer());
            return angle <= CurrentAttackStats.maxAngle;
        }

        /// <summary>プレイヤーとの距離を求める（Y座標を無視する）</summary>
        private float GetHorizontalDistanceToPlayer()
        {
            var playerPos = GameManager.Instance.Player.transform.position;
            return Vector3.Distance(new Vector3(playerPos.x, 0, playerPos.z), new Vector3(transform.position.x, 0, transform.position.z));
        }

        /// <summary>プレイヤーへの方向を求める（Y座標を無視する）</summary>
        private Vector3 GetHorizontalDirectionToPlayer()
        {
            var playerPos = GameManager.Instance.Player.transform.position;
            return (new Vector3(playerPos.x, 0, playerPos.z) - new Vector3(transform.position.x, 0, transform.position.z));
        }

        public void SetAttackStats()
        {
            CurrentAttackStats = attackStatsList[Random.Range(0, attackStatsList.Count)];
        }
    }
}