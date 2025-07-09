using System;
using UnityEngine;
using Player.Interface;
using Definitions.Enum;
using System.Collections.Generic;
using System.Linq;

namespace Player.Handler
{
    /// <summary>
    /// Cyber（プレイヤー）の状態を制御するクラス
    /// </summary>
    public class CyberStateHandler : MonoBehaviour, IPlayerStateHandler
    {
        private readonly PlayerState _idleState = new(InGameEnums.PlayerStateType.Idle);       // 待機状態
        private readonly PlayerState _moveState = new(InGameEnums.PlayerStateType.Move);       // 移動状態
        private readonly PlayerState _dashState = new(InGameEnums.PlayerStateType.Dash);       // ダッシュ状態
        private readonly PlayerState _rollState = new(InGameEnums.PlayerStateType.Roll);       // 回避状態
        private readonly PlayerState _parryState = new(InGameEnums.PlayerStateType.Parry);     // パリィ状態
        private readonly PlayerState _guardState = new(InGameEnums.PlayerStateType.Guard);     // 防御状態
        private readonly PlayerState _attackNState = new(InGameEnums.PlayerStateType.AttackN); // 通常攻撃
        private readonly PlayerState _attackSState = new(InGameEnums.PlayerStateType.AttackS); // 特殊攻撃
        private readonly PlayerState _attackEState = new(InGameEnums.PlayerStateType.AttackE); // EX攻撃
        private readonly PlayerState _damageState = new(InGameEnums.PlayerStateType.Damage);   // 被弾状態
        private readonly PlayerState _deadState = new(InGameEnums.PlayerStateType.Dead);       // 死亡状態
        public PlayerState CurrentState { get; private set; } // 現在の状態
        
        // 各状態とそのインスタンスを紐づけたマップ
        private readonly Dictionary<InGameEnums.PlayerStateType, PlayerState> _playerStates = new();
        
        // 各状態から「遷移できない状態」を定義するマップ
        private readonly Dictionary<InGameEnums.PlayerStateType, PlayerState[]> _invalidMap = new();
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            // 各状態とインスタンスを紐づける
            _playerStates[InGameEnums.PlayerStateType.Idle] = _idleState;
            _playerStates[InGameEnums.PlayerStateType.Move] = _moveState;
            _playerStates[InGameEnums.PlayerStateType.Dash] = _dashState;
            _playerStates[InGameEnums.PlayerStateType.Roll] = _rollState;
            _playerStates[InGameEnums.PlayerStateType.Parry] = _parryState;
            _playerStates[InGameEnums.PlayerStateType.Guard] = _guardState;
            _playerStates[InGameEnums.PlayerStateType.AttackN] = _attackNState;
            _playerStates[InGameEnums.PlayerStateType.AttackS] = _attackSState;
            _playerStates[InGameEnums.PlayerStateType.AttackE] = _attackEState;
            _playerStates[InGameEnums.PlayerStateType.Damage] = _damageState;
            _playerStates[InGameEnums.PlayerStateType.Dead] = _deadState;
            
            // 待機状態
            _invalidMap[InGameEnums.PlayerStateType.Idle] = new PlayerState[]
            {

            };

            // 移動状態
            _invalidMap[InGameEnums.PlayerStateType.Move] = new[] 
            { 
                _moveState, _parryState, _guardState, _attackNState, _attackSState, _attackEState
            };
            
            // ダッシュ状態
            _invalidMap[InGameEnums.PlayerStateType.Dash] = new[]
            {
                _rollState, _parryState, _guardState, _attackNState, _attackSState, _attackEState
            };
            
            // 回避状態
            _invalidMap[InGameEnums.PlayerStateType.Roll] = new[]
            {
                _moveState, _dashState, _parryState, _guardState, _attackNState, _attackSState, _attackEState
            };
            
            // パリィ状態
            _invalidMap[InGameEnums.PlayerStateType.Parry] = new[]
            {
                _moveState, _dashState, _rollState, _guardState, _attackNState, _attackSState, _attackEState
            };
            
            // 防御状態
            _invalidMap[InGameEnums.PlayerStateType.Guard] = new[]
            {
                _moveState, _dashState, _rollState, _parryState, _attackNState, _attackSState, _attackEState
            };
            
            // 通常攻撃
            _invalidMap[InGameEnums.PlayerStateType.AttackN] = new[]
            {
                _moveState, _dashState, _rollState, _parryState, _guardState, _attackSState, _attackEState
            };
            
            // 特殊攻撃
            _invalidMap[InGameEnums.PlayerStateType.AttackS] = new[]
            {
                _moveState, _dashState, _rollState, _parryState, _guardState, _attackNState, _attackEState
            };
            
            // EX攻撃
            _invalidMap[InGameEnums.PlayerStateType.AttackE] = new[]
            {
                _moveState, _dashState, _rollState, _parryState, _guardState, _attackNState, _attackSState
            };
            
            // 被弾状態
            _invalidMap[InGameEnums.PlayerStateType.Damage] = new[]
            {
                _moveState, _dashState, _rollState, _parryState, _guardState, _attackNState, _attackSState, _attackEState
            };
            
            // 死亡状態
            _invalidMap[InGameEnums.PlayerStateType.Dead] = new[]
            {
                _moveState, _dashState, _rollState, _parryState, _guardState, _attackNState, _attackSState, _attackEState
            };
            
            // 初期状態は待機状態に設定する
            CurrentState = _idleState;
        }
        
        //-------------------------------------------------------------------------------
        // 状態の制御に関する処理
        //-------------------------------------------------------------------------------
        
        public void SetStateAction(InGameEnums.PlayerStateType stateType, Action onEnter, Action onUpdate, Action onExit, Action onFixedUpdate)
        {
            _playerStates[stateType].SetAction(onEnter, onUpdate, onExit, onFixedUpdate);
        }
        
        public bool CanChangeState(InGameEnums.PlayerStateType newStateType)
        {
            return !_invalidMap[CurrentState.StateType].Contains(_playerStates[newStateType]);
        }
        
        public void ChangeState(InGameEnums.PlayerStateType newStateType)
        {
            if (!CanChangeState(newStateType)) return; // 状態を変更できない場合は処理を抜ける
            
            CurrentState.OnExit?.Invoke();  // 現在の状態の終了アクションを呼ぶ
            CurrentState = _playerStates[newStateType]; // 新しい状態に変更する
            CurrentState.OnEnter?.Invoke(); // 新しい状態の開始アクションを呼ぶ
        }

        public bool IsInvincible()
        {
            if (CurrentState.StateType == InGameEnums.PlayerStateType.Roll ||
                CurrentState.StateType == InGameEnums.PlayerStateType.AttackS ||
                CurrentState.StateType == InGameEnums.PlayerStateType.AttackE || 
                CurrentState.StateType == InGameEnums.PlayerStateType.Damage ||
                CurrentState.StateType == InGameEnums.PlayerStateType.Dead) return true; return false;
        }

        public bool CanHandleMoveInput()
        {
            if (CurrentState.StateType == InGameEnums.PlayerStateType.Idle ||
                CurrentState.StateType == InGameEnums.PlayerStateType.Move ||
                CurrentState.StateType == InGameEnums.PlayerStateType.Dash ||
                CurrentState.StateType == InGameEnums.PlayerStateType.AttackE)
                return true; return false;
        }
    }
}