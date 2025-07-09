using Common;
using Definitions.Const;
using Definitions.Data;
using Enemy.AI;
using Managers;
using Player.Controller;
using UnityEngine;

namespace Enemy.Attack
{
    /// <summary>
    /// 敵の攻撃判定を制御する派生クラス
    /// AttackerBaseを継承し、敵固有の処理を実装する
    /// </summary>
    public class EnemyAttacker : AttackerBase
    {
        private EnemyAIBase _ai; // AI制御クラス
        [SerializeField] public EnemyAttackStats attackStats; // 攻撃のパラメーター
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();
            _ai = GetComponentInParent<EnemyAIBase>();
            Collider.enabled = false;
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃判定に関する処理
        //-------------------------------------------------------------------------------

        protected override void OnTriggerEnter(Collider other)
        {
            // プレイヤーのゲームオブジェクトと接触した場合
            if (other.CompareTag(InGameConsts.PlayerGameObjectTag))
            {
                var player = other.gameObject.GetComponent<PlayerControllerBase>(); // プレイヤーの制御クラス
                var hitPos = other.ClosestPoint(transform.position); // 攻撃が命中したワールド座標
                var damage = Random.Range(attackStats.damage * 0.75f, attackStats.damage); // 攻撃のダメージ量
                player.OnHit(_ai, damage, hitPos); // 攻撃命中時の処理を呼ぶ
            }
        }
    }
}