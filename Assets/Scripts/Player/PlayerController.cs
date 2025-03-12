using System;
using Player.Handler;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>プレイヤーを制御するクラス</summary>
    public class PlayerController : MonoBehaviour
    {
        private PlayerAnimationHandler _animationHandler;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Start()
        {
            _animationHandler = GetComponent<PlayerAnimationHandler>();
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
                // 移動の入力値を受け取る
                var direction = context.ReadValue<Vector2>();
                // アニメーション処理
                _animationHandler.SetMoveFlag(true);
            }
            // ボタンを離した瞬間の処理
            else if (context.canceled)
            {
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