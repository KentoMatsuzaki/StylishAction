using Cysharp.Threading.Tasks;
using Player;
using SO.Enemy;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Enemy.AsyncNode;
using Enemy.Handler;
using SO.Player;
using UnityEngine.Serialization;

namespace Enemy.AI
{
    /// <summary>敵のAI挙動を制御する抽象クラス</summary>
    public abstract class EnemyAIBase : MonoBehaviour
    {
        /// <summary>プレイヤー</summary>
        [SerializeField] public PlayerController player;
        
        /// <summary>ステータス情報</summary>
        [SerializeField] protected EnemyStats stats;

        /// <summary>攻撃情報のリスト</summary>
        [SerializeField] protected List<EnemyAttackStats> attackStatsList;

        /// <summary>現在の攻撃情報</summary>
        protected EnemyAttackStats CurrentAttackStats;
        
        /// <summary>ビヘイビアツリーの最上位ノード</summary>
        protected AsyncSelectorNode RootNode;

        /// <summary>ビヘイビアツリーのキャンセルトークン</summary>
        protected CancellationTokenSource Cts;
        
        /// <summary>アニメーションの制御クラス</summary>
        protected EnemyAnimationHandler AnimationHandler;

        /// <summary>攻撃の制御クラス</summary>
        protected EnemyAttackHandler AttackHandler;

        /// <summary>現在の体力値</summary>
        protected float CurrentHp;

        /// <summary>現在の靭性値</summary>
        protected int CurrentPoise;

        /// <summary>剛体</summary>
        protected Rigidbody Rb;

        //-------------------------------------------------------------------------------
        // ビヘイビアツリーに関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>ビヘイビアツリーを構築する</summary>
        protected abstract void BuildTree();

        /// <summary>ビヘイビアツリーの評価を実行する</summary>
        protected abstract UniTask Tick();
        
        /// <summary>ビヘイビアツリーの評価を開始する</summary>
        protected abstract void StartBehaviour();
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃の結果を適用する</summary>
        public abstract void ApplyAttack(PlayerController playerController, Vector3 hitPosition);
        
        /// <summary>パリィの結果を適用する</summary>
        public abstract void ApplyParry();

        /// <summary>ダメージを適用する</summary>
        public abstract void ApplyDamage(PlayerAttackStats attackStats, Vector3 hitPosition);
    }
}