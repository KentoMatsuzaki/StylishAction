using System;
using System.Threading;
using Enemy.AsyncNode;
using Cysharp.Threading.Tasks;
using Enemy.Handler;
using Enemy.Phase;
using Enum;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.AI
{
    /// <summary>敵のAIを制御するクラス</summary>
    public class Phase1AI : IEnemyAI
    {
        /// <summary>メインノード</summary>
        private BaseAsyncNode _mainNode;
        /// <summary>アニメーション</summary>
        private readonly EnemyAnimationHandler _animationHandler;
        /// <summary>スキル1の攻撃クラス</summary>
        private EnemyAttackHandler _skill1AttackHandler;
        /// <summary>スキル2の攻撃クラス</summary>
        private EnemyAttackHandler _skill2AttackHandler;

        /// <summary>コンストラクタ</summary>
        public Phase1AI(EnemyAnimationHandler animationHandler,
            EnemyAttackHandler skill1AttackHandler, EnemyAttackHandler skill2AttackHander)
        {
            _animationHandler = animationHandler;
            _skill1AttackHandler = skill1AttackHandler;
            _skill2AttackHandler = skill2AttackHander;
        }

        private void Start()
        {
            var attack = new AsyncActionNode(async (token) =>
            {
                await _animationHandler.WaitForAnimationEnd(token);
                Debug.Log("Test");
                return EnemyEnum.NodeStatus.Success;
            });

            var sequence = new AsyncSequenceNode();
            sequence.AddNode(attack);
            _mainNode = sequence;
        }

        /// <summary>ビヘイビアツリーを実行する</summary>
        public async UniTask ExecuteBehaviourTree(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await _mainNode.ExecuteAsync(token);
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Behaviour Tree Canceled");
            }
        }

        /// <summary>ビヘイビアツリーを構築する</summary>
        public AsyncSelectorNode ConstructBehaviourTree()
        {
            return new AsyncSelectorNode();
        }

        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃シーケンスを構築する</summary>
        public AsyncSequenceNode ConstructAttackSequence()
        {
            var attackAction = new AsyncActionNode(async (token) =>
            {
                // 攻撃アニメーションのトリガーを設定する
                _animationHandler.TriggerAttack(Random.Range(1,3));
                
                // 攻撃アニメーションの再生終了を待機する
                await _animationHandler.WaitForAnimationEnd(token);

                // ノードの評価結果を返す
                return EnemyEnum.NodeStatus.Success;
            });
            
            var attackSequence = new AsyncSequenceNode();
            return attackSequence;
        }
        
    }
}