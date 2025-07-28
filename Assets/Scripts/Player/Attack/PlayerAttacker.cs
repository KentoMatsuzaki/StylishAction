using Common;
using Definitions.Const;
using Definitions.Data;
using Definitions.Enum;
using Enemy.AI;
using Managers;
using UnityEngine;

namespace Player.Attack
{
    /// <summary>
    /// プレイヤーの攻撃判定を制御する派生クラス
    /// AttackerBaseを継承し、プレイヤー固有の処理を実装する
    /// </summary>
    public class PlayerAttacker : AttackerBase
    {
        [SerializeField] private bool isColliderDisabled;
        [SerializeField] private PlayerAttackStats attackStats; // 攻撃のパラメーター
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();
            Collider.enabled = !isColliderDisabled;
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃判定に関する処理
        //-------------------------------------------------------------------------------

        protected override void OnTriggerEnter(Collider other)
        {
            // 敵のゲームオブジェクトと接触した場合
            if (other.CompareTag(InGameConsts.EnemyGameObjectTag))
            {
                var enemy = other.GetComponent<EnemyAIBase>(); // 敵AIの制御クラス
                var hitPos = other.ClosestPoint(transform.position); // 攻撃が命中したワールド座標
                var damage = Random.Range(attackStats.damage * 0.75f, attackStats.damage);
                enemy.OnHit(damage, hitPos); // 攻撃命中時の処理を呼ぶ
                ShowDamageUI(hitPos, damage); // ダメージUIを表示する
            }
        }

        private void ShowDamageUI(Vector3 hitPos, float damage)
        {
            switch (attackStats.attackType)
            {
                case InGameEnums.PlayerAttackType.Normal:
                    UIManager.Instance.ShowDamageUI(hitPos, damage, Color.yellow, Color.white);
                    break;
                
                case InGameEnums.PlayerAttackType.Special:
                    UIManager.Instance.ShowDamageUI(hitPos, damage, Color.cyan, Color.white);
                    break;
                
                case InGameEnums.PlayerAttackType.Extra:
                    UIManager.Instance.ShowDamageUI(hitPos, damage,
                        new Color(0.4f, 0.2f, 1.0f, 1.0f), new Color(0.85f, 0.8f, 1.0f, 1.0f));
                    break;
            }
        }
    }
}