using System.Threading;
using Enemy.AI;
using Enemy.Handler;
using Player;
using UnityEngine;

namespace Enemy
{
    /// <summary>敵を制御するクラス</summary>
    public class EnemyController : MonoBehaviour
    {
        private EnemyAIBase _phase1AI;
        
        private CancellationTokenSource _cts;
        public EnemyAIBase Bt { get; private set; }
        

        private void Start()
        {
            _phase1AI = GetComponent<EnemyAIBase>();
        }

        /// <summary>ビヘイビアツリーを開始する</summary>
        private void BeginBehaviourTree() => Bt.BeginBehaviourTree();
    }
}