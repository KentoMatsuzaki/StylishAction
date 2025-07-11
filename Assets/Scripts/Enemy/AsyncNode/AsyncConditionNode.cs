using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Enum;

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
        public override UniTask<InGameEnums.EnemyNodeStatus> ExecuteAsync(CancellationToken token)
        {
            return UniTask.FromResult(_condition() ? 
                InGameEnums.EnemyNodeStatus.Success : InGameEnums.EnemyNodeStatus.Failure);
        }
    }
}