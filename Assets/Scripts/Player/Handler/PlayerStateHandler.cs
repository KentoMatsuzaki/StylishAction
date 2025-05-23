using Enum.Player;
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
        
        /// <summary>着地状態</summary>
        public PlayerState TransitionState { get; private set; }
        
        /// <summary>パリィ状態</summary>
        public PlayerState ParryState { get; private set; }
        
        /// <summary>防御状態</summary>
        public PlayerState GuardState { get; private set; }
        
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
            IdleState = new PlayerState(PlayerEnum.EPlayerState.Idle);
            
            // 移動状態
            MoveState = new PlayerState(PlayerEnum.EPlayerState.Move);
            
            // スプリント状態
            SprintState = new PlayerState(PlayerEnum.EPlayerState.Sprint);
            
            // 回避状態
            DodgeState = new PlayerState(PlayerEnum.EPlayerState.Dodge);
            
            // 通常攻撃状態
            AttackNormalState = new PlayerState(PlayerEnum.EPlayerState.AttackNormal);
            
            // 特殊攻撃状態
            AttackSpecialState = new PlayerState(PlayerEnum.EPlayerState.AttackSpecial);
            
            // 必殺攻撃状態
            AttackExtraState = new PlayerState(PlayerEnum.EPlayerState.AttackExtra);
            
            // 遷移状態（他の状態へ遷移するための中継的な状態）
            TransitionState = new PlayerState(PlayerEnum.EPlayerState.Transition);
            
            // パリィ状態
            ParryState = new PlayerState(PlayerEnum.EPlayerState.Parry);
            
            // 防御状態
            GuardState = new PlayerState(PlayerEnum.EPlayerState.Guard);
        }

        /// <summary>各状態ごとに遷移できない状態を定義する</summary>
        private void DefineInvalidTransitions()
        {
            // 移動状態
            MoveState.InvalidTransitions.Add(PlayerEnum.EPlayerState.AttackNormal);
            MoveState.InvalidTransitions.Add(PlayerEnum.EPlayerState.AttackSpecial);
            MoveState.InvalidTransitions.Add(PlayerEnum.EPlayerState.AttackExtra);
            MoveState.InvalidTransitions.Add(PlayerEnum.EPlayerState.Guard);
            
            // スプリント状態
            SprintState.InvalidTransitions.Add(PlayerEnum.EPlayerState.AttackNormal);
            SprintState.InvalidTransitions.Add(PlayerEnum.EPlayerState.AttackSpecial);
            SprintState.InvalidTransitions.Add(PlayerEnum.EPlayerState.AttackExtra);
            SprintState.InvalidTransitions.Add(PlayerEnum.EPlayerState.Guard);
            
            // 通常攻撃状態
            AttackNormalState.InvalidTransitions.Add(PlayerEnum.EPlayerState.Move);
            AttackNormalState.InvalidTransitions.Add(PlayerEnum.EPlayerState.Sprint);
            
            // 特殊攻撃状態
            AttackSpecialState.InvalidTransitions.Add(PlayerEnum.EPlayerState.Move);
            AttackSpecialState.InvalidTransitions.Add(PlayerEnum.EPlayerState.Sprint);
            
            // 必殺攻撃状態
            AttackExtraState.InvalidTransitions.Add(PlayerEnum.EPlayerState.Move);
            AttackExtraState.InvalidTransitions.Add(PlayerEnum.EPlayerState.Sprint);
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
            
            // 次の状態へ現在の状態から遷移できない場合は処理を抜ける
            if (CurrentState.InvalidTransitions.Contains(nextState.StateType))
            {
                Debug.Log("Invalid Transition");
                return;
            }
            
            // 現在の状態の終了時に呼ばれる処理を呼び出す
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }
            
            Debug.Log($"{CurrentState.StateType} to {nextState.StateType} {Time.time}");
            
            CurrentState = nextState;
            CurrentState.Enter();
        }

        /// <summary>移動入力を受け付けるかどうか</summary>
        public bool CanAcceptMoveInput()
        {
            return CurrentState == IdleState || CurrentState == MoveState || 
                   CurrentState == SprintState || CurrentState == AttackExtraState;
        }
    }
}