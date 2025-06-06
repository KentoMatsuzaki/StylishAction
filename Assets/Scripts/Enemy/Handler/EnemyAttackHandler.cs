using System.Collections.Generic;
using Const;
using Enum;
using Player;
using SO.Enemy;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>敵の攻撃を制御するクラス</summary>
    public class EnemyAttackHandler : MonoBehaviour
    {
        [Header("攻撃の発生クラス")] 
        [SerializeField] private EnemyAttackInvoker scythe;
        [SerializeField] private EnemyAttackInvoker meteor;
        [SerializeField] private EnemyAttackInvoker seraphic;
        [SerializeField] private EnemyAttackInvoker eclipse;
        [SerializeField] private EnemyAttackInvoker explosion;
        [SerializeField] private EnemyAttackInvoker vortex;

        [Header("WaterFallの攻撃発生クラス")] 
        [SerializeField] private List<EnemyAttackInvoker> waterFallInvokers;
        
        [Header("Photonの攻撃発生クラス")]
        [SerializeField] private List<EnemyAttackInvoker> photonInvokers;

        /// <summary>攻撃の発生クラスの辞書</summary>
        private readonly Dictionary<EnemyEnums.AttackType, EnemyAttackInvoker> _invokerDic = new();
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _invokerDic.Add(EnemyEnums.AttackType.Scythe, scythe);
            _invokerDic.Add(EnemyEnums.AttackType.Meteor, meteor);
            _invokerDic.Add(EnemyEnums.AttackType.Seraph, seraphic);
            _invokerDic.Add(EnemyEnums.AttackType.Eclipse, eclipse);
            _invokerDic.Add(EnemyEnums.AttackType.Explosion, explosion);
            _invokerDic.Add(EnemyEnums.AttackType.Vortex, vortex);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃の種類を指定して、発生クラスを取得する</summary>
        private EnemyAttackInvoker GetInvoker(EnemyEnums.AttackType attackType)
        {
            return _invokerDic.GetValueOrDefault(attackType);
        }
        
        /// <summary>攻撃の当たり判定を有効化する</summary>
        private void EnableAttackCollider(EnemyEnums.AttackType attackType)
        {
            GetInvoker(attackType).EnableCollider();
        }

        /// <summary>攻撃の当たり判定を無効化する</summary>
        private void DisableAttackCollider(EnemyEnums.AttackType attackType)
        {
            GetInvoker(attackType).DisableCollider();
        }

        /// <summary>プレイヤーを攻撃可能か</summary>
        public bool CanAttackPlayer(PlayerController player, EnemyAttackStats attackStats)
        {
            return IsPlayerInAttackRange(player, attackStats) && IsPlayerInAttackAngle(player, attackStats);
        }
        
        //-------------------------------------------------------------------------------
        // 距離に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>Y座標を無視してプレイヤーへの距離を求める</summary>
        private float GetFlatDistanceToPlayer(PlayerController player)
        {
            return Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), 
                new Vector3(transform.position.x, 0, transform.position.z));
        }
        
        /// <summary>プレイヤーが攻撃の有効射程内にいるか</summary>
        private bool IsPlayerInAttackRange(PlayerController player, EnemyAttackStats attackStats)
        {
            return !IsPlayerTooClose(player, attackStats) && !IsPlayerTooFar(player, attackStats);
        }

        /// <summary>プレイヤーが最小有効射程よりも近くにいるか</summary>
        public bool IsPlayerTooClose(PlayerController player, EnemyAttackStats attackStats)
        {
            var distance = GetFlatDistanceToPlayer(player);
            return attackStats.minAttackRange > distance;
        }

        /// <summary>プレイヤーが最大有効射程よりも遠くにいるか</summary>
        public bool IsPlayerTooFar(PlayerController player, EnemyAttackStats attackStats)
        {
            var distance = GetFlatDistanceToPlayer(player);
            return distance > attackStats.maxAttackRange;
        }
        
        //-------------------------------------------------------------------------------
        // 方向に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>Y座標を無視してプレイヤーへの方向を求める</summary>
        public Vector3 GetFlatDirectionToPlayer(PlayerController player)
        {
            return (new Vector3(player.transform.position.x, 0, player.transform.position.z) - 
                    new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        }

        /// <summary>プレイヤーが攻撃の有効角度内にいるか</summary>
        public bool IsPlayerInAttackAngle(PlayerController player, EnemyAttackStats attackStats)
        {
            var angle = Vector3.Angle(transform.forward, GetFlatDirectionToPlayer(player));
            return angle <= attackStats.attackAngle;
        }
        
        //-------------------------------------------------------------------------------
        // 移動に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>プレイヤーの方向へ移動させる</summary>
        public void MoveTowardsPlayer(PlayerController player, float moveSpeed)
        {
            transform.Translate(Time.deltaTime * GetFlatDirectionToPlayer(player) * moveSpeed, Space.World);
        }

        /// <summary>プレイヤーの逆方向へ移動させる</summary>
        public void MoveAwayFromPlayer(PlayerController player, float moveSpeed)
        {
            transform.Translate(Time.deltaTime * GetFlatDirectionToPlayer(player) * -moveSpeed, Space.World);
        }
        
        //-------------------------------------------------------------------------------
        // 回転に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>プレイヤーの方向へ回転させる</summary>
        public void RotateTowardsPlayer(PlayerController player, float rotateSpeed)
        {
            var rotation = Quaternion.LookRotation(GetFlatDirectionToPlayer(player));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
        
        //-------------------------------------------------------------------------------
        // アニメーションイベント
        //-------------------------------------------------------------------------------

        public void EnableScytheCollider()
        {
            EnableAttackCollider(EnemyEnums.AttackType.Scythe);
        }

        public void DisableScytheCollider()
        {
            DisableAttackCollider(EnemyEnums.AttackType.Scythe);
        }
        
        public void HandleMeteorCollider()
        {
            EnableAttackCollider(EnemyEnums.AttackType.Meteor);
            Invoke(nameof(DisableMeteorCollider), 0.1f);
        }
        
        public void DisableMeteorCollider()
        {
            DisableAttackCollider(EnemyEnums.AttackType.Meteor);
        }

        public void EnableSeraphCollider()
        {
            EnableAttackCollider(EnemyEnums.AttackType.Seraph);
        }

        public void DisableSeraphCollider()
        {
            DisableAttackCollider(EnemyEnums.AttackType.Seraph);
        }

        public void EnableEclipseCollider()
        {
            EnableAttackCollider(EnemyEnums.AttackType.Eclipse);
        }

        public void DisableEclipseCollider()
        {
            DisableAttackCollider(EnemyEnums.AttackType.Eclipse);
        }

        public void EnableExplosionCollider()
        {
            EnableAttackCollider(EnemyEnums.AttackType.Explosion);
        }

        public void DisableExplosionCollider()
        {
            DisableAttackCollider(EnemyEnums.AttackType.Explosion);
        }

        public void EnableWaterFallColliders()
        {
            foreach (var waterFallInvoker in waterFallInvokers)
            {
                waterFallInvoker.EnableCollider();
            }
        }

        public void DisableWaterFallColliders()
        {
            foreach (var waterFallInvoker in waterFallInvokers)
            {
                waterFallInvoker.DisableCollider();
            }
        }

        public void EnableVortexCollider()
        {
            EnableAttackCollider(EnemyEnums.AttackType.Vortex);
        }

        public void DisableVortexCollider()
        {
            DisableAttackCollider(EnemyEnums.AttackType.Vortex);
        }

        public void HandlePhotonCollider()
        {
            foreach (var photonInvoker in photonInvokers)
            {
                photonInvoker.EnableCollider();
            }
            
            Invoke(nameof(DisablePhotonColliders), 1.25f);
        }

        public void DisablePhotonColliders()
        {
            foreach (var photonInvoker in photonInvokers)
            {
                photonInvoker.DisableCollider();
            }
        }
    }
}