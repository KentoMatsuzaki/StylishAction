using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Definitions.Data;
using Definitions.Enum;
using Enemy.Attack;
using Enemy.Interface;
using Managers;
using UnityEngine;
using GameManager = Managers.GameManager;
using Random = UnityEngine.Random;

namespace Enemy.Handler
{
    /// <summary>Death（敵）の攻撃を制御するクラス</summary>
    public class DeathAttackHandler : MonoBehaviour, IEnemyAttackHandler
    {
        public EnemyAttackStats CurrentAttackStats { get; private set; }

        [SerializeField] private List<EnemyAttackStats> attackStatsList = new();
        
        [SerializeField] private List<EnemyAttacker> attackerList = new();

        private static Dictionary<InGameEnums.EnemyAttackType, EnemyAttacker> _attackerMap = new();
        
        [SerializeField] private List<EnemyAttacker> waterfallAttackers = new();
        
        [SerializeField] private List<EnemyAttacker> photonAttackers = new();
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _attackerMap = attackerList.ToDictionary(k => k.attackStats.attackType, v => v);
        }
        
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
        
        //-------------------------------------------------------------------------------
        // アニメーションイベント
        //-------------------------------------------------------------------------------

        public void EnableScytheCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Scythe].EnableCollider();
        }

        public void DisableScytheCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Scythe].DisableCollider();
        }

        public async void HandleMeteorCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Meteor].EnableCollider();
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            _attackerMap[InGameEnums.EnemyAttackType.Meteor].DisableCollider();
        }

        public void EnableSeraphCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Seraph].EnableCollider();
        }

        public void DisableSeraphCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Seraph].DisableCollider();
        }

        public void EnableEclipseCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Eclipse].EnableCollider();
        }
        
        public void DisableEclipseCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Eclipse].DisableCollider();
        }

        public void EnableVortexCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Vortex].EnableCollider();
        }

        public void DisableVortexCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Vortex].DisableCollider();
        }

        public async void HandlePhotonCollider()
        {
            foreach (var photonAttacker in photonAttackers)
            {
                photonAttacker.EnableCollider();
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(1.25f));
            
            foreach (var photonAttacker in photonAttackers)
            {
                photonAttacker.DisableCollider();
            }
        }
        
        public void EnableExplosionCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Explosion].EnableCollider();
        }
        
        public void DisableExplosionCollider()
        {
            _attackerMap[InGameEnums.EnemyAttackType.Explosion].DisableCollider();
        }

        public void EnableWaterfallCollider()
        {
            foreach (var waterfallAttacker in waterfallAttackers)
            {
                waterfallAttacker.EnableCollider();
            }
        }

        public void DisableWaterfallCollider()
        {
            foreach (var waterfallAttacker in waterfallAttackers)
            {
                waterfallAttacker.DisableCollider();
            }
        }

        public void PlayScytheSound()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Scythe);
        }

        public void PlayMeteorSound()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Meteor);
        }

        public void PlaySeraphSound()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Seraph);
        }

        public void PlayVortexSound()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Vortex);
        }

        public void PlayPhotonSound()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Photon);
        }

        public void PlayExplosionSound()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Explosion);
        }

        public void PlayWaterfallSound()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Waterfall);
        }
    }
}