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
        protected IEnemyAttackHandler AttackHandler; // 攻撃ハンドラー
        protected IEnemyMovementHandler MovementHandler; // 移動ハンドラー
        protected IEnemyAnimationHandler AnimationHandler; // アニメーション

        protected ReactiveProperty<float> CurrentHp = new(); // 現在のHP
        public IReadOnlyReactiveProperty<float> EnemyHp => CurrentHp;
        
        public float MaxHp { get; private set; } // 最大HP
        
        protected EnemyBaseStats BaseStats; // 基本パラメーター

        protected AsyncSelectorNode RootNode = new(); // 最上位ノード
        private CancellationTokenSource _cts; // キャンセルトークン
        
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
            AnimationHandler = GetComponent<IEnemyAnimationHandler>();
        }

        private void InitializeStats(EnemyBaseStats baseStats)
        {
            BaseStats = baseStats;
            CurrentHp.Value = BaseStats.maxHp;
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
                while (!_cts.IsCancellationRequested)
                {
                    await RootNode.ExecuteAsync(_cts.Token);
                    await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("BehaviourTree canceled.");
            }
        }

        /// <summary>BehaviourTreeの実行を開始する</summary>
        private void StartBehaviourTree()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            AttackHandler.SetAttackStats();
            ExecuteBehaviourTree().Forget();
        }
    }
}