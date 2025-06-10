using Enum;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーの状態を制御するクラス</summary>
    public class PlayerStateHandler : MonoBehaviour
    {
        /// <summary>現在のプレイヤーの状態</summary>
        public PlayerState CurrentState { get; set; }

        /// <summary>静止状態</summary>
        public PlayerState IdleState { get; private set; }

        /// <summary>移動状態</summary>
        public PlayerState MoveState { get; private set; }

        /// <summary>スプリント状態</summary>
        public PlayerState SprintState { get; private set; }
        
        /// <summary>回避状態</summary>
        public PlayerState DodgeState { get; private set; }
        
        /// <summary>通常攻撃状態</summary>
        public PlayerState AttackNormalState { get; private set; }
        
        /// <summary>特殊攻撃状態</summary>
        public PlayerState AttackSpecialState { get; private set; }
        
        /// <summary>EX攻撃状態</summary>
        public PlayerState AttackExtraState { get; private set; }
        
        /// <summary>遷移状態</summary>
        public PlayerState TransitionState { get; private set; }
        
        /// <summary>パリィ状態</summary>
        public PlayerState ParryState { get; private set; }
        
        /// <summary>防御状態</summary>
        public PlayerState GuardState { get; private set; }
        
        /// <summary>被弾状態</summary>
        public PlayerState DamageState { get; private set; }
        
        /// <summary>死亡状態</summary>
        public PlayerState DeathState { get; private set; }
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            CreateAllStates();
            DefineInvalidTransitions();
            Initialize();
        }

        /// <summary>全ての状態を作成する</summary>
        private void CreateAllStates()
        {
            // 静止状態
            IdleState = new PlayerState(PlayerEnums.PlayerState.Idle);
            
            // 移動状態
            MoveState = new PlayerState(PlayerEnums.PlayerState.Move);
            
            // スプリント状態
            SprintState = new PlayerState(PlayerEnums.PlayerState.Sprint);
            
            // 回避状態
            DodgeState = new PlayerState(PlayerEnums.PlayerState.Dodge);
            
            // 通常攻撃状態
            AttackNormalState = new PlayerState(PlayerEnums.PlayerState.AttackNormal);
            
            // 特殊攻撃状態
            AttackSpecialState = new PlayerState(PlayerEnums.PlayerState.AttackSpecial);
            
            // 必殺攻撃状態
            AttackExtraState = new PlayerState(PlayerEnums.PlayerState.AttackExtra);
            
            // 遷移状態（他の状態へ遷移するための中継的な状態）
            TransitionState = new PlayerState(PlayerEnums.PlayerState.Transition);
            
            // パリィ状態
            ParryState = new PlayerState(PlayerEnums.PlayerState.Parry);
            
            // 防御状態
            GuardState = new PlayerState(PlayerEnums.PlayerState.Guard);
            
            // 被弾状態
            DamageState = new PlayerState(PlayerEnums.PlayerState.Damage);
            
            // 死亡状態
            DeathState = new PlayerState(PlayerEnums.PlayerState.Death);
        }

        /// <summary>各状態ごとに遷移できない状態を定義する</summary>
        private void DefineInvalidTransitions()
        {
            // 移動状態
            MoveState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            MoveState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            MoveState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            MoveState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            
            // スプリント状態
            SprintState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            SprintState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            SprintState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            SprintState.InvalidTransitions.Add(PlayerEnums.PlayerState.Parry);
            SprintState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            
            // 回避状態
            DodgeState.InvalidTransitions.Add(PlayerEnums.PlayerState.Move);
            DodgeState.InvalidTransitions.Add(PlayerEnums.PlayerState.Sprint);
            DodgeState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            DodgeState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            DodgeState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            DodgeState.InvalidTransitions.Add(PlayerEnums.PlayerState.Parry);
            DodgeState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            
            // 通常攻撃状態
            AttackNormalState.InvalidTransitions.Add(PlayerEnums.PlayerState.Move);
            AttackNormalState.InvalidTransitions.Add(PlayerEnums.PlayerState.Sprint);
            AttackNormalState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            AttackNormalState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            AttackNormalState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            
            // 特殊攻撃状態
            AttackSpecialState.InvalidTransitions.Add(PlayerEnums.PlayerState.Move);
            AttackSpecialState.InvalidTransitions.Add(PlayerEnums.PlayerState.Sprint);
            AttackSpecialState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            AttackSpecialState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            AttackSpecialState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            
            // 必殺攻撃状態
            AttackExtraState.InvalidTransitions.Add(PlayerEnums.PlayerState.Move);
            AttackExtraState.InvalidTransitions.Add(PlayerEnums.PlayerState.Sprint);
            AttackExtraState.InvalidTransitions.Add(PlayerEnums.PlayerState.Dodge);
            AttackExtraState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            AttackExtraState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            AttackExtraState.InvalidTransitions.Add(PlayerEnums.PlayerState.Parry);
            AttackExtraState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            
            // 遷移状態
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.Move);
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.Sprint);
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.Dodge);
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.Parry);
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.Damage);
            TransitionState.InvalidTransitions.Add(PlayerEnums.PlayerState.Death);
            
            // パリィ状態
            ParryState.InvalidTransitions.Add(PlayerEnums.PlayerState.Move);
            ParryState.InvalidTransitions.Add(PlayerEnums.PlayerState.Sprint);
            ParryState.InvalidTransitions.Add(PlayerEnums.PlayerState.Dodge);
            ParryState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            ParryState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            ParryState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            ParryState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            ParryState.InvalidTransitions.Add(PlayerEnums.PlayerState.Damage);
            ParryState.InvalidTransitions.Add(PlayerEnums.PlayerState.Death);
            
            // ガード状態
            GuardState.InvalidTransitions.Add(PlayerEnums.PlayerState.Move);
            GuardState.InvalidTransitions.Add(PlayerEnums.PlayerState.Sprint);
            GuardState.InvalidTransitions.Add(PlayerEnums.PlayerState.Dodge);
            GuardState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            GuardState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            GuardState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            GuardState.InvalidTransitions.Add(PlayerEnums.PlayerState.Parry);
            GuardState.InvalidTransitions.Add(PlayerEnums.PlayerState.Damage);
            
            // 被弾状態
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.Move);
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.Sprint);
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.Dodge);
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.Parry);
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.Damage);
            DamageState.InvalidTransitions.Add(PlayerEnums.PlayerState.Death);
            
            // 死亡状態
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.Idle);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.Move);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.Sprint);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.Dodge);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackNormal);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackSpecial);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.AttackExtra);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.Transition);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.Parry);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.Guard);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.Damage);
            DeathState.InvalidTransitions.Add(PlayerEnums.PlayerState.Death);
        }

        /// <summary>状態の初期化を行う</summary>
        private void Initialize()
        {
            CurrentState = IdleState;
            CurrentState.Enter();
        }
        
        //-------------------------------------------------------------------------------
        // 状態処理
        //-------------------------------------------------------------------------------

        /// <summary>状態を切り替える</summary>
        /// <param name="nextState">次の状態</param>
        public void SwitchState(PlayerState nextState)
        {
            // 次の状態が存在しない場合はログを出力して処理を抜ける
            if (nextState== null)
            {
                Debug.LogWarning("Next State Not Found");
                return;
            }
            
            // 現在の状態から次の状態へ遷移できない場合は処理を抜ける
            if (CurrentState.InvalidTransitions.Contains(nextState.StateType))
            {
                return;
            }
            
            // 現在の状態の終了時に呼ばれる処理を呼び出す
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }
            
            //Debug.Log($"{CurrentState.StateType} to {nextState.StateType} {Time.time}");
            
            CurrentState = nextState;
            CurrentState.Enter();
        }

        /// <summary>移動入力を受け付けるかどうか</summary>
        public bool CanAcceptMoveInput()
        {
            return CurrentState == IdleState || CurrentState == MoveState || CurrentState == SprintState || CurrentState == AttackExtraState;
        }

        /// <summary>ダメージを受け付けるかどうか</summary>
        public bool IsDamageReceivable()
        {
            return CurrentState != DamageState || CurrentState != TransitionState || CurrentState == AttackExtraState;
        }

        public void ResetState()
        {
            CurrentState = IdleState;
        }
    }
}