using System;
using Definitions.Data;
using Definitions.Enum;
using UnityEngine;
using UnityEngine.InputSystem;
using GameManager = Managers.GameManager;

namespace Player.Controller
{
    /// <summary>
    /// Cyber（プレイヤー）の挙動を制御する派生クラス
    /// PlayerControllerBaseを継承し、Cyber固有の処理を実装する
    /// </summary>
    public class CyberController : PlayerControllerBase
    {
        public PlayerBaseStats test;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Start() // テスト用
        {
            Initialize(test);
        }

        public override void InitializeState()
        {
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Idle, OnIdleEnter);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Move, OnMoveEnter, OnMoveUpdate, OnMoveExit);
        }
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void Update()
        {
            StateHandler.CurrentState.OnUpdate?.Invoke();
        }

        //-------------------------------------------------------------------------------
        // 入力のコールバックイベント
        //-------------------------------------------------------------------------------
        
        public override void OnMoveInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Move); // 移動状態に遷移する
                MovementHandler.SetMoveDirection(context.ReadValue<Vector2>()); // 移動方向を設定する
            }
            else if (context.canceled) // 入力終了時
            {
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle); // 待機状態に遷移する
            }
        }
        
        public override void OnDashInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                
            }
        }

        public override void OnRollInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                
            }
        }

        public override void OnParryInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                
            }
        }

        public override void OnGuardInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                
            }
        }

        public override void OnAttackNInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                
            }
        }

        public override void OnAttackSInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                
            }
        }

        public override void OnAttackEInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                
            }
        }
        
        //-------------------------------------------------------------------------------
        // 待機状態のアクション
        //-------------------------------------------------------------------------------

        private void OnIdleEnter()
        {
            AnimationHandler.PlayIdleAnimation(); // 待機アニメーションを再生する
        }
        
        //-------------------------------------------------------------------------------
        // 移動状態のアクション
        //-------------------------------------------------------------------------------

        private void OnMoveEnter()
        {
            AnimationHandler.PlayMoveAnimation(); // 移動アニメーションを再生する
        }

        private void OnMoveUpdate()
        {
            MovementHandler.RotateTowardsCameraRelativeDir(GameManager.Instance.MainCamera.transform);
        }

        private void OnMoveExit()
        {
            AnimationHandler.CancelMoveAnimation(); // 移動アニメーションを中止する
        }
        
        //-------------------------------------------------------------------------------
        // ダッシュ状態のアクション
        //-------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------
        // 回避状態のアクション
        //-------------------------------------------------------------------------------
    }
}