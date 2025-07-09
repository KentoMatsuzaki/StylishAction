using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Enum;

namespace Enemy.Interface
{
    /// <summary>敵のアニメーション制御に関するインターフェース</summary>
    public interface IEnemyAnimationHandler
    {
        /// <summary>移動アニメーションを再生中かどうか</summary>
        bool IsPlayingMoveAnimation { get; set; }
        
        /// <summary>指定した種類の攻撃アニメーションを再生する</summary>
        void PlayAttackAnimation(InGameEnums.EnemyAttackType type);
        
        /// <summary>待機アニメーションを再生する</summary>
        void PlayIdleAnimation();
        
        /// <summary>移動アニメーションを再生する</summary>
        void PlayMoveAnimation();
        
        /// <summary>被パリィアニメーションを再生する</summary>
        void PlayParriedAnimation();
        
        /// <summary>復帰アニメーションを再生する</summary>
        void PlayRecoveryAnimation();

        /// <summary>死亡アニメーションを再生する</summary>
        void PlayDieAnimation();

        /// <summary>アニメーションの再生完了を待機する</summary>
        UniTask WaitUntilAnimationComplete();
    }
}