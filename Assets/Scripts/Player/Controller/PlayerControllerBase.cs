using Definitions.Data;
using Player.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    /// <summary>
    /// プレイヤーの挙動を制御する基底クラス
    /// 共通の制御ロジックを提供し、各種プレイヤーコントローラーが継承して実装する。
    /// </summary>
    public abstract class PlayerControllerBase : MonoBehaviour
    {
        protected IPlayerStateHandler StateHandler;　       // 状態ハンドラー
        protected IPlayerAttackHandler AttackHandler;       // 攻撃ハンドラー
        protected IPlayerMovementHandler MovementHandler;   // 移動ハンドラー
        protected IPlayerAnimationHandler AnimationHandler; // アニメーション

        protected ReactiveProperty<float> CurrentHp = new(); // 現在のHP
        public IReadOnlyReactiveProperty<float> PlayerHp => CurrentHp;
        protected ReactiveProperty<float> CurrentSp = new(); // 現在のSP
        public IReadOnlyReactiveProperty<float> PlayerSp => CurrentSp;
        protected ReactiveProperty<float> CurrentEp = new(); // 現在のEP
        public IReadOnlyReactiveProperty<float> PlayerEp => CurrentEp;
        
        public float MaxHp { get; protected set; } // 最大HP
        public float MaxSp { get; protected set; } // 最大SP
        public float MaxEp { get; protected set; } // 最大EP
        
        protected PlayerBaseStats BaseStats; // 基本パラメーター
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>プレイヤーを初期化する</summary>
        public void Initialize(PlayerBaseStats baseStats)
        {
            InitializeComponent();
            InitializeStats(baseStats);
            InitializeState();
        }

        /// <summary>コンポーネントを初期化する</summary>
        private void InitializeComponent()
        {
            StateHandler = GetComponent<IPlayerStateHandler>();
            AttackHandler = GetComponent<IPlayerAttackHandler>();
            MovementHandler = GetComponent<IPlayerMovementHandler>();
            AnimationHandler = GetComponent<IPlayerAnimationHandler>();
        }

        /// <summary>パラメーターを初期化する</summary>
        private void InitializeStats(PlayerBaseStats baseStats)
        {
            // 基本パラメーターを初期化する
            BaseStats = baseStats;
            
            // 各種パラメーターの初期値を初期化する
            CurrentHp.Value = BaseStats.maxHp;
            CurrentSp.Value = BaseStats.maxSp;
            CurrentEp.Value = 0f;
            
            // 各種パラメーターの最大値を初期化する
            MaxHp = BaseStats.maxHp;
            MaxSp = BaseStats.maxSp;
            MaxEp = BaseStats.maxEp;
        }

        /// <summary>状態を初期化する</summary>
        public abstract void InitializeState();
        
        //-------------------------------------------------------------------------------
        // 入力のコールバックイベント
        //-------------------------------------------------------------------------------

        public abstract void OnMoveInput(InputAction.CallbackContext context); 
        public abstract void OnDashInput(InputAction.CallbackContext context); 
        public abstract void OnRollInput(InputAction.CallbackContext context); 
        public abstract void OnParryInput(InputAction.CallbackContext context);
        public abstract void OnGuardInput(InputAction.CallbackContext context);
        public abstract void OnAttackNInput(InputAction.CallbackContext context);
        public abstract void OnAttackSInput(InputAction.CallbackContext context);
        public abstract void OnAttackEInput(InputAction.CallbackContext context);
    }
}