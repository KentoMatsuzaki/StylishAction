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
        private IDisposable _guardHitAnimDisposable;
        private IDisposable _damageAnimDisposable;
        
        private CompositeDisposable _atkNDisposable = new();
        private IDisposable _atkSDisposable;

        private int _atkNAnimLoopCount;

        private int AtkNAnimLoopCount
        {
            get => _atkNAnimLoopCount;
            set => _atkNAnimLoopCount = value >= AtkNAnimStateMap.Count ? 0 : value;
        }
        
        public bool IsPlayingAtkNAnim { get; private set; }

        private static readonly Dictionary<int, string> AtkNAnimStateMap = new()
        {
            { 0, InGameConsts.PlayerAttackNAnimState1 },
            { 1, InGameConsts.PlayerAttackNAnimState2 },
            { 2, InGameConsts.PlayerAttackNAnimState3 },
            { 3, InGameConsts.PlayerAttackNAnimState4 },
            { 4, InGameConsts.PlayerAttackNAnimState5 }
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

        public void SetMoveParameter(Vector2 moveInput)
        {
            _animator.SetFloat(InGameConsts.PlayerMoveInputX, moveInput.x);
            _animator.SetFloat(InGameConsts.PlayerMoveInputY, moveInput.y);
        }

        public void ResetMoveParameter()
        {
            _animator.SetFloat(InGameConsts.PlayerMoveInputX, 0);
            _animator.SetFloat(InGameConsts.PlayerMoveInputY, 0);
        }
        
        public void PlayFreeMoveAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerMoveFreeInAnimState, 0.05f);
            _moveAnimDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerMoveFreeInAnimState, loop: false)
                .Where(t => t >= 1.0f)
                .First()
                .Subscribe(_ =>
                {
                    _animator.CrossFade(InGameConsts.PlayerMoveFreeLoopAnimState, 0.05f);
                });
        }

        public void CancelMoveAnimation()
        {
            _moveAnimDisposable?.Dispose();
            _moveAnimDisposable = null;
        }

        public void PlayLockOnMoveAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerMoveLockOnLoopAnimState, 0.05f);
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

        public void PlaySlideAnimation(Action onFinished)
        {
            _animator.CrossFade(InGameConsts.PlayerSlideAnimState, 0.05f);
            _rollAnimDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerSlideAnimState, loop: false)
                .Where(t => t >= 0.85f)
                .First()
                .Subscribe(_ => onFinished?.Invoke());
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

        public void PlayGuardAnimation(Action onFinished)
        {
            _animator.CrossFade(InGameConsts.PlayerGuardAnimState, 0.05f);
            _guardAnimDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerGuardAnimState, loop: false)
                .Where(t => t >= 1.0f)
                .First()
                .Subscribe(_ => onFinished?.Invoke());
        }

        public void CancelGuardAnimation()
        {
            _guardAnimDisposable?.Dispose();
            _guardAnimDisposable = null;
        }

        public void PlayGuardHitAnimation(Action onFinished)
        {
            _animator.CrossFade(InGameConsts.PlayerGuardHitAnimState, 0.05f);
            _guardHitAnimDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerGuardHitAnimState, loop: false)
                .Where(t => t >= 1.0f)
                .First()
                .Subscribe(_ => onFinished?.Invoke());
        }

        public void CancelGuardHitAnimation()
        {
            _guardHitAnimDisposable?.Dispose();
            _guardHitAnimDisposable = null;
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
            _animator.CrossFade(InGameConsts.PlayerAttackSAnimState, 0.05f);
            _atkSDisposable = _animator.ObserveNormalizedTime(InGameConsts.PlayerAttackSAnimState, loop: false)
                .Where(t => t >= 0.9f)
                .First()
                .Subscribe(_ => onFinished?.Invoke());
        }

        public void CancelAttackSAnimation()
        {
            _atkSDisposable?.Dispose();
            _atkSDisposable = null;
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
            IsPlayingAtkNAnim = false;
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