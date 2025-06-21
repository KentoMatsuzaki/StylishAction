using Definitions.Data;
using Definitions.Enum;
using Enemy.AI;
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

        protected override void InitializeState()
        {
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Idle, OnIdleEnter);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Move, OnMoveEnter, OnMoveUpdate, OnMoveExit, OnMoveFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Dash, OnDashEnter, OnDashUpdate, null, OnDashFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Roll, OnRollEnter, null, OnRollExit, OnRollFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Parry, OnParryEnter, null, OnParryExit);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Guard, OnGuardEnter);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.AttackN, OnAttackNEnter, null, OnAttackNExit, OnAttackNFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.AttackS, OnAttackSEnter, null, OnAttackSExit, OnAttackSFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.AttackE, OnAttackEEnter, OnAttackEUpdate, null, OnAttackEFixedUpdate);
        }
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void Update()
        {
            // 現在の状態の更新処理（Update）を呼ぶ
            StateHandler.CurrentState.OnUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            // 現在の状態の更新処理（FixedUpdate）を呼ぶ
            StateHandler.CurrentState.OnFixedUpdate?.Invoke();
        }

        //-------------------------------------------------------------------------------
        // 移動入力のコールバックイベント
        //-------------------------------------------------------------------------------
        
        public override void OnMoveInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                MovementHandler.SetMoveDirection(context.ReadValue<Vector2>()); // 移動方向を設定する

                if (StateHandler.CurrentState.StateType != InGameEnums.PlayerStateType.Dash &&
                    StateHandler.CurrentState.StateType != InGameEnums.PlayerStateType.AttackE)
                {
                    StateHandler.ChangeState(InGameEnums.PlayerStateType.Move); // 移動状態に遷移する
                }
            }
            else if (context.canceled) // 入力終了時
            {
                MovementHandler.ResetMoveDirection(); // 移動方向を初期化する

                if (StateHandler.CurrentState.StateType != InGameEnums.PlayerStateType.AttackE)
                {
                    StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle); // 待機状態に遷移する
                }
            }
        }
        
        //-------------------------------------------------------------------------------
        // ダッシュ入力のコールバックイベント
        //-------------------------------------------------------------------------------
        
        public override void OnDashInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                if (MovementHandler.IsMoving()) // 移動入力がある場合
                {
                    StateHandler.ChangeState(InGameEnums.PlayerStateType.Dash);
                }
            }
            else if (context.canceled) // 入力終了時
            {
                StateHandler.ChangeState(MovementHandler.IsMoving() ? 
                    InGameEnums.PlayerStateType.Move : InGameEnums.PlayerStateType.Idle);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 回避入力のコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnRollInput(InputAction.CallbackContext context)
        {
            if (context.started) // 入力開始時
            {
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Roll);
            }
        }
        
        //-------------------------------------------------------------------------------
        // パリィ入力のコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnParryInput(InputAction.CallbackContext context)
        {
            if (context.started) // 入力開始時
            {
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Parry);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 防御入力のコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnGuardInput(InputAction.CallbackContext context)
        {
            if (context.started) // 入力開始時
            {
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Guard);
            }
            else if (context.canceled) // 入力終了時
            {
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 通常攻撃入力のコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnAttackNInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                if (!AnimationHandler.IsPlayingAtkNAnim)
                {
                    StateHandler.ChangeState(InGameEnums.PlayerStateType.AttackN);
                }
            }
        }
        
        //-------------------------------------------------------------------------------
        // 特殊攻撃入力のコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnAttackSInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                if (!AnimationHandler.IsPlayingAtkSAnim)
                {
                    StateHandler.ChangeState(InGameEnums.PlayerStateType.AttackS);
                }
            }
        }
        
        //-------------------------------------------------------------------------------
        // EX攻撃入力のコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnAttackEInput(InputAction.CallbackContext context)
        {
            if (context.performed) // 入力実行時
            {
                StateHandler.ChangeState(
                    StateHandler.CurrentState.StateType == InGameEnums.PlayerStateType.AttackE ? 
                        InGameEnums.PlayerStateType.Idle : InGameEnums.PlayerStateType.AttackE);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 待機状態のアクション
        //-------------------------------------------------------------------------------

        private void OnIdleEnter()
        {
            AnimationHandler.ResetAttackContext();
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
        
        private void OnMoveFixedUpdate()
        {
            MovementHandler.MoveForward(BaseStats.moveForce); // 正面方向に移動させる
        }
        
        //-------------------------------------------------------------------------------
        // ダッシュ状態のアクション
        //-------------------------------------------------------------------------------

        private void OnDashEnter()
        {
            AnimationHandler.PlayDashAnimation(); // ダッシュアニメーションを再生する
        }

        private void OnDashUpdate()
        {
            MovementHandler.RotateTowardsCameraRelativeDir(GameManager.Instance.MainCamera.transform);
        }

        private void OnDashFixedUpdate()
        {
            MovementHandler.MoveForward(BaseStats.dashForce); // 正面方向に移動させる
        }
        
        //-------------------------------------------------------------------------------
        // 回避状態のアクション
        //-------------------------------------------------------------------------------

        private void OnRollEnter()
        {
            AnimationHandler.PlayRollAnimation(
                () => StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle));
        }

        private void OnRollExit()
        {
            AnimationHandler.CancelRollAnimation();
        }

        private void OnRollFixedUpdate()
        {
            MovementHandler.ApplyRootMotion(AnimationHandler.DeltaPosition);
        }
        
        //-------------------------------------------------------------------------------
        // パリィ状態のアクション
        //-------------------------------------------------------------------------------

        private void OnParryEnter()
        {
            AnimationHandler.PlayParryAnimation(
                () => StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle));
        }

        private void OnParryExit()
        {
            AnimationHandler.CancelParryAnimation();
        }
        
        //-------------------------------------------------------------------------------
        // 防御状態のアクション
        //-------------------------------------------------------------------------------

        private void OnGuardEnter()
        {
            AnimationHandler.PlayGuardAnimation();
        }
        
        //-------------------------------------------------------------------------------
        // 通常攻撃のアクション
        //-------------------------------------------------------------------------------

        private void OnAttackNEnter()
        {
            AnimationHandler.PlayAttackNAnimation(
                () => StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle));
        }

        private void OnAttackNExit()
        {
            AnimationHandler.CancelAttackNAnimation();
        }

        private void OnAttackNFixedUpdate()
        {
            MovementHandler.ApplyRootMotion(AnimationHandler.DeltaPosition);
        }
        
        //-------------------------------------------------------------------------------
        // 特殊攻撃のアクション
        //-------------------------------------------------------------------------------

        private void OnAttackSEnter()
        {
            AnimationHandler.PlayAttackSAnimation(
                () => StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle));
        }

        private void OnAttackSExit()
        {
            AnimationHandler.CancelAttackSAnimation();
        }

        private void OnAttackSFixedUpdate()
        {
            MovementHandler.ApplyRootMotion(AnimationHandler.DeltaPosition);
        }
        
        //-------------------------------------------------------------------------------
        // EX攻撃のアクション
        //-------------------------------------------------------------------------------

        private void OnAttackEEnter()
        {
            AnimationHandler.PlayAttackEAnimation();
        }

        private void OnAttackEUpdate()
        {
            MovementHandler.RotateTowardsCameraRelativeDir(GameManager.Instance.MainCamera.transform);
        }

        private void OnAttackEFixedUpdate()
        {
            MovementHandler.MoveForward(BaseStats.atkEForce);
        }
        
        //-------------------------------------------------------------------------------
        // 被弾状態のアクション
        //-------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------
        // 死亡状態のアクション
        //-------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------
        // 被弾時の処理
        //-------------------------------------------------------------------------------

        public override void OnHit(EnemyAIBase enemy, EnemyAttackStats attackStats, Vector3 hitPosition)
        {
            // 無敵状態
            if (StateHandler.IsInvincible())
            {
                return;
            }
            
            // 防御状態
            if (StateHandler.CurrentState.StateType == InGameEnums.PlayerStateType.Guard)
            {
                // エフェクトを表示する
                // サウンドを再生する
                // ヒットアニメーションを再生する
            }
            
            // パリィ状態
            if (StateHandler.CurrentState.StateType == InGameEnums.PlayerStateType.Parry)
            {
                // エフェクトを表示する
                // サウンドを再生する
                // 被パリィ処理を呼ぶ
            }

            // その他の状態
            else
            {
                // エフェクトを表示する
                // ダメージを適用する
                ApplyDamage(attackStats.damage);
            }
        }
    }
}