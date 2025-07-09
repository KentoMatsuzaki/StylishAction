using System;
using System.Collections.Generic;
using Definitions.Const;
using Extensions;
using UnityEngine;
using Player.Interface;
using UniRx;

namespace Player.Handler
{
    /// <summary>
    /// Cyber（プレイヤー）のアニメーションを制御するクラス
    /// </summary>
    public class CyberAnimationHandler : MonoBehaviour, IPlayerAnimationHandler
    { 
        public Vector3 DeltaPosition { get; private set; }
        
        private Animator _animator;
        
        private IDisposable _moveAnimDisposable;
        private IDisposable _rollAnimDisposable;
        private IDisposable _parryAnimDisposable;
        private IDisposable _guardAnimDisposable;
        private IDisposable _damageAnimDisposable;
        
        private CompositeDisposable _atkNDisposable = new();
        private CompositeDisposable _atkSDisposable = new();

        private int _atkNAnimLoopCount;
        private int _atkSAnimLoopCount;

        private int AtkNAnimLoopCount
        {
            get => _atkNAnimLoopCount;
            set => _atkNAnimLoopCount = value >= AtkNAnimStateMap.Count ? 0 : value;
        }

        private int AtkSAnimLoopCount
        {
            get => _atkSAnimLoopCount;
            set => _atkSAnimLoopCount = value >= AtkSAnimStateMap.Count ? 0 : value;
        }
        
        public bool IsPlayingAtkNAnim { get; private set; }
        public bool IsPlayingAtkSAnim { get; private set; }

        private static readonly Dictionary<int, string> AtkNAnimStateMap = new()
        {
            { 0, InGameConsts.PlayerAttackNAnimState1 },
            { 1, InGameConsts.PlayerAttackNAnimState2 },
            { 2, InGameConsts.PlayerAttackNAnimState3 },
            { 3, InGameConsts.PlayerAttackNAnimState4 },
            { 4, InGameConsts.PlayerAttackNAnimState5 }
        };

        private static readonly Dictionary<int, string> AtkSAnimStateMap = new()
        {
            { 0, InGameConsts.PlayerAttackSAnimState1 },
            { 1, InGameConsts.PlayerAttackSAnimState2 },
            { 2, InGameConsts.PlayerAttackSAnimState3 }
        };
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        //-------------------------------------------------------------------------------
        // 更新処理
        //-------------------------------------------------------------------------------

        private void OnAnimatorMove()
        {
            DeltaPosition = _animator.deltaPosition;
        }
        
        //-------------------------------------------------------------------------------
        // 待機アニメーション
        //-------------------------------------------------------------------------------

