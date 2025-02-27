using Enum;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Enemy.AsyncNode
{
    /// <summary>非同期条件ノード</summary>
    public class AsyncConditionNode : BaseAsyncNode
    {
        /// <summary>条件</summary>
        private readonly Func<bool> _condition;

        /// <summary>コンストラクター</summary>
        public AsyncConditionNode(Func<bool> condition)
        {
            _condition = condition;
        }

        /// <summary>ノードの評価結果を返す</summary>
        /// <returns>Success = 成功, Failure = 失敗</returns>
        public override UniTask<EnemyEnum.NodeStatus> ExecuteAsync(CancellationToken token)
        {
            return UniTask.FromResult(_condition() ? EnemyEnum.NodeStatus.Success : EnemyEnum.NodeStatus.Failure);
        }
    }
}