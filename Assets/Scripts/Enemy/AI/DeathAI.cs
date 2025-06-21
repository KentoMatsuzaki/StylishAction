using System;
using Cysharp.Threading.Tasks;
using Definitions.Enum;
using Enemy.AsyncNode;

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
            return new AsyncActionNode(async (token) =>
            {
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
    }
}