        public void PlayIdleAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerIdleAnimState, 0.1f);
        }
        
        //-------------------------------------------------------------------------------
        // 移動アニメーション
        //-------------------------------------------------------------------------------

        public void PlayMoveAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerMoveInAnimState, 0.05f);
            _moveAnimDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerMoveInAnimState, loop: false)
                .Where(t => t >= 1.0f)
                .First()
                .Subscribe(_ =>
                {
                    _animator.CrossFade(InGameConsts.PlayerMoveLoopAnimState, 0.05f);
                });
        }

        public void CancelMoveAnimation()
        {
            _moveAnimDisposable?.Dispose();
            _moveAnimDisposable = null;
        }
        
        //-------------------------------------------------------------------------------
        // ダッシュアニメーション
        //-------------------------------------------------------------------------------

        public void PlayDashAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerDashAnimState, 0.05f);
        }
        
        //-------------------------------------------------------------------------------
        // 回避アニメーション
        //-------------------------------------------------------------------------------

        public void PlayRollAnimation(Action onFinished)
        {
            _animator.CrossFade(InGameConsts.PlayerRollAnimState, 0.05f);
            _rollAnimDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerRollAnimState, loop: false)
                .Where(t => t >= 0.7f)
                .First()
                .Subscribe(_ => onFinished?.Invoke());
        }

        public void CancelRollAnimation()
        {
            _rollAnimDisposable?.Dispose();
            _rollAnimDisposable = null;
        }
        
        //-------------------------------------------------------------------------------
        // パリィアニメーション
        //-------------------------------------------------------------------------------

        public void PlayParryAnimation(Action onFinished)
        {
            _animator.CrossFade(InGameConsts.PlayerParryAnimState, 0.05f);
            _parryAnimDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerParryAnimState, loop: false)
                .Where(t => t >= 1.0f)
                .First()
                .Subscribe(_ => onFinished?.Invoke());
        }

        public void CancelParryAnimation()
        {
            _parryAnimDisposable?.Dispose();
            _parryAnimDisposable = null;
        }
        
        //-------------------------------------------------------------------------------
        // 防御アニメーション
        //-------------------------------------------------------------------------------

        public void PlayGuardAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerGuardAnimState, 0.05f);
        }

        public void PlayGuardHitAnimation(Action onFinished)
        {
            _animator.CrossFade(InGameConsts.PlayerGuardHitAnimState, 0.05f);
            _guardAnimDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerGuardHitAnimState, loop: false)
                .Where(t => t >= 1.0f)
                .First()
                .Subscribe(_ => onFinished?.Invoke());
        }

        public void CancelGuardHitAnimation()
        {
            _guardAnimDisposable?.Dispose();
            _guardAnimDisposable = null;
        }
        
        //-------------------------------------------------------------------------------
        // 通常攻撃アニメーション
        //-------------------------------------------------------------------------------

        public void PlayAttackNAnimation(Action onFinished)
        {
            var atkNAnimState = AtkNAnimStateMap[AtkNAnimLoopCount];
            _animator.CrossFade(atkNAnimState, 0.05f);
            
            AtkNAnimLoopCount += 1;
            IsPlayingAtkNAnim = true;

            _atkNDisposable.Add(_animator.ObserveNormalizedTime(atkNAnimState, loop: false)
                .Where(t => t >= 0.5f)
                .First()
                .Subscribe(_ =>
                {
                    IsPlayingAtkNAnim = false;
                }));

            _atkNDisposable.Add(_animator.ObserveNormalizedTime(atkNAnimState, loop: false)
                .Where(t => t >= 0.75f)
                .First()
                .Subscribe(_ => onFinished?.Invoke()));
        }

        public void CancelAttackNAnimation()
        {
            _atkNDisposable?.Clear();
        }
        
        //-------------------------------------------------------------------------------
        // 特殊攻撃アニメーション
        //-------------------------------------------------------------------------------

        public void PlayAttackSAnimation(Action onFinished)
        {
            var atkSAnimState = AtkSAnimStateMap[AtkSAnimLoopCount];
            _animator.CrossFade(atkSAnimState, 0.05f);
            
            AtkSAnimLoopCount += 1;
            IsPlayingAtkSAnim = true;

            _atkSDisposable.Add(_animator.ObserveNormalizedTime(atkSAnimState, loop: false)
                .Where(t => t >= 0.65f)
                .First()
                .Subscribe(_ =>
                {
                    IsPlayingAtkSAnim = false;
                }));

            _atkSDisposable.Add(_animator.ObserveNormalizedTime(atkSAnimState, loop: false)
                .Where(t => t >= 0.95f)
                .First()
                .Subscribe(_ => onFinished?.Invoke()));
        }

        public void CancelAttackSAnimation()
        {
            _atkSDisposable?.Clear();
        }
        
        //-------------------------------------------------------------------------------
        // EX攻撃アニメーション
        //-------------------------------------------------------------------------------

        public void PlayAttackEAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerAttackEAnimState, 0.05f);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃アニメーションに関する処理
        //-------------------------------------------------------------------------------

        public void ResetAttackContext()
        {
            AtkNAnimLoopCount = 0;
            AtkSAnimLoopCount = 0;
            IsPlayingAtkNAnim = false;
            IsPlayingAtkSAnim = false;
        }
        
        //-------------------------------------------------------------------------------
        // 被弾アニメーション
        //-------------------------------------------------------------------------------

        public void PlayDamageAnimation(Action onFinished)
        {
            _animator.CrossFade(InGameConsts.PlayerDamageAnimState, 0.05f);
            _damageAnimDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerDamageAnimState, loop: false)
                .Where(t => t >= 0.75f)
                .First()
                .Subscribe(_ => onFinished?.Invoke());
        }

        public void CancelDamageAnimation()
        {
            _damageAnimDisposable?.Dispose();
            _damageAnimDisposable = null;
        }
        
        //-------------------------------------------------------------------------------
        // 死亡アニメーション
        //-------------------------------------------------------------------------------

        public void PlayDeadAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerDeadAnimState, 0.05f);
        }
    }
}