using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy.AsyncNode;

namespace Enemy.AI
{
    /// <summary>敵AIのインターフェース</summary>
    public interface IEnemyAI
    {
        /// <summary>ビヘイビアツリーを実行する</summary>
        UniTask ExecuteAsync(CancellationToken token);

        /// <summary>ビヘイビアツリーを構築する</summary>
        AsyncSelectorNode ConstructBehaviourTree();
    }
}