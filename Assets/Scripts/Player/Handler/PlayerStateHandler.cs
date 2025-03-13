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
        public void SetState(PlayerEnum.PlayerState state)
        {
            _currentState = state;
        }

        /// <summary>現在の状態を取得する</summary>
        public PlayerEnum.PlayerState GetState()
        {
            return _currentState;
        }
    }
}
