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
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            CreateAllStates();
            Initialize();
        }

        /// <summary>全ての状態を作成する</summary>
        private void CreateAllStates()
        {
            IdleState = new PlayerState(PlayerEnum.EPlayerState.Idle);
            MoveState = new PlayerState(PlayerEnum.EPlayerState.Move);
            SprintState = new PlayerState(PlayerEnum.EPlayerState.Sprint);
            DodgeState = new PlayerState(PlayerEnum.EPlayerState.Dodge);
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
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }
            
            if (nextState== null)
            {
                Debug.LogWarning("New state not found");
                return;
            }

            
            
            CurrentState = nextState;
            CurrentState.Enter();
            
            Debug.Log($"{CurrentState.State} {Time.time}");
        }
    }
}