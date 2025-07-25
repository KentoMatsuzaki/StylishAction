using System;
using UnityEngine;

namespace Player.Interface
{
    /// <summary>プレイヤーのアニメーション制御に関するインターフェース</summary>
    public interface IPlayerAnimationHandler
    {
        /// <summary>ルートモーションによる移動の変化量</summary>
        Vector3 DeltaPosition { get; }
        
        /// <summary>通常攻撃アニメーションを再生しているか</summary>
        bool IsPlayingAtkNAnim { get; }
        
        /// <summary>特殊攻撃アニメーションを再生しているか</summary>
        bool IsPlayingAtkSAnim { get; }
        
        /// <summary>待機アニメーションを再生する</summary>
        void PlayIdleAnimation();
        
        /// <summary>移動アニメーション（非ロックオン）を再生する</summary>
        void PlayFreeMoveAnimation();

        /// <summary>移動パラメーターを設定する</summary>
        void SetMoveParameter(Vector2 moveInput);

        /// <summary>移動パラメーターを初期化する</summary>
        void ResetMoveParameter();
        
        /// <summary>移動アニメーション（ロックオン）を再生する</summary>
        void PlayLockOnMoveAnimation();
        
        /// <summary>移動アニメーションを中止する</summary>
        void CancelMoveAnimation();
        
        /// <summary>回避アニメーション（ローリング）を再生する</summary>
        void PlayRollAnimation(Action onFinished);
        
        /// <summary>回避アニメーション（スライド）を再生する</summary>
        void PlaySlideAnimation(Action onFinished);
        
        /// <summary>回避アニメーションを中止する</summary>
        void CancelRollAnimation();
        
        /// <summary>パリィアニメーションを再生する</summary>
        void PlayParryAnimation(Action onFinished);
        
        /// <summary>パリィアニメーションを中止する</summary>
        void CancelParryAnimation();
        
        /// <summary>防御アニメーションを再生する</summary>
        void PlayGuardAnimation();
        
        /// <summary>防御時の攻撃命中アニメーションを再生する</summary>
        void PlayGuardHitAnimation(Action onFinished);

        /// <summary>通常攻撃アニメーションを再生する</summary>
        void PlayAttackNAnimation(Action onFinished);

        /// <summary>通常攻撃アニメーションを中止する</summary>
        void CancelAttackNAnimation();
        
        /// <summary>特殊攻撃アニメーションを再生する</summary>
        void PlayAttackSAnimation(Action onFinished);

        /// <summary>特殊攻撃アニメーションを中止する</summary>
        void CancelAttackSAnimation();

        /// <summary>攻撃アニメーションのループ回数と再生状態を初期化する</summary>
        void ResetAttackContext();

        /// <summary>EX攻撃アニメーションを再生する</summary>
        void PlayAttackEAnimation();
        
        /// <summary>被弾アニメーションを再生する</summary>
        void PlayDamageAnimation(Action onFinished);
        
        /// <summary>被弾アニメーションを停止する</summary>
        void CancelDamageAnimation();

        /// <summary>死亡アニメーションを再生する</summary>
        void PlayDeadAnimation();
    }
}