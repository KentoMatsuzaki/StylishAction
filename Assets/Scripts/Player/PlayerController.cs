using Camera;
using SO.Player;
using Enemy;
using Enemy.AI;
using Enum;
using Player.Handler;
using Player.Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>プレイヤーを制御するクラス</summary>
    public class PlayerController : MonoBehaviour, IPlayerAnimationEventHandler
    {
        private PlayerLocomotionHandler _locomotionHandler;
        private PlayerStateHandler _stateHandler;
        private PlayerAttackHandler _attackHandler;
        private PlayerParticleHandler _particleHandler;
        private PlayerAnimationHandler _animationHandler;
        private PlayerInput _playerInput;
        
        [Header("ステータス情報"), SerializeField] private PlayerStats stats;
        [Header("カメラの位置"), SerializeField] private Transform cameraTransform;
        [Header("敵"), SerializeField] private EnemyController enemy;

        public Transform modelTransform;

        private float _currentHp;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Start()
        {
            _locomotionHandler = GetComponent<PlayerLocomotionHandler>();
            _stateHandler = GetComponent<PlayerStateHandler>();
            _attackHandler = GetComponent<PlayerAttackHandler>();
            _particleHandler = GetComponent<PlayerParticleHandler>();
            _animationHandler = GetComponent<PlayerAnimationHandler>();
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
                // EX攻撃のフラグを有効化する
                _animationHandler.EnableAttackExtra();
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
                    // 着地状態に切り替える
                    _stateHandler.SwitchState(_stateHandler.LandState);
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
                    _attackHandler.RotateInstantlyTowardsEnemy(enemy.transform.position);
                }
                
                OrbitCamera.Instance.SwitchLockOnTarget();
            }
        }
        
        //-------------------------------------------------------------------------------
        // 被ダメージ処理
        //-------------------------------------------------------------------------------

        /// <summary>敵の攻撃が命中した時の処理</summary>
        public void OnHitByEnemy(float damage, EnemyAIBase ai)
        {
            // パリィした場合は処理を抜ける
            _particleHandler.ActivateParticle(InGameEnum.PlayerParticleType.Parry.ToString());
            ai.OnParried();
            
            // ダメージ処理
            TakeDamage(damage);
            
            // 死亡処理
            if (IsDied())
            {
                OnDie();
            }
            
            // アニメーション処理
            _animationHandler.PlayHitAnimation();
        }

        /// <summary>ダメージ処理</summary>
        /// <param name="damage"></param>
        private void TakeDamage(float damage)
        {
            if (damage > 0)
            {
                // ダメージを適用する
                _currentHp = Mathf.Max(_currentHp - damage, 0);
            }
        }
        
        /// <summary>死亡判定</summary>
        private bool IsDied() => _currentHp == 0;

        /// <summary>死亡時の処理</summary>
        private void OnDie()
        {

            // アニメーション処理
            _animationHandler.PlayDieAnimation();
        }
        
        //-------------------------------------------------------------------------------
        // アニメーションイベント
        //-------------------------------------------------------------------------------
        
        /// <summary>静止アニメーションのアニメーションイベントから呼ばれる</summary>
        public void SwitchStateToIdle()
        {
            _stateHandler.SwitchState(_stateHandler.IdleState);
        }
        
        /// <summary>特殊攻撃アニメーション4のアニメーションイベントから呼ばれる</summary>
        public void ApplyAttackSpecial4Force()
        {
            _locomotionHandler.ApplyAttackSpecial4Force(stats.attackSpecial4Power);
        }
    }
}