using Camera;
using SO.Player;
using Enemy;
using Enemy.AI;
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
        private PlayerLocomotionHandler _locomotionHandler;
        private PlayerStateHandler _stateHandler;
        private PlayerAttackHandler _attackHandler;
        private PlayerParticleHandler _particleHandler;
        private PlayerAnimationHandler _animationHandler;
        
        [Header("プレイヤーデータ"), SerializeField] private PlayerStatusData statusData;
        [Header("カメラの位置"), SerializeField] private Transform cameraTransform;
        [Header("敵"), SerializeField] private EnemyController enemy;
        
        private Coroutine _debounceCoroutine;

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
            _currentHp = statusData.maxHp;
        }

        /// <summary>全ての状態のアクションを登録する</summary>
        private void SetUpAllStatesActions()
        {
            // 移動状態の開始時に呼ばれる処理
            _stateHandler.MoveState.OnEnter = () =>
            {
                // 移動のフラグを切り替える
                _animationHandler.ToggleMove();
            };
            
            // 移動状態の更新処理
            _stateHandler.MoveState.OnUpdate = () =>
            {
                // プレイヤーを回転させる
                _locomotionHandler.RotateTowardsCameraRelativeDirection(cameraTransform);
                // プレイヤーを移動させる
                _locomotionHandler.MoveForward(statusData.moveSpeed);
            };
            
            // 移動状態の終了時に呼ばれる処理
            _stateHandler.MoveState.OnExit = () =>
            {
                // 移動のフラグを切り替える
                _animationHandler.ToggleMove();
            };
            
            // スプリント状態の開始時に呼ばれる処理
            _stateHandler.SprintState.OnEnter = () =>
            {
                // スプリントのフラグを切り替える
                _animationHandler.ToggleSprint();
            };
            
            // スプリント状態の更新処理
            _stateHandler.SprintState.OnUpdate = () =>
            {
                // プレイヤーを回転させる
                _locomotionHandler.RotateTowardsCameraRelativeDirection(cameraTransform);
                // プレイヤーを移動させる
                _locomotionHandler.MoveForward(statusData.moveSpeed);
            };
            
            // スプリント状態の終了時に呼ばれる処理
            _stateHandler.SprintState.OnExit = () =>
            {
                // スプリントのフラグを切り替える
                _animationHandler.ToggleSprint();
            };
        }
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void Update()
        {
            _stateHandler.ManualUpdate();
        }

        //-------------------------------------------------------------------------------
        // 移動のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            // 移動の入力値を取得する
            var moveInput = context.ReadValue<Vector2>();
            
            // 入力が実行された時の処理
            if (context.performed)
            {
                // スプリント状態でない場合
                if (_stateHandler.CurrentState != _stateHandler.SprintState)
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
                // 静止状態に切り替える
                _stateHandler.SwitchState(_stateHandler.IdleState);
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
        // 攻撃のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnAttack(InputAction.CallbackContext context)
        {
            
            // ボタンを押した瞬間の処理
            if (context.performed)
            {
                // アニメーション処理
                
                switch (context.action.name)
                {
                    case "Attack 1": _animationHandler.TriggerAttack(InGameEnum.PlayerAttackType.Iai); break;
                    case "Attack 2": _animationHandler.TriggerAttack(InGameEnum.PlayerAttackType.Hien); break;
                    case "Attack 3": _animationHandler.TriggerAttack(InGameEnum.PlayerAttackType.Shiden); break;
                }
                
                // 回転処理
                _attackHandler.RotateSmoothlyTowardsEnemy(enemy.transform.position, statusData.attackAimAssistSpeed);
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
    }
}