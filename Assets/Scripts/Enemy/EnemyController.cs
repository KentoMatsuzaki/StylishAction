using System.Collections.Generic;
using Enemy.AI;
using Player;
using SO.Enemy;
using UnityEngine;

namespace Enemy
{
    /// <summary>敵を制御するクラス</summary>
    public class EnemyController : MonoBehaviour
    {
        [Header("ステータス情報のリスト"), SerializeField] private List<EnemyStatusData> statusDataList;
        public PlayerController player;
        public EnemyAIBase CurrentBehaviourTree { get; private set; }
        
        private void Start()
        {
            CurrentBehaviourTree = GetComponent<Phase1AI>();
            CurrentBehaviourTree.Initialize(player, statusDataList[0]);
            CurrentBehaviourTree.ConstructBehaviourTree();
            CurrentBehaviourTree.BeginBehaviourTree();
            Cursor.visible = false;
        }
    }
}