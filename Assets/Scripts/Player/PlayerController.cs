using Camera;
using SO.Player;
using Enemy.AI;
using Enum;
using Particle;
using Player.Handler;
using Player.Interface;
using SO.Enemy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    /// <summary>プレイヤーを制御するクラス</summary>
    public class PlayerController : MonoBehaviour, IPlayerAnimationEventHandler
    {
        private PlayerLocomotionHandler _locomotionHandler;
        private PlayerStateHandler _stateHandler;
        private PlayerAttackHandler _attackHandler;
        private PlayerAnimationHandler _animationHandler;
        private PlayerInput _playerInput;
        
        [Header("ステータス情報"), SerializeField] private PlayerStats stats;
        [Header("カメラの位置"), SerializeField] private Transform cameraTransform;
        [Header("敵"), SerializeField] private EnemyAIBase enemy;

        public Transform modelTransform;

        private float _currentHp;
        private Collider _collider;
        public PlayerAttackStats CurrentAttackStats { get; private set; }
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Start()
        {
            _locomotionHandler = GetComponent<PlayerLocomotionHandler>();
            _stateHandler = GetComponent<PlayerStateHandler>();
            _attackHandler = GetComponent<PlayerAttackHandler>();
            _animationHandler = GetComponent<PlayerAnimationHandler>();
            _collider = GetComponent<Collider>();
            Initialize();
            SetUpAllStatesActions();
        }

        private void Initialize()
        {
            _currentHp = stats.maxHp;
        }

        /// <summary>全ての状態のアクションを登録する</summary>
        private void SetUpAllStatesActions()
        {
            // 移動状態の開始時に呼ばれる処理
            _stateHandler.MoveState.OnEnter = () =>
            {
                // 移動のフラグを有効化する
                _animationHandler.EnableMove();
            };
            
            // 移動状態の更新処理
            _stateHandler.MoveState.OnUpdate = () =>
            {
                // プレイヤーを回転させる
                _locomotionHandler.RotateTowardsCameraRelativeDirection(cameraTransform);
                // プレイヤーを移動させる
                _locomotionHandler.MoveForward(stats.moveSpeed);
            };
            
            // 移動状態の終了時に呼ばれる処理
            _stateHandler.MoveState.OnExit = () =>
            {
                // 移動のフラグを無効化する
                _animationHandler.DisableMove();
            };
            
            // スプリント状態の開始時に呼ばれる処理
            _stateHandler.SprintState.OnEnter = () =>
            {
                // スプリントのフラグを有効化する
                _animationHandler.EnableSprint();
            };
            
            // スプリント状態の更新処理
            _stateHandler.SprintState.OnUpdate = () =>
            {
                // プレイヤーを回転させる
                _locomotionHandler.RotateTowardsCameraRelativeDirection(cameraTransform);
                // プレイヤーを移動させる
                _locomotionHandler.MoveForward(stats.sprintSpeed);
            };
            
            // スプリント状態の終了時に呼ばれる処理
            _stateHandler.SprintState.OnExit = () =>
            {
                // スプリントのフラグを無効化する
                _animationHandler.DisableSprint();
            };
            
            // 回避状態の開始時に呼ばれる処理
            _stateHandler.DodgeState.OnEnter = () =>
            {
                // Y軸の回転を固定する
                _locomotionHandler.FreezeRotationY();
                // RootMotionを有効化する
                _animationHandler.EnableRootMotion();
                // 回避のアニメーションを再生する
                _animationHandler.PlayDodgeAnimation();
                // 回避アニメーションに合わせて、前方に力を加える
                _locomotionHandler.ApplyDodgeForce(stats.dodgePower);
            };
            
            // 回避状態の終了時に呼ばれる処理
            _stateHandler.DodgeState.OnExit = () =>
            {
                // Y軸の回転の固定を解除する
                _locomotionHandler.UnfreezeRotationY();
                // RootMotionを無効化する
                _animationHandler.DisableRootMotion();
                // モデルと本体の位置を同期させる
                _locomotionHandler.SyncWithModelPosition(modelTransform);
            };
            
            // 通常攻撃状態の開始時に呼ばれる処理
            _stateHandler.AttackNormalState.OnEnter = () =>
            {
                // 敵の方向へ回転させる
                _attackHandler.RotateTowardsEnemyInstantly(enemy);
                // 現在の攻撃の情報を通常攻撃に設定する
                CurrentAttackStats = _attackHandler.GetAttackStats(PlayerEnums.AttackType.AttackNormal);
                // RootMotionを有効化する
                _animationHandler.EnableRootMotion();
                // 通常攻撃のトリガーを有効化する
                _animationHandler.TriggerAttackNormal();
            };
            
            // 通常攻撃状態の終了時に呼ばれる処理
            _stateHandler.AttackNormalState.OnExit = () =>
            {
                // RootMotionを無効化する
                _animationHandler.DisableRootMotion();
                // モデルと本体の位置を同期させる
                _locomotionHandler.SyncWithModelPosition(modelTransform);
            };
            
            // 特殊攻撃状態の開始時に呼ばれる処理
            _stateHandler.AttackSpecialState.OnEnter = () =>
            {
                // 敵の方向へ回転させる
                _attackHandler.RotateTowardsEnemyInstantly(enemy);
                // 現在の攻撃の情報を特殊攻撃に設定する
                CurrentAttackStats = _attackHandler.GetAttackStats(PlayerEnums.AttackType.AttackSpecial);
                // RootMotionを有効化する
                _animationHandler.EnableRootMotion();
                // 特殊攻撃のトリガーを有効化する
                _animationHandler.TriggerAttackSpecial();
            };
            
            // 特殊攻撃状態の終了時に呼ばれる処理
            _stateHandler.AttackSpecialState.OnExit = () =>
            {
                // RootMotionを無効化する
                _animationHandler.DisableRootMotion();
                // モデルと本体の位置を同期させる
                _locomotionHandler.SyncWithModelPosition(modelTransform);
            };
            
            // EX攻撃状態の開始時に呼ばれる処理
            _stateHandler.AttackExtraState.OnEnter = () =>
            {
                // 敵の方向へ回転させる
                _attackHandler.RotateTowardsEnemyInstantly(enemy);
                // 現在の攻撃の情報をEX攻撃に設定する
                CurrentAttackStats = _attackHandler.GetAttackStats(PlayerEnums.AttackType.AttackExtra);
                // EX攻撃のフラグを有効化する
                _animationHandler.EnableAttackExtra();
                // EX攻撃のパーティクルを有効化する
                ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.AttackExtra);
            };

            // EX攻撃状態の更新処理
            _stateHandler.AttackExtraState.OnUpdate = () =>
            {
                // プレイヤーを回転させる
                _locomotionHandler.RotateTowardsCameraRelativeDirection(cameraTransform);
                // プレイヤーを移動させる
                _locomotionHandler.ApplyAttackExtraMovement(stats.attackExtraSpeed);
            };
            
            // EX攻撃状態の終了時に呼ばれる処理
            _stateHandler.AttackExtraState.OnExit = () =>
            {
                // EX攻撃のフラグを無効化する
                _animationHandler.DisableAttackExtra();
                // EX攻撃のパーティクルを無効化する
                ParticleManager.Instance.DeactivateParticle(ParticleEnums.ParticleType.AttackExtra);
            };
            
            // パリィ状態の開始時に呼ばれる処理
            _stateHandler.ParryState.OnEnter = () =>
            {
                // パリィのアニメーションを再生する
                _animationHandler.PlayParryAnimation();
            };
            
            // 防御状態の開始時に呼ばれる処理
            _stateHandler.GuardState.OnEnter = () =>
            {
                // 防御のフラグを有効化する
                _animationHandler.EnableGuard();
            };
            
            // 防御状態の終了時に呼ばれる処理
            _stateHandler.GuardState.OnExit = () =>
            {
                // 防御のフラグを無効化する
                _animationHandler.DisableGuard();
            };
            
            // 被弾状態の開始時に呼ばれる処理
            _stateHandler.DamageState.OnEnter = () =>
            {
                // RootMotionを有効化する
                _animationHandler.EnableRootMotion();
                // 被弾アニメーションを再生する
                _animationHandler.PlayDamageAnimation();
            };
            
            // 被弾状態の終了時に呼ばれる処理
            _stateHandler.DamageState.OnExit = () =>
            {
                // RootMotionを無効化する
                _animationHandler.DisableRootMotion();
                // モデルと本体の位置を同期させる
                _locomotionHandler.SyncWithModelPosition(modelTransform);
            };
            
            // 死亡状態の開始時に呼ばれる処理
            _stateHandler.DeathState.OnExit = () =>
            {
                // 死亡アニメーションを再生する
                //_animationHandler.PlayDieAnimation();
            };
        }
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void Update()
        {
            _stateHandler.CurrentState.Update();
        }

        //-------------------------------------------------------------------------------
        // 移動のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            // 移動入力を受け付けない場合は処理を抜ける
            if (!_stateHandler.CanAcceptMoveInput()) return;
            
            // 移動の入力値を取得する
            var moveInput = context.ReadValue<Vector2>();
            
            // 入力が実行された時の処理
            if (context.performed)
            {
                // スプリント状態またはEX攻撃状態でない場合
                if (_stateHandler.CurrentState != _stateHandler.SprintState &&
                    _stateHandler.CurrentState != _stateHandler.AttackExtraState)
                {
                    // 移動状態に切り替える
                    _stateHandler.SwitchState(_stateHandler.MoveState);
                }
                // 移動方向を設定する
                _locomotionHandler.SetMoveDirection(moveInput);
                // アニメーターの移動パラメーターを設定する
                _animationHandler.SetMoveParameter(moveInput);
            }
            
            // 入力が終了された時の処理
            else if (context.canceled)
            {
                // EX攻撃状態でない場合
                if (_stateHandler.CurrentState != _stateHandler.AttackExtraState)
                {
                    // 静止状態に切り替える
                    _stateHandler.SwitchState(_stateHandler.IdleState);
                }
                // 移動方向を初期化する
                _locomotionHandler.SetMoveDirection(Vector2.zero);
                // アニメーターの移動パラメーターを初期化する
                _animationHandler.SetMoveParameter(Vector2.zero);
            }
        }
        
        //-------------------------------------------------------------------------------
        // スプリントのコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
        public void OnSprint(InputAction.CallbackContext context)
        {
            // 入力が実行された時の処理
            if (context.performed)
            {
                // 移動入力がある場合
                if (_locomotionHandler.MoveDirection.magnitude > 0.1f)
                {
                    // スプリント状態に切り替える
                    _stateHandler.SwitchState(_stateHandler.SprintState);
                }
            }
            
            // 入力が終了された時の処理
            if (context.canceled)
            {
                // 移動入力がある場合
                if (_locomotionHandler.MoveDirection.magnitude > 0.1f)
                {
                    // 移動状態に切り替える
                    _stateHandler.SwitchState(_stateHandler.MoveState);
                }
                // 移動入力がない場合
                else
                {
                    // 静止状態に切り替える
                    _stateHandler.SwitchState(_stateHandler.IdleState);
                }
            }
        }
        
        //-------------------------------------------------------------------------------
        // 回避のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
        public void OnDodge(InputAction.CallbackContext context)
        {
            // 入力が開始された時の処理
            if (context.started)
            {
                _stateHandler.SwitchState(_stateHandler.DodgeState);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 通常攻撃のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
        public void OnAttackNormal(InputAction.CallbackContext context)
        {
            // 入力が実行された時の処理
            if (context.performed)
            {
                // 通常攻撃状態に切り替える
                _stateHandler.SwitchState(_stateHandler.AttackNormalState);
                // 回転処理
                //_attackHandler.RotateSmoothlyTowardsEnemy(enemy.transform.position, stats.attackAimAssistSpeed);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 特殊攻撃のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
        public void OnAttackSpecial(InputAction.CallbackContext context)
        {
            // 入力が実行された時の処理
            if (context.performed)
            {
                // 特殊攻撃状態に切り替える
                _stateHandler.SwitchState(_stateHandler.AttackSpecialState);
            }
        }
        
        //-------------------------------------------------------------------------------
        // EX攻撃のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
        public void OnAttackExtra(InputAction.CallbackContext context)
        {
            // 入力が実行された時の処理
            if (context.performed)
            {
                // EX攻撃状態である場合
                if (_stateHandler.CurrentState == _stateHandler.AttackExtraState)
                {
                    // 遷移状態に切り替える
                    _stateHandler.SwitchState(_stateHandler.TransitionState);
                }
                
                // EX攻撃状態でない場合
                else
                {
                    // EX攻撃状態に切り替える
                    _stateHandler.SwitchState(_stateHandler.AttackExtraState);
                }
            }
        }
        
        //-------------------------------------------------------------------------------
        // パリィのコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
        public void OnParry(InputAction.CallbackContext context)
        {
            // 入力が開始された時の処理
            if (context.started)
            {
                // パリィ状態に切り替える
                _stateHandler.SwitchState(_stateHandler.ParryState);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 防御のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
        public void OnGuard(InputAction.CallbackContext context)
        {
            // 入力が実行された時の処理
            if (context.performed)
            {
                // 防御状態に切り替える
                _stateHandler.SwitchState(_stateHandler.GuardState);
            }
            
            // 入力が終了された時の処理
            if (context.canceled)
            {
                // 防御状態である場合
                if (_stateHandler.CurrentState == _stateHandler.GuardState)
                {
                    // 遷移状態に切り替える
                    _stateHandler.SwitchState(_stateHandler.TransitionState);
                }
            }
        }
        
        //-------------------------------------------------------------------------------
        // 被弾時の処理
        //-------------------------------------------------------------------------------

        /// <summary>ダメージを適用する</summary>
        public void ApplyDamage(EnemyAIBase enemyAI, EnemyAttackStats attackStats)
        {
            // ダメージを受け付けない状態である場合
            if (!_stateHandler.IsDamageReceivable())
            {
                // 処理を抜ける
                return;
            }
            
            // パリィ状態である場合
            if (_stateHandler.CurrentState == _stateHandler.ParryState)
            {
                // パリィのエフェクトを表示する
                
                // 敵側のパリィの結果を適用する処理を呼ぶ
                enemyAI.ApplyParry();
            }
            
            // 防御状態である場合
            else if (_stateHandler.CurrentState == _stateHandler.GuardState)
            {
                // 防御のエフェクトを表示する
                
            }
            
            // その他の状態である場合
            else
            {
                // ダメージを反映する
                TakeDamage(attackStats.attackDamage);
            }
        }

        /// <summary>ダメージを反映する</summary>
        private void TakeDamage(float attackDamage)
        {
            // ダメージが0以下の場合はログを出力して処理を抜ける
            if (attackDamage <= 0)
            {
                Debug.LogWarning($"Received non-positive damage value : {attackDamage}");
                return;
            }

            // 現在の体力からダメージ量を減少させる
            _currentHp = Mathf.Max(0, _currentHp - attackDamage);
            Debug.Log($"Received damage : {attackDamage}. Current HP : {_currentHp}");
            
            // 死亡している場合
            if (_currentHp <= 0)
            {
                // 死亡時の処理を呼び出す
                ApplyDeath();
            }
            // 死亡していない場合
            else
            {
                // 被弾状態に切り替える
                _stateHandler.SwitchState(_stateHandler.DamageState);
            }
        }

        /// <summary>死亡時の処理</summary>
        private void ApplyDeath()
        {
            // 死亡状態に切り替える
            _stateHandler.SwitchState(_stateHandler.DeathState);
        }
        
        //-------------------------------------------------------------------------------
        // ロックオン処理
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnLockOn(InputAction.CallbackContext context)
        {
            // ボタンを押した瞬間の処理
            if (context.performed)
            {
                // 非ロックオン時
                if (!OrbitCamera.Instance.IsLockingOnEnemy)
                {
                    // 敵の方を向く
                    
                }
                
                OrbitCamera.Instance.SwitchLockOnTarget();
            }
        }
        
        //-------------------------------------------------------------------------------
        // アニメーションイベント
        //-------------------------------------------------------------------------------
        
        /// <summary>静止状態に切り替える</summary>
        public void SwitchStateToIdle()
        {
            _stateHandler.SwitchState(_stateHandler.IdleState);
        }

        /// <summary>遷移状態に切り替える</summary>
        public void SwitchStateToTransition()
        {
            _stateHandler.SwitchState(_stateHandler.TransitionState);
        }
        
        /// <summary>特殊攻撃アニメーション4のアニメーションイベントから呼ばれる</summary>
        public void ApplyAttackSpecial4Force()
        {
            _locomotionHandler.ApplyAttackSpecial4Force(stats.attackSpecial4Power);
        }

        /// <summary>通常攻撃1の攻撃パーティクルを有効化する</summary>
        public void ActivateAttackNormal1Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.AttackNormal1);
        }

        public void ActivateAttackNormal2Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.AttackNormal2);
        }

        public void ActivateAttackNormal3Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.AttackNormal3);
        }

        public void ActivateAttackNormal4Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.AttackNormal4);
        }

        public void ActivateAttackNormal5Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.AttackNormal5);
        }

        public void ActivateAttackSpecial1Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.AttackSpecial1);
        }

        public void ActivateAttackSpecial2Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.AttackSpecial2);
        }

        public void ActivateAttackSpecial3Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.AttackSpecial3);
        }
    }
}