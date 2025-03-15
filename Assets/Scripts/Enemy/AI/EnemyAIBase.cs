using Cysharp.Threading.Tasks;
using Enemy.AsyncNode;
using Player;
using UnityEngine;

namespace Enemy.AI
{
    /// <summary>敵AIの基底クラス</summary>
    public abstract class EnemyAIBase : MonoBehaviour
    {
        /// <summary>ビヘイビアツリーを開始する</summary>
        public abstract void BeginBehaviourTree();

        /// <summary>ビヘイビアツリーを実行する</summary>
        protected abstract UniTask ExecuteBehaviourTree();

        /// <summary>ビヘイビアツリーを構築する</summary>
        protected abstract void ConstructBehaviourTree();

        /// <summary>攻撃シーケンスを構築する</summary>
        protected abstract AsyncSequenceNode ConstructAttackSequence();

        /// <summary>プレイヤーに攻撃が命中した時の処理</summary>
        public abstract void OnHitPlayer(PlayerController player);
        
        /// <summary>プレイヤーを設定する</summary>
        public abstract void SetPlayer(PlayerController player);

        /// <summary>ステータスを設定する</summary>
        protected abstract void SetStatus();
    }
}