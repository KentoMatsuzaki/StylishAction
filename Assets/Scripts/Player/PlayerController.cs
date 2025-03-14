using Enum;
using Enum.Player;
using Player.Handler;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>プレイヤーを制御するクラス</summary>
    public class PlayerController : MonoBehaviour
    {
        private PlayerMoveHandler _moveHandler;
        private PlayerStateHandler _stateHandler;
        private PlayerAnimationHandler _animationHandler;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Start()
        {
            _moveHandler = GetComponent<PlayerMoveHandler>();
            _stateHandler = GetComponent<PlayerStateHandler>();
            _animationHandler = GetComponent<PlayerAnimationHandler>();
        }
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void FixedUpdate()
        {
            if (_stateHandler.GetState() == PlayerEnum.PlayerState.Move)
            {
                _moveHandler.MoveForward(2.5f);
            }
        }

        //-------------------------------------------------------------------------------
        // ダッシュのコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            // ボタンを押した瞬間の処理
            if (context.performed)
            {
                // 状態処理
                _stateHandler.SetState(PlayerEnum.PlayerState.Move);
                
                // 回転処理
                _moveHandler.RotateTowardsMovement(context.ReadValue<Vector2>());
                
                // アニメーション処理
                _animationHandler.SetMoveFlag(true);
            }
            // ボタンを離した瞬間の処理
            else if (context.canceled)
            {
                // 状態処理
                _stateHandler.SetState(PlayerEnum.PlayerState.Idle);
                // アニメーション処理
                _animationHandler.SetMoveFlag(false);
            }
        }
        
        //-------------------------------------------------------------------------------
        // ダッシュのコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnDash(InputAction.CallbackContext context)
        {
            // ボタンを押した瞬間の処理
            if (context.performed)
            {
                // 移動処理
                _moveHandler.DashForward(5f);
                // アニメーション処理
                _animationHandler.TriggerDash();
            }
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnAttack(InputAction.CallbackContext context)
        {
            // ボタンを押した瞬間の処理
            if (context.performed)
            {
                var actionName = context.action.name;
                
                switch (actionName)
                {
                    case "Attack 1":
                        _animationHandler.TriggerAttack(1); // アニメーション処理
                        break;
                    case "Attack 2":
                        _animationHandler.TriggerAttack(2); // アニメーション処理
                        break;
                    case "Attack 3":
                        _animationHandler.TriggerAttack(3); // アニメーション処理
                        break;
                }
            }
        }
        
        //-------------------------------------------------------------------------------
        // パリィのコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnParry(InputAction.CallbackContext context)
        {
            // ボタンを押した瞬間の処理
            if (context.performed)
            {
                // アニメーション処理
                _animationHandler.TriggerParry();
            }
        }
        
    }
}