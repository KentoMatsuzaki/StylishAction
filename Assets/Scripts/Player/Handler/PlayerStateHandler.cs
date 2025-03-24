using Enum.Player;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーの状態を制御するクラス</summary>
    public class PlayerStateHandler : MonoBehaviour
    {
        [Header("デバッグモード"), SerializeField] private bool isDebug;
        
        /// <summary>現在の状態</summary>
        private PlayerEnum.PlayerState _currentState;

        private void Update()
        {
            if (isDebug) Debug.Log($"Current State : {_currentState}");
        }

        /// <summary>現在の状態を設定する</summary>
        public void SetCurrentState(PlayerEnum.PlayerState state)
        {
            _currentState = state;
        }

        /// <summary>現在の状態を取得する</summary>
        public PlayerEnum.PlayerState GetCurrentState()
        {
            return _currentState;
        }
        
        /// <summary>移動可能かどうか</summary>
        public bool CanMove()
        {
            return _currentState is PlayerEnum.PlayerState.Idle or PlayerEnum.PlayerState.Move;
        }

        /// <summary>攻撃可能かどうか</summary>
        public bool CanAttack()
        {
            return _currentState is PlayerEnum.PlayerState.Idle or PlayerEnum.PlayerState.Move;
        }
        
        //-------------------------------------------------------------------------------
        // アニメーションイベント
        //-------------------------------------------------------------------------------

        /// <summary>Idleアニメーションから呼ばれる</summary>
        public void SetStateIdle()
        {
            _currentState = PlayerEnum.PlayerState.Idle;
        }

        /// <summary>Parryアニメーションから呼ばれる</summary>
        public void SetStateParry()
        {
            _currentState = PlayerEnum.PlayerState.Parry;
        }
    }
}
