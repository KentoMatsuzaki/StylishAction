using System;
using Definitions.Enum;

namespace Enemy.Interface
{
    /// <summary>敵のアニメーション制御に関するインターフェース</summary>
    public interface IEnemyAnimationHandler
    {
        /// <summary>指定した種類の攻撃アニメーションを再生する</summary>
        void PlayAttackAnimation(InGameEnums.EnemyAttackType type);
        
        /// <summary>待機アニメーションを再生する</summary>
        void PlayIdleAnimation();
        
        /// <summary>移動アニメーションを再生する</summary>
        void PlayMoveAnimation();
    }
}