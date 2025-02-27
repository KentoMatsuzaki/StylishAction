using System.Threading;
using Enemy.AsyncNodes;
using Cysharp.Threading.Tasks;
using Enemy.Handler;
using Enum;
using UnityEngine;

namespace Enemy
{
    /// <summary>敵のAIを制御するクラス</summary>
    public class EnemyAI : MonoBehaviour
    {
        private CancellationTokenSource _cts = new();
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

            RunAI(_cts.Token).Forget();
        }

        private async UniTask RunAI(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var result = await _rootNode.ExecuteAsync(token);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------


    }
}