using Enemy.AI;
using Player;
using UnityEngine;

namespace Enemy
{
    /// <summary>敵を制御するクラス</summary>
    public class EnemyController : MonoBehaviour
    {
        [Header("プレイヤー"), SerializeField] private PlayerController player;
        public EnemyAIBase Bt { get; private set; }
        
        private void Start()
        {
            Bt = GetComponent<Phase1AI>();
            Bt.SetPlayer(player);
            Bt.BeginBehaviourTree();
        }
    }
}