using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy.AsyncNode;
using Enemy.Handler;
using Player;
using SO.Enemy;
using UnityEngine;

namespace Enemy.AI
{
    /// <summary>敵AIの基底クラス</summary>
    public abstract class EnemyAIBase : MonoBehaviour
    {
        /// <summary>プレイヤーの制御クラス</summary>
        protected PlayerController Player;
        /// <summary>ステータス情報</summary>
        protected EnemyStatusData StatusData;
        /// <summary>現在の体力</summary>
        protected float CurrentHp;
        /// <summary>現在の攻撃ヒット数</summary>
        protected int CurrentHitCount;
        /// <summary>BTのキャンセルトークン</summary>
        protected CancellationTokenSource Cts;
        /// <summary>BTのルートノード</summary>
        protected BaseAsyncNode MainNode;
        /// <summary>アニメーション制御クラス</summary>
        protected EnemyAnimationHandler AnimationHandler;
        /// <summary>剛体</summary>
        protected Rigidbody Rigidbody;

        private void Awake()
        {
            AnimationHandler = GetComponent<EnemyAnimationHandler>();
            Rigidbody = GetComponent<Rigidbody>();
        }
        
        /// <summary>ビヘイビアツリーを開始する</summary>
        public abstract void BeginBehaviourTree();

        /// <summary>ビヘイビアツリーを実行する</summary>
        protected abstract UniTask ExecuteBehaviourTree();

        /// <summary>ビヘイビアツリーを構築する</summary>
        public abstract void ConstructBehaviourTree();

        /// <summary>攻撃がプレイヤーに命中した際の処理</summary>
        public abstract void OnHitPlayer(PlayerController player);

        /// <summary>プレイヤーの攻撃が命中した際の処理</summary>
        public abstract void OnHitByPlayer(float damage, Vector3 hitPosition);

        /// <summary>初期化する</summary>
        public void Initialize(PlayerController player, EnemyStatusData data)
        {
            StatusData = data;
            Player = player;
            CurrentHp = data.maxHp;
        }
    }
}