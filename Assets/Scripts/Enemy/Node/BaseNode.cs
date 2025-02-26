using Enum;

namespace Enemy.Nodes
{
    /// <summary>ノードの基底クラス</summary>
    public abstract class BaseNode
    {
        /// <summary>ノードの評価結果を返す</summary>
        /// <returns>Success = 成功, Failure = 失敗, Running = 実行中</returns>
        public abstract EnemyEnum.NodeStatus Execute();
    }
}