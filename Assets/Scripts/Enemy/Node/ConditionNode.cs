using Enum;
using System;

namespace Enemy.Nodes
{
    /// <summary>条件ノード</summary>
    public class ConditionNode : BaseNode
    {
        /// <summary>条件</summary>
        readonly Func<bool> _condition;

        /// <summary>コンストラクター</summary>
        public ConditionNode(Func<bool> condition)
        {
            _condition = condition;
        }

        /// <summary>ノードの評価結果を返す</summary>
        /// <returns>Success = 成功, Failure = 失敗</returns>
        public override EnemyEnum.NodeStatus Execute()
        {
            return _condition() ? EnemyEnum.NodeStatus.Success : EnemyEnum.NodeStatus.Failure;
        }
    }
}