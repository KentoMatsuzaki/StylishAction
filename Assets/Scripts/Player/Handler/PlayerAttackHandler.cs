using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;
using Enemy.AI;
using Enum;

namespace Player.Handler
{
    /// <summary>プレイヤーの攻撃を制御するクラス</summary>
    public class PlayerAttackHandler : MonoBehaviour
    {
        [Header("攻撃の発生クラス")] 
        [SerializeField] private PlayerAttackInvoker rightHandInvoker;
        [SerializeField] private PlayerAttackInvoker leftHandInvoker;
        [SerializeField] private PlayerAttackInvoker extraInvoker;

        /// <summary>攻撃の発生クラスの辞書</summary>
        private readonly Dictionary<PlayerEnums.AttackType, PlayerAttackInvoker> _invokerDic = new();
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _invokerDic.Add(PlayerEnums.AttackType.RightSword, rightHandInvoker);
            _invokerDic.Add(PlayerEnums.AttackType.LeftSword, leftHandInvoker);
            _invokerDic.Add(PlayerEnums.AttackType.ExtraAttack, extraInvoker);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃の種類を指定して、発生クラスを取得する</summary>
        private PlayerAttackInvoker GetInvoker(PlayerEnums.AttackType attackType)
        {
            return _invokerDic.GetValueOrDefault(attackType);
        }
        
        /// <summary>攻撃の当たり判定を有効化する</summary>
        public void EnableAttackCollider(PlayerEnums.AttackType attackType)
        {
            GetInvoker(attackType).EnableCollider();
        }

        /// <summary>攻撃の当たり判定を無効化する</summary>
        public void DisableAttackCollider(PlayerEnums.AttackType attackType)
        {
            GetInvoker(attackType).DisableCollider();
        }

        /// <summary>攻撃の当たり判定を持続時間だけ有効化する</summary>
        public void EnableAttackColliderForDuration(PlayerEnums.AttackType attackType, float attackDuration)
        {
            GetInvoker(attackType).EnableCollider();
            StartCoroutine(DisableAttackColliderRoutine(attackType, attackDuration));
        }
        
        /// <summary>攻撃の持続時間後に、攻撃の当たり判定を無効化するコルーチン</summary>
        private IEnumerator DisableAttackColliderRoutine(PlayerEnums.AttackType attackType, float attackDuration)
        {
            yield return new WaitForSeconds(attackDuration);
            GetInvoker(attackType).DisableCollider();
        }

        /// <summary>Y座標を無視して敵への方向を求める</summary>
        private Vector3 GetFlatDirectionToEnemy(EnemyAIBase enemy)
        {
            return (new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z) - 
                    new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        }
        
        /// <summary>敵の方向へ回転させる</summary>
        public void RotateTowardsEnemy(EnemyAIBase enemy, float rotateSpeed)
        {
            var rotation = Quaternion.LookRotation(GetFlatDirectionToEnemy(enemy));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
    }
}
