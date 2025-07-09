using System;
using Cysharp.Threading.Tasks;
using Definitions.Enum;
using Enemy.AsyncNode;
using Managers;
using UnityEngine;

namespace Enemy.AI
{
    /// <summary>
    /// Death（敵）のAI挙動を制御する派生クラス
    /// EnemyAIBaseを継承し、Death固有の処理を実装する
    /// </summary>
    public class DeathAI : EnemyAIBase
    {
        //-------------------------------------------------------------------------------
        // BehaviourTreeに関する処理
        //-------------------------------------------------------------------------------

        protected override void BuildBehaviourTree()
        {
            RootNode.AddNode(ConstructAttackSequence());
            RootNode.AddNode(ConstructMoveSequence());
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃シーケンス
        //-------------------------------------------------------------------------------

        /// <summary>攻撃シーケンスを構築する</summary>
        private AsyncSequenceNode ConstructAttackSequence()
        {
            var attackSequence = new AsyncSequenceNode();
            attackSequence.AddNode(new AsyncConditionNode(AttackHandler.CanAttackPlayer));
            attackSequence.AddNode(ConstructAttackAction());
            return attackSequence;
        }

        /// <summary>攻撃アクションを構築する</summary>
        private AsyncActionNode ConstructAttackAction()
        {
            return new AsyncActionNode(async (token) =>
            {
                AnimationHandler.IsPlayingMoveAnimation = false;
                AnimationHandler.PlayAttackAnimation(AttackHandler.CurrentAttackStats.attackType);
                await UniTask.Delay(TimeSpan.FromSeconds(AttackHandler.CurrentAttackStats.cooldown), cancellationToken: token);
                AttackHandler.SetAttackStats();
                return InGameEnums.EnemyNodeStatus.Success;
            });
        }
        
        //-------------------------------------------------------------------------------
        // 移動シーケンス
        //-------------------------------------------------------------------------------

        /// <summary>移動シーケンスを構築する</summary>
        private AsyncSequenceNode ConstructMoveSequence()
        {
            var moveSequence = new AsyncSequenceNode();
            moveSequence.AddNode(ConstructMoveAction());
            return moveSequence;
        }

        /// <summary>移動アクションを構築する</summary>
        private AsyncActionNode ConstructMoveAction()
        {
            return new AsyncActionNode(async (_) =>
            {
                if (!AnimationHandler.IsPlayingMoveAnimation) 
                    AnimationHandler.PlayMoveAnimation();

                if (!AttackHandler.IsPlayerInAngle())
                {
                    MovementHandler.RotateTowardsPlayer(BaseStats.rotateSpeed);
                }
                
                if (AttackHandler.IsPlayerTooFar())
                {
                    MovementHandler.MoveTowardsPlayer(BaseStats.moveForce);
                }

                if (AttackHandler.IsPlayerTooClose())
                {
                    MovementHandler.MoveAwayFromPlayer(BaseStats.moveForce);
                }
                
                return InGameEnums.EnemyNodeStatus.Running;
            });
        }
        
        //-------------------------------------------------------------------------------
        // プレイヤーの攻撃が命中した際の処理
        //-------------------------------------------------------------------------------

        public override void OnHit(float damage, Vector3 hitPosition)
        {
            // 被弾時のエフェクトを表示する
            ParticleHandler.PlayHitParticle(hitPosition);
            // 被弾時のサウンドを再生する
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.EnemyHit);
            // ダメージを適用する
            ApplyDamage(damage);
        }

        protected override void OnDamage()
        {
            // プレイヤーの方向へ即座に回転させる
            //MovementHandler.RotateTowardsPlayerInstantly();
            // ノックバックさせる
            //MovementHandler.ApplyKnockBack(BaseStats.knockBackForce);
        }

        protected override void OnDie()
        {
            // BehaviourTreeを停止させる
            Cts?.Cancel();
            // 死亡アニメーションを再生する
            AnimationHandler.PlayDieAnimation();
        }
        
        //-------------------------------------------------------------------------------
        // プレイヤーにパリィされた際の処理
        //-------------------------------------------------------------------------------

        public override async void OnParried()
        {
            // BehaviourTreeを停止させる
            Cts?.Cancel();
            // 被パリィ時のエフェクトを表示する
            ParticleHandler.PlayOnParriedParticle();
            // 被パリィアニメーションを再生する
            AnimationHandler.PlayParriedAnimation();
            // 停止時間だけ待機させる
            await UniTask.Delay(TimeSpan.FromSeconds(BaseStats.parriedDuration));
            // 復帰アニメーションを再生する
            AnimationHandler.PlayRecoveryAnimation();
            // 被パリィ時のエフェクトを停止する
            ParticleHandler.StopOnParriedParticle();
            // 復帰アニメーションの再生完了を待機する
            await AnimationHandler.WaitUntilAnimationComplete();
            // BehaviourTreeを再開する
            StartBehaviourTree();
        }
    }
}