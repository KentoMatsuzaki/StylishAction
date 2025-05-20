using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーの状態を制御するクラス</summary>
    public class PlayerStateHandler : MonoBehaviour
    {
        /// <summary>現在のプレイヤーの状態</summary>
        private PlayerState _currentState;

        /// <summary>移動状態</summary>
        public PlayerState MoveState { get; private set; }

        /// <summary>スプリント状態</summary>
        public PlayerState SprintState { get; private set; }
        
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
            MoveState = new PlayerState();
            SprintState = new PlayerState();
        }

        /// <summary>状態の初期化を行う</summary>
        private void Initialize()
        {
            _currentState = MoveState;
            _currentState.Enter();
        }
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------
        
        /// <summary>現在の状態の更新処理を呼び出す</summary>
        public void ManualUpdate()
        {
            _currentState.Update();
        }
        
        //-------------------------------------------------------------------------------
        // 状態処理
        //-------------------------------------------------------------------------------

        /// <summary>状態を切り替える</summary>
        /// <param name="nextState">次の状態</param>
        public void SwitchState(PlayerState nextState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }
            
            if (nextState== null)
            {
                Debug.LogWarning("New state not found");
                return;
            }
            
            _currentState = nextState;
            _currentState.Enter();
        }
    }
}