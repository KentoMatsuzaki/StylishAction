using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Data;
using Enemy.AsyncNode;
using Enemy.Interface;
using UniRx;
using UnityEngine;

namespace Enemy.AI
{
    /// <summary>
    /// 敵のAI挙動を制御する基底クラス
    /// 共通の制御ロジックを提供し、各種敵AIクラスが継承して実装する
    /// </summary>
    public abstract class EnemyAIBase : MonoBehaviour
    {
        protected IEnemyAttackHandler AttackHandler;       // 攻撃ハンドラー
        protected IEnemyMovementHandler MovementHandler;   // 移動ハンドラー
        protected IEnemyParticleHandler ParticleHandler;   // パーティクルハンドラー
        protected IEnemyAnimationHandler AnimationHandler; // アニメーションハンドラー

        private readonly ReactiveProperty<float> _currentHp = new(); // 現在のHP
        public IReadOnlyReactiveProperty<float> EnemyHp => _currentHp;
        
        public float MaxHp { get; private set; } // 最大HP
        
        protected EnemyBaseStats BaseStats; // 基本パラメーター

        protected readonly AsyncSelectorNode RootNode = new(); // 最上位ノード
        protected CancellationTokenSource Cts; // キャンセルトークン
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>敵の初期化処理を行う</summary>
        /// <param name="baseStats">敵の基本パラメーター</param>
        public void Initialize(EnemyBaseStats baseStats)
        {
            InitializeComponent();
            InitializeStats(baseStats);
            BuildBehaviourTree();
            StartBehaviourTree();
        }

        private void InitializeComponent()
        {
            AttackHandler = GetComponent<IEnemyAttackHandler>();
            MovementHandler = GetComponent<IEnemyMovementHandler>();
            ParticleHandler = GetComponent<IEnemyParticleHandler>();
            AnimationHandler = GetComponent<IEnemyAnimationHandler>();
        }

        private void InitializeStats(EnemyBaseStats baseStats)
        {
            BaseStats = baseStats;
            _currentHp.Value = BaseStats.maxHp;
            MaxHp = BaseStats.maxHp;
        }
        
        //-------------------------------------------------------------------------------
        // BehaviourTreeに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>BehaviourTreeを構築する</summary>
        protected abstract void BuildBehaviourTree();

        /// <summary>BehaviourTreeを実行する</summary>
        private async UniTask ExecuteBehaviourTree()
        {
            try
            {
                while (!Cts.IsCancellationRequested)
                {
                    await RootNode.ExecuteAsync(Cts.Token);
                    await UniTask.Yield(PlayerLoopTiming.Update, Cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("BehaviourTree canceled.");
            }
        }

        /// <summary>BehaviourTreeの実行を開始する</summary>
        protected void StartBehaviourTree()
        {
            Cts?.Cancel();
            Cts = new CancellationTokenSource();
            AttackHandler.SetAttackStats();
            ExecuteBehaviourTree().Forget();
        }
        
        //-------------------------------------------------------------------------------
        // プレイヤーの攻撃が命中した際の処理
        //-------------------------------------------------------------------------------

        /// <summary>プレイヤーの攻撃を受けた際に呼ばれる処理</summary>
        /// <param name="damage">受けた攻撃のダメージ量</param>
        /// <param name="hitPosition">攻撃がヒットしたワールド座標</param>
        public abstract void OnHit(float damage, Vector3 hitPosition);

        /// <summary>ダメージの数値だけHPを減少させ、適切なメソッドを呼び出す</summary>
        protected void ApplyDamage(float damage)
        {
            _currentHp.Value = Mathf.Max(0, _currentHp.Value - damage);
            
            if (_currentHp.Value > 0)
            {
                OnDamage();
            }
            else
            {
                OnDie();
            }
        }

        /// <summary>被弾時の処理</summary>
        protected abstract void OnDamage();

        /// <summary>死亡時の処理</summary>
        protected abstract void OnDie();

        /// <summary>パリィされた時の処理</summary>
        public abstract void OnParried();
    }
}