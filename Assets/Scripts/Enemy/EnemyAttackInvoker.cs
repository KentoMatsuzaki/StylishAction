using Const;
using Enemy.AI;
using Player;
using UnityEngine;

namespace Enemy
{
    /// <summary>敵の攻撃を発生させるクラス</summary>
    public class EnemyAttackInvoker : MonoBehaviour
    {
        /// <summary>AI制御クラス</summary>
        private EnemyAIBase _ai;
        
        /// <summary>攻撃の当たり判定</summary>
        private Collider _collider;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            // AI制御クラスを取得する
            _ai = GetComponentInParent<EnemyAIBase>();
            // 攻撃の当たり判定を取得する
            _collider = GetComponent<Collider>();
            // 攻撃の当たり判定をトリガーにする
            _collider.isTrigger = true;
            // 攻撃の当たり判定を無効化する
            _collider.enabled = false;
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>攻撃の当たり判定を有効化する</summary>
        public void EnableCollider() => _collider.enabled = true;
        
        /// <summary>攻撃の当たり判定を無効化する</summary>
        public void DisableCollider() => _collider.enabled = false;

        /// <summary>攻撃の当たり判定による接触イベント</summary>
        private void OnTriggerEnter(Collider other)
        {
            // プレイヤーと接触した場合
            if (other.CompareTag(PlayerConst.GameObjectTag))
            {
                // プレイヤーの制御クラスを取得する
                var player = other.GetComponent<PlayerController>();
                // 攻撃の結果を適用する
                _ai.ApplyAttack();
            }
        }
    }
}