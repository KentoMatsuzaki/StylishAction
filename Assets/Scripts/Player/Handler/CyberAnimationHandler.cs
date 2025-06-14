using System;
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
        private Animator _animator;
        
        private IDisposable _moveAnimDisposable;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        //-------------------------------------------------------------------------------
        // 待機アニメーション
        //-------------------------------------------------------------------------------

        public void PlayIdleAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerIdleAnimState, 0.25f);
        }
        
        //-------------------------------------------------------------------------------
        // 移動アニメーション
        //-------------------------------------------------------------------------------

        public void PlayMoveAnimation()
        {
            _animator.CrossFade(InGameConsts.PlayerMoveInAnimState, 0.05f);
            _moveAnimDisposable = 
                _animator.ObserveNormalizedTime(InGameConsts.PlayerMoveInAnimState, loop: false)
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
            
        }
        
        //-------------------------------------------------------------------------------
        // 回避アニメーション
        //-------------------------------------------------------------------------------

        public void PlayRollAnimation()
        {
            
        }
        
        //-------------------------------------------------------------------------------
        // パリィアニメーション
        //-------------------------------------------------------------------------------

        public void PlayParryAnimation()
        {
            
        }
        
        //-------------------------------------------------------------------------------
        // 防御アニメーション
        //-------------------------------------------------------------------------------

        public void PlayGuardAnimation()
        {
            
        }
        
        //-------------------------------------------------------------------------------
        // 通常攻撃アニメーション
        //-------------------------------------------------------------------------------

        public void PlayAttackNAnimation()
        {
            
        }
        
        //-------------------------------------------------------------------------------
        // 特殊攻撃アニメーション
        //-------------------------------------------------------------------------------

        public void PlayAttackSAnimation()
        {
            
        }
        
        //-------------------------------------------------------------------------------
        // EX攻撃アニメーション
        //-------------------------------------------------------------------------------

        public void PlayAttackEAnimation()
        {
            
        }
        
        //-------------------------------------------------------------------------------
        // 被弾アニメーション
        //-------------------------------------------------------------------------------

        public void PlayDamageAnimation()
        {
            
        }
        
        //-------------------------------------------------------------------------------
        // 死亡アニメーション
        //-------------------------------------------------------------------------------

        public void PlayDeadAnimation()
        {
            
        }
    }
}