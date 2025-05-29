using Const;
using Enemy.AI;
using UnityEngine;

namespace Player
{
    /// <summary>プレイヤーの攻撃を発生させるクラス</summary>
    public class PlayerAttackInvoker : MonoBehaviour
    {
        /// <summary>プレイヤーの制御クラス</summary>
        private PlayerController _player;
        
        /// <summary>攻撃の当たり判定</summary>
        private Collider _collider;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            // プレイヤーの制御クラスを取得する
            _player = GetComponentInParent<PlayerController>();
            // 攻撃の当たり判定を取得する
            _collider = GetComponent<Collider>();
            // 攻撃の当たり判定をトリガーにする
            _collider.isTrigger = true;
            // 攻撃の当たり判定を有効化する
            _collider.enabled = true;
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃の当たり判定による接触イベント</summary>
        private void OnTriggerEnter(Collider other)
        {
            // プレイヤーと接触した場合
            if (other.CompareTag(EnemyConst.GameObjectTag))
            {
                // 敵AIの制御クラスを取得する
                var enemy = other.GetComponent<EnemyAIBase>();
                // 敵AIのダメージ適用処理を呼び出す
                enemy.ApplyDamage(_player.CurrentAttackStats);
            }
        }
    }
}