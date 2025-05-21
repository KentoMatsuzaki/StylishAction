using UnityEngine;

namespace Player.Handler
{
    /// <summary>モデルのアニメーションイベントを制御するクラス</summary>
    public class PlayerModelEventHandler : MonoBehaviour
    {
        private PlayerStateHandler _stateHandler;
        private PlayerLocomotionHandler _locomotionHandler;

        private void Awake()
        {
            _stateHandler = GetComponentInParent<PlayerStateHandler>();
            _locomotionHandler = GetComponentInParent<PlayerLocomotionHandler>();
        }

        /// <summary>静止状態に切り替える</summary>
        public void SwitchStateToIdle()
        {
            _stateHandler.SwitchState(_stateHandler.IdleState);
        }
    }
}