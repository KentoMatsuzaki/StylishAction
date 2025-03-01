using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy.AI;
using UnityEngine;

namespace Enemy
{
    /// <summary>敵を制御するクラス</summary>
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Phase1AI phase1AI;
        
        private CancellationTokenSource _cts;
        private IEnemyAI _bt;

        private void Start()
        {

        }

        private void BeginBehaviourTree()
        {
            // キャンセルトークンを作成する
            _cts = new CancellationTokenSource();
            // ビヘイビアツリーを実行する
            _bt.ExecuteBehaviourTree(_cts.Token).Forget();
        }
    }
}