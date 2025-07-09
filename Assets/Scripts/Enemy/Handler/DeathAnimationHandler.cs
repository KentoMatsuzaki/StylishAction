using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Const;
using Definitions.Enum;
using Enemy.Interface;
using Extensions;
using UniRx;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>Death（敵）のアニメーションを制御するクラス</summary>
    public class DeathAnimationHandler : MonoBehaviour, IEnemyAnimationHandler
    {
        private Animator _animator;
        private IDisposable _attackAnimDisposable;

        /// <summary>攻撃アニメーションステートのマップ</summary>
        private static readonly Dictionary<InGameEnums.EnemyAttackType, string> AttackAnimStateMap = new()
        {
            { InGameEnums.EnemyAttackType.Scythe, InGameConsts.DeathScytheAnimState },
            { InGameEnums.EnemyAttackType.Meteor, InGameConsts.DeathMeteorAnimState },
            { InGameEnums.EnemyAttackType.Seraph, InGameConsts.DeathSeraphAnimState },
            { InGameEnums.EnemyAttackType.Eclipse, InGameConsts.DeathEclipseAnimState },
            { InGameEnums.EnemyAttackType.Vortex, InGameConsts.DeathVortexAnimState },
            { InGameEnums.EnemyAttackType.Photon, InGameConsts.DeathPhotonAnimState },
            { InGameEnums.EnemyAttackType.Explosion, InGameConsts.DeathExplosionAnimState },
            { InGameEnums.EnemyAttackType.Waterfall, InGameConsts.DeathWaterfallAnimState },
        };
        
        public bool IsPlayingMoveAnimation { get; set; }
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃アニメーション
        //-------------------------------------------------------------------------------

        public void PlayAttackAnimation(InGameEnums.EnemyAttackType type)
        {
            _animator.CrossFade(AttackAnimStateMap[type], 0.05f);

            _attackAnimDisposable = _animator.ObserveNormalizedTime(AttackAnimStateMap[type], false)
                .Where(t => t >= 0.75f)
                .First()
                .Subscribe(_ =>
                {
                    CancelAttackAnimation();
                    PlayIdleAnimation();
                });
        }

        private void CancelAttackAnimation()
        {
            _attackAnimDisposable?.Dispose();
            _attackAnimDisposable = null;
        }
        
        //-------------------------------------------------------------------------------
        // 待機アニメーション
        //-------------------------------------------------------------------------------

        public void PlayIdleAnimation()
        {
            _animator.CrossFade(InGameConsts.DeathIdleAnimState, 0.1f);
        }
        
        //-------------------------------------------------------------------------------
        // 移動アニメーション
        //-------------------------------------------------------------------------------

        public void PlayMoveAnimation()
        {
            IsPlayingMoveAnimation = true;
            _animator.CrossFade(InGameConsts.DeathMoveAnimState, 0.05f);
        }
        
        //-------------------------------------------------------------------------------
        // 被パリィアニメーション
        //-------------------------------------------------------------------------------

        public void PlayParriedAnimation()
        {
            _animator.CrossFade(InGameConsts.DeathParriedAnimState, 0.05f);
        }

        public void PlayRecoveryAnimation()
        {
            _animator.CrossFade(InGameConsts.DeathRecoveryAnimState, 0.05f);
        }
        
        //-------------------------------------------------------------------------------
        // 死亡アニメーション
        //-------------------------------------------------------------------------------

        public void PlayDieAnimation()
        {
            _animator.CrossFade(InGameConsts.DeathDieAnimState, 0.05f);
        }
        
        //-------------------------------------------------------------------------------
        // アニメーションの待機処理
        //-------------------------------------------------------------------------------

        public async UniTask WaitUntilAnimationComplete()
        {
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        }
    }
}