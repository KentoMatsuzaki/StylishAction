using Definitions.Const;
using Definitions.Data;
using Definitions.Enum;
using Enemy.AI;
using Player.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    /// <summary>
    /// プレイヤーの挙動を制御する基底クラス
    /// 共通の制御ロジックを提供し、各種プレイヤーコントローラーが継承して実装する
    /// </summary>
    public abstract class PlayerControllerBase : MonoBehaviour
    {
        protected IPlayerStateHandler StateHandler;　       // 状態ハンドラー
        protected IPlayerAttackHandler AttackHandler;       // 攻撃ハンドラー
        protected IPlayerMovementHandler MovementHandler;   // 移動ハンドラー
        protected IPlayerParticleHandler ParticleHandler;   // パーティクルハンドラー
        protected IPlayerAnimationHandler AnimationHandler; // アニメーションハンドラー

        private readonly ReactiveProperty<float> _currentHp = new(); // 現在のHP
        public IReadOnlyReactiveProperty<float> PlayerHp => _currentHp;
        
        protected readonly ReactiveProperty<float> RollCoolDown = new(); // 回避のクールタイム
        public IReadOnlyReactiveProperty<float> PlayerRollCoolDown => RollCoolDown;
        
        protected readonly ReactiveProperty<float> ParryCoolDown = new(); // パリィのクールタイム
        public IReadOnlyReactiveProperty<float> PlayerParryCoolDown => ParryCoolDown;
        
        protected readonly ReactiveProperty<float> GuardCoolDown = new(); // 防御のクールタイム
        public IReadOnlyReactiveProperty<float> PlayerGuardCoolDown => GuardCoolDown;
        
        protected readonly ReactiveProperty<float> AtkSCoolDown = new(); // 特殊攻撃のクールタイム
        public IReadOnlyReactiveProperty<float> PlayerAtkSCoolDown => AtkSCoolDown;
        
        protected readonly ReactiveProperty<float> AtkECoolDown = new(); // EX攻撃のクールタイム
        public IReadOnlyReactiveProperty<float> PlayerAtkECoolDown => AtkECoolDown;
        
        protected readonly ReactiveProperty<float> AtkEDuration = new(); // EX攻撃の持続時間
        public IReadOnlyReactiveProperty<float> PlayerAtkEDuration => AtkEDuration;
        
        public float MaxHp { get; private set; } // 最大HP
        
        protected PlayerBaseStats BaseStats; // 基本パラメーター

        public bool isLockOnEnemy;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>プレイヤーの初期化処理を行う</summary>
        /// <param name="baseStats">プレイヤーの基本パラメーター</param>
        public virtual void Initialize(PlayerBaseStats baseStats)
        {
            InitializeComponent();
            InitializeStats(baseStats);
            InitializeState();
            
            // クールタイムを設定する
            AtkECoolDown.Value = InGameConsts.PlayerAtkECoolDown;
            // クールタイムを購読する
            Observable.EveryUpdate()
                .TakeWhile(_ => AtkECoolDown.Value > 0)
                .Subscribe(_ => AtkECoolDown.Value -= Time.deltaTime, () => AtkECoolDown.Value = 0);
        }

        /// <summary>コンポーネントを初期化する</summary>
        private void InitializeComponent()
        {
            StateHandler = GetComponent<IPlayerStateHandler>();
            AttackHandler = GetComponent<IPlayerAttackHandler>();
            MovementHandler = GetComponent<IPlayerMovementHandler>();
            ParticleHandler = GetComponent<IPlayerParticleHandler>();
            AnimationHandler = GetComponent<IPlayerAnimationHandler>();
        }

        /// <summary>パラメーターを初期化する</summary>
        private void InitializeStats(PlayerBaseStats baseStats)
        {
            // 基本パラメーターを初期化する
            BaseStats = baseStats;
            
            // 体力の初期値を初期化する
            _currentHp.Value = BaseStats.maxHp;
            
            // 体力の最大値を初期化する
            MaxHp = BaseStats.maxHp;
        }

        /// <summary>状態を初期化する</summary>
        protected abstract void InitializeState();
        
        //-------------------------------------------------------------------------------
        // 入力のコールバックイベント
        //-------------------------------------------------------------------------------

        public abstract void OnMoveInput(InputAction.CallbackContext context); 
        public abstract void OnRollInput(InputAction.CallbackContext context); 
        public abstract void OnParryInput(InputAction.CallbackContext context);
        public abstract void OnGuardInput(InputAction.CallbackContext context);
        public abstract void OnAttackNInput(InputAction.CallbackContext context);
        public abstract void OnAttackSInput(InputAction.CallbackContext context);
        public abstract void OnAttackEInput(InputAction.CallbackContext context);
        
        //-------------------------------------------------------------------------------
        // 敵の攻撃が命中した際の処理
        //-------------------------------------------------------------------------------

        /// <summary>敵の攻撃を受けた際に呼ばれる処理</summary>
        /// <param name="enemy">攻撃を実行した敵のAI制御クラス</param>
        /// <param name="damage">攻撃のダメージ</param>
        /// <param name="hitPosition">攻撃がヒットしたワールド座標</param>
        public abstract void OnHit(EnemyAIBase enemy, float damage, Vector3 hitPosition);

        /// <summary>ダメージの数値だけHPを減少させ、適切な状態遷移を行う</summary>
        protected void ApplyDamage(float damage)
        {
            _currentHp.Value = Mathf.Max(0f, _currentHp.Value - damage);
            StateHandler.ChangeState(_currentHp.Value > 0 ? InGameEnums.PlayerStateType.Damage : InGameEnums.PlayerStateType.Dead);
        }
    }
}