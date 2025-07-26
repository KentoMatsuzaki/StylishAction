using System;
using Definitions.Const;
using Definitions.Enum;
using Enemy.AI;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using GameManager = Managers.GameManager;
using UniRx;
using Observable = UniRx.Observable;

namespace Player.Controller
{
    /// <summary>
    /// Cyber（プレイヤー）の挙動を制御する派生クラス
    /// PlayerControllerBaseを継承し、Cyber固有の処理を実装する
    /// </summary>
    public class CyberController : PlayerControllerBase
    {
        private IDisposable _rollDisposable;
        private IDisposable _parryDisposable;
        private IDisposable _guardDisposable;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        protected override void InitializeState()
        {
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Idle, OnIdleEnter);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Move, OnMoveEnter, OnMoveUpdate, OnMoveExit, OnMoveFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Roll, OnRollEnter, null, OnRollExit, OnRollFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Parry, OnParryEnter, null, OnParryExit);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Guard, OnGuardEnter, null, OnGuardExit);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.AttackN, OnAttackNEnter, null, OnAttackNExit, OnAttackNFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.AttackS, OnAttackSEnter, null, OnAttackSExit, OnAttackSFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.AttackE, OnAttackEEnter, OnAttackEUpdate, OnAttackEExit, OnAttackEFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Damage, OnDamageEnter, null, OnDamageExit, OnDamageFixedUpdate);
            StateHandler.SetStateAction(InGameEnums.PlayerStateType.Dead, OnDeadEnter);
        }
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void Update()
        {
            // 現在の状態の更新処理（Update）を呼ぶ
            StateHandler.CurrentState.OnUpdate?.Invoke();
            // 敵をロックオンしている場合は、敵の方向に回転させる
            if (isLockOnEnemy)
            {
                MovementHandler.RotateTowardsEnemy();
            }
        }

        private void FixedUpdate()
        {
            // 現在の状態の更新処理（FixedUpdate）を呼ぶ
            StateHandler.CurrentState.OnFixedUpdate?.Invoke();
        }

        //-------------------------------------------------------------------------------
        // 移動アクションが入力された時に呼ばれるコールバックイベント
        //-------------------------------------------------------------------------------
        
        public override void OnMoveInput(InputAction.CallbackContext context)
        {
            if (!StateHandler.CanHandleMoveInput()) return;
            
            if (context.performed) // 入力実行時
            {
                var moveInput = context.ReadValue<Vector2>();
                MovementHandler.SetMoveDirection(moveInput); // 移動方向を設定する
                AnimationHandler.SetMoveParameter(moveInput); // 移動パラメーターを設定する

                if (StateHandler.CurrentState.StateType != InGameEnums.PlayerStateType.AttackE)
                {
                    StateHandler.ChangeState(InGameEnums.PlayerStateType.Move); // 移動状態に遷移する
                }
            }
            else if (context.canceled) // 入力終了時
            {
                MovementHandler.ResetMoveDirection(); // 移動方向を初期化する
                AnimationHandler.ResetMoveParameter(); // 移動パラメーターを初期化する

                if (StateHandler.CurrentState.StateType != InGameEnums.PlayerStateType.AttackE)
                {
                    StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle); // 待機状態に遷移する
                }
            }
        }
        
        //-------------------------------------------------------------------------------
        // 回避アクションが入力された時に呼ばれるコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnRollInput(InputAction.CallbackContext context)
        {
            // 入力を開始した時の処理
            if (context.started) 
            {
                // クールタイム中は処理を抜ける
                if (RollCoolDown.Value > 0) return;
                // クールタイムを設定する
                RollCoolDown.Value = InGameConsts.PlayerRollCoolDown;
                // クールタイムの購読を破棄する
                _rollDisposable?.Dispose();
                // クールタイムを購読する
                _rollDisposable = Observable.EveryUpdate()
                    .TakeWhile(_ => RollCoolDown.Value > 0)
                    .Subscribe(_ => RollCoolDown.Value -= Time.deltaTime, () => RollCoolDown.Value = 0)
                    .AddTo(this);
                // 回避状態に遷移する
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Roll);
            }
        }
        
        //-------------------------------------------------------------------------------
        // パリィアクションが入力された時に呼ばれるコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnParryInput(InputAction.CallbackContext context)
        {
            // 入力を開始した時の処理
            if (context.started) 
            {
                // クールタイム中は処理を抜ける
                if (ParryCoolDown.Value > 0) return;
                // クールタイムを設定する
                ParryCoolDown.Value = InGameConsts.PlayerParryCoolDown;
                // クールタイムの購読を破棄する
                _parryDisposable?.Dispose();
                // クールタイムを購読する
                _parryDisposable = Observable.EveryUpdate()
                    .TakeWhile(_ => ParryCoolDown.Value > 0)
                    .Subscribe(_ => ParryCoolDown.Value -= Time.deltaTime, () => ParryCoolDown.Value = 0)
                    .AddTo(this);
                // パリィ状態に遷移する
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Parry);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 防御アクションが入力された時に呼ばれるコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnGuardInput(InputAction.CallbackContext context)
        {
            // 入力を開始した時の処理
            if (context.started) 
            {
                // クールタイム中は処理を抜ける
                if (GuardCoolDown.Value > 0) return;
                // クールタイムを設定する
                GuardCoolDown.Value = InGameConsts.PlayerGuardCoolDown;
                // クールタイムの購読を破棄する
                _guardDisposable?.Dispose();
                // クールタイムを購読する
                _guardDisposable = Observable.EveryUpdate()
                    .TakeWhile(_ => GuardCoolDown.Value > 0)
                    .Subscribe(_ => GuardCoolDown.Value -= Time.deltaTime, () => GuardCoolDown.Value = 0)
                    .AddTo(this);
                // 防御状態に遷移する
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Guard);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 通常攻撃アクションが入力された時に呼ばれるコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnAttackNInput(InputAction.CallbackContext context)
        {
            // 入力を開始した時の処理
            if (context.performed) 
            {
                if (!AnimationHandler.IsPlayingAtkNAnim)
                {
                    StateHandler.ChangeState(InGameEnums.PlayerStateType.AttackN);
                }
            }
        }
        
        //-------------------------------------------------------------------------------
        // 特殊攻撃アクションが入力された時に呼ばれるコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnAttackSInput(InputAction.CallbackContext context)
        {
            // 入力を開始した時の処理
            if (context.performed) 
            {
                StateHandler.ChangeState(InGameEnums.PlayerStateType.AttackS);
            }
        }
        
        //-------------------------------------------------------------------------------
        // EX攻撃アクションが入力された時に呼ばれるコールバックイベント
        //-------------------------------------------------------------------------------

        public override void OnAttackEInput(InputAction.CallbackContext context)
        {
            // 入力を実行した時の処理
            if (context.performed) 
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
            if (isLockOnEnemy)
            {
                AnimationHandler.PlayLockOnMoveAnimation(); // 移動アニメーションを再生する
            }
            else
            {
                AnimationHandler.PlayFreeMoveAnimation(); // 移動アニメーションを再生する
            }
        }

        private void OnMoveUpdate()
        {
            if (!isLockOnEnemy)
            {
                MovementHandler.RotateTowardsCameraRelativeDir(GameManager.Instance.MainCamera.transform);
            }
        }

        private void OnMoveExit()
        {
            AnimationHandler.CancelMoveAnimation(); // 移動アニメーションを中止する
        }
        
        private void OnMoveFixedUpdate()
        {
            if (isLockOnEnemy)
            {
                MovementHandler.MoveStrafe(BaseStats.strafeForce); // 入力方向に移動させる
            }
            else
            {
                MovementHandler.MoveForward(BaseStats.moveForce); // 正面方向に移動させる
            }
        }
        
        //-------------------------------------------------------------------------------
        // 回避状態のアクション
        //-------------------------------------------------------------------------------

        private void OnRollEnter()
        {
            if (isLockOnEnemy)
            {
                AnimationHandler.PlaySlideAnimation(
                    () => StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle));
                SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Rolling);
            }
            else
            {
                AnimationHandler.PlayRollAnimation(
                    () => StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle));
                SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Rolling);
            }
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
            ParticleHandler.PlayGuardParticle();
            AnimationHandler.PlayGuardAnimation(
                () => StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle));
        }

        private void OnGuardExit()
        {
            AnimationHandler.CancelGuardAnimation();
            ParticleHandler.StopGuardParticle();
        }
        
        //-------------------------------------------------------------------------------
        // 通常攻撃のアクション
        //-------------------------------------------------------------------------------

        private void OnAttackNEnter()
        {
            AttackHandler.RotateTowardsEnemyInstantly();
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
            AttackHandler.RotateTowardsEnemyInstantly();
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
            ParticleHandler.PlayAtkE01Particle();
        }

        private void OnAttackEUpdate()
        {
            MovementHandler.RotateTowardsCameraRelativeDir(GameManager.Instance.MainCamera.transform);

            CurrentEp.Value = Mathf.Max(0f, CurrentEp.Value - Time.deltaTime * MaxEp * 0.1f);

            if (CurrentEp.Value <= 0)
            {
                StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle);
            }
        }

        private void OnAttackEExit()
        {
            ParticleHandler.StopAtkE01Particle();
        }

        private void OnAttackEFixedUpdate()
        {
            MovementHandler.MoveForward(BaseStats.atkEForce);
        }
        
        //-------------------------------------------------------------------------------
        // 被弾状態のアクション
        //-------------------------------------------------------------------------------

        private void OnDamageEnter()
        {
            AnimationHandler.PlayDamageAnimation(
                () => StateHandler.ChangeState(InGameEnums.PlayerStateType.Idle));
        }

        private void OnDamageExit()
        {
            AnimationHandler.CancelGuardHitAnimation();
            AnimationHandler.CancelDamageAnimation();
        }

        private void OnDamageFixedUpdate()
        {
            MovementHandler.ApplyRootMotion(AnimationHandler.DeltaPosition);
        }
        
        //-------------------------------------------------------------------------------
        // 死亡状態のアクション
        //-------------------------------------------------------------------------------

        private void OnDeadEnter()
        {
            AnimationHandler.PlayDeadAnimation();
            enabled = false;
        }
        
        //-------------------------------------------------------------------------------
        // 被弾時の処理
        //-------------------------------------------------------------------------------

        public override void OnHit(EnemyAIBase enemy, float damage, Vector3 hitPosition)
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
                ParticleHandler.PlayHitParticle(hitPosition);
                // サウンドを再生する
                SoundManager.Instance.PlaySe(OutGameEnums.SoundType.GuardHit);
                // ヒットアニメーションを再生する
                AnimationHandler.PlayGuardHitAnimation(
                    () => StateHandler.ChangeState(StateHandler.CurrentState.StateType == InGameEnums.PlayerStateType.Guard ? InGameEnums.PlayerStateType.Guard : InGameEnums.PlayerStateType.Idle));
                return;
            }
            
            // パリィ状態
            if (StateHandler.CurrentState.StateType == InGameEnums.PlayerStateType.Parry)
            {
                // エフェクトを表示する
                ParticleHandler.PlayParryParticle();
                // サウンドを再生する
                SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Parry);
                // 被パリィ処理を呼ぶ
                enemy.OnParried();
            }

            // その他の状態
            else
            {
                // エフェクトを表示する
                ParticleHandler.PlayHitParticle(hitPosition);
                // サウンドを再生する
                SoundManager.Instance.PlaySe(OutGameEnums.SoundType.PlayerHit);
                // ダメージを適用する
                ApplyDamage(damage);
                // ダメージUIを表示する
                UIManager.Instance.ShowDamageUI(hitPosition, damage, Color.red, Color.black);
            }
        }
    }
}