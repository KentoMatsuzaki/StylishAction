using System.Threading;
using Enemy.AsyncNode;
using Cysharp.Threading.Tasks;
using Enemy.Handler;
using Enum;
using UnityEngine;

namespace Enemy.AI
{
    /// <summary>敵のAIを制御するクラス</summary>
    public class Phase1AI : MonoBehaviour, IEnemyAI
    {
        private readonly CancellationTokenSource _cts = new();
        private BaseAsyncNode _rootNode;
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
            _rootNode = sequence;

            ExecuteAsync(_cts.Token).Forget();
        }

        /// <summary>ビヘイビアツリーを実行する</summary>
        public async UniTask ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var result = await _rootNode.ExecuteAsync(token);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
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
        
    }
}