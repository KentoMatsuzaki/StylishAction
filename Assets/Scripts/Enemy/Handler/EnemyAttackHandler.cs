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

        /// <summary>攻撃の発生クラスの辞書</summary>
        private readonly Dictionary<EnemyEnum.AttackType, EnemyAttackInvoker> _invokerDic = new();
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _invokerDic.Add(EnemyEnum.AttackType.Scythe, scythe);
            _invokerDic.Add(EnemyEnum.AttackType.Meteor, meteor);
            _invokerDic.Add(EnemyEnum.AttackType.Seraphic, seraphic);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃の種類を指定して、発生クラスを取得する</summary>
        private EnemyAttackInvoker GetInvoker(EnemyEnum.AttackType attackType)
        {
            return _invokerDic.GetValueOrDefault(attackType);
        }
        
        /// <summary>攻撃の当たり判定を有効化する</summary>
        public void EnableAttackCollider(EnemyEnum.AttackType attackType)
        {
            GetInvoker(attackType).EnableCollider();
        }

        /// <summary>攻撃の当たり判定を無効化する</summary>
        public void DisableAttackCollider(EnemyEnum.AttackType attackType)
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
            var distance = GetFlatDistanceToPlayer(player);
            return attackStats.minAttackRange <= distance && distance <= attackStats.maxAttackRange;
        }
        
        //-------------------------------------------------------------------------------
        // 方向に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>Y座標を無視してプレイヤーへの方向を求める</summary>
        private Vector3 GetFlatDirectionToPlayer(PlayerController player)
        {
            return (new Vector3(player.transform.position.x, 0, player.transform.position.z) - 
                    new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        }

        /// <summary>プレイヤーが攻撃の有効角度内にいるか</summary>
        private bool IsPlayerInAttackAngle(PlayerController player, EnemyAttackStats attackStats)
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

        /// <summary>Meteorの攻撃アニメーションから呼ばれる</summary>
        public void EnableMeteorCollider()
        {
            EnableAttackCollider(EnemyEnum.AttackType.Meteor);
        }

        /// <summary>Meteorの攻撃アニメーションから呼ばれる</summary>
        public void DisableMeteorCollider()
        {
            DisableAttackCollider(EnemyEnum.AttackType.Meteor);
        }
    }
}