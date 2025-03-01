using System;
using System.Threading;
using Enemy.AsyncNode;
using Cysharp.Threading.Tasks;
using Enemy.Handler;
using Enum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.AI
{
    /// <summary>敵のAIを制御するクラス</summary>
    public class Phase1AI : MonoBehaviour, IEnemyAI
    {
        private readonly CancellationTokenSource _cts = new();
        private BaseAsyncNode _mainNode;
        private EnemyAnimationHandler _animationHandler;

        private void Start()
        {
            _animationHandler = GetComponent<EnemyAnimationHandler>();
            
            var attack = new AsyncActionNode(async (token) =>
            {
                await _animationHandler.WaitForAnimationEnd(token);
                Debug.Log("Test");
                return EnemyEnum.NodeStatus.Success;
            });

            var sequence = new AsyncSequenceNode();
            sequence.AddNode(attack);
            _mainNode = sequence;

            RunBehaviourTree(_cts.Token).Forget();
        }

        /// <summary>ビヘイビアツリーを実行する</summary>
        public async UniTask RunBehaviourTree(CancellationToken token)
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
        }
        
    }
}