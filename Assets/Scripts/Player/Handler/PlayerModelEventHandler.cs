using UnityEngine;

namespace Player.Handler
{
    /// <summary>モデルのアニメーションイベントを制御するクラス</summary>
    public class PlayerModelEventHandler : MonoBehaviour
    {
        private PlayerStateHandler _stateHandler;

        private void Awake()
        {
            _stateHandler = GetComponentInParent<PlayerStateHandler>();
        }
        
    }
}