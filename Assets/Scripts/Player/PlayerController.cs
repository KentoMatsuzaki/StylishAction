using Camera;
using SO.Player;
using Enemy;
using Enemy.AI;
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
        private PlayerAttackHandler _attackHandler;
        private PlayerEffectHandler _effectHandler;
        private PlayerAnimationHandler _animationHandler;
        
        [Header("プレイヤーデータ"), SerializeField] private PlayerStatusData statusData;
        [Header("カメラの位置"), SerializeField] private Transform cameraTransform;
        [Header("敵"), SerializeField] private EnemyController enemy;

        private float _currentHp;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Start()
        {
            _moveHandler = GetComponent<PlayerMoveHandler>();
            _stateHandler = GetComponent<PlayerStateHandler>();
            _attackHandler = GetComponent<PlayerAttackHandler>();
            _effectHandler = GetComponent<PlayerEffectHandler>();
            _animationHandler = GetComponent<PlayerAnimationHandler>();
            Initialize();
        }

        private void Initialize()
        {
            _currentHp = statusData.maxHp;
        }
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void FixedUpdate()
        {
            // 移動状態なら移動処理を呼ぶ
            if (_stateHandler.GetCurrentState() is PlayerEnum.PlayerState.Move)
            {
                _moveHandler.MoveForward(statusData.moveSpeed);
            }
        }

        //-------------------------------------------------------------------------------
        // 移動のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            // 移動が不可能なら処理を抜ける
            if (!_stateHandler.CanMove()) return;
            
            // ボタンを押した瞬間の処理
            if (context.performed)
            {
                // 状態処理
                _stateHandler.SetCurrentState(PlayerEnum.PlayerState.Move);
                // アニメーション処理
                _animationHandler.SetMoveFlag(true);
                // 回転処理
                _moveHandler.RotateToInputRelativeToCamera(context.ReadValue<Vector2>(), cameraTransform);
            }
            // ボタンを離した瞬間の処理
            else if (context.canceled)
            {
                // 状態処理
                _stateHandler.SetCurrentState(PlayerEnum.PlayerState.Idle);
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
            // 移動が不可能なら処理を抜ける
            if (!_stateHandler.CanMove()) return;
            
            // ボタンを押した瞬間の処理
            if (context.performed)
            {
                // 状態処理
                _stateHandler.SetCurrentState(PlayerEnum.PlayerState.Move);
                // アニメーション処理
                _animationHandler.TriggerDash();
                // 移動処理
                _moveHandler.DashForward(statusData.dashSpeed);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃のコールバック
        //-------------------------------------------------------------------------------

        /// <summary>PlayerInputから呼ばれる</summary>
        public void OnAttack(InputAction.CallbackContext context)
        {
            // 攻撃が不可能なら処理を抜ける
            if (!_stateHandler.CanAttack()) return;
            
            // ボタンを押した瞬間の処理
            if (context.performed)
            {
                // 状態処理
                _stateHandler.SetCurrentState(PlayerEnum.PlayerState.Attack);
                // アニメーション処理
                _animationHandler.SetMoveFlag(false);
                
                switch (context.action.name)
                {
                    case "Attack 1": _animationHandler.TriggerAttack(1); break;
                    case "Attack 2": _animationHandler.TriggerAttack(2); break;
                    case "Attack 3": _animationHandler.TriggerAttack(3); break;
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
                // 状態処理
                _stateHandler.SetCurrentState(PlayerEnum.PlayerState.Parry);
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
            if (_stateHandler.GetCurrentState() is PlayerEnum.PlayerState.Parry)
            {
                _effectHandler.ActivateParryEffect();
                ai.OnParried();
                return;
            }
            
            // ダメージ処理
            TakeDamage(damage);
            
            // 死亡処理
            if (IsDied())
            {
                OnDie();
            }
            
            // 状態処理
            _stateHandler.SetCurrentState(PlayerEnum.PlayerState.Damage);
            // アニメーション処理
            _animationHandler.SetMoveFlag(false);
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
            // 状態処理
            _stateHandler.SetCurrentState(PlayerEnum.PlayerState.Dead);
            // アニメーション処理
            _animationHandler.PlayDieAnimation();
        }
    }
}