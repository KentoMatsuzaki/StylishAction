using Definitions.Data;
using Enemy.AI;
using Player.Controller;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public UnityEngine.Camera MainCamera { get; private set; }

        [SerializeField] private PlayerControllerBase player;
        public PlayerControllerBase Player => player;

        [SerializeField] private EnemyAIBase enemy;

        [SerializeField] private EnemyBaseStats enemyBaseStats;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            MainCamera = UnityEngine.Camera.main;
        }

        private void Start()
        {
            enemy.Initialize(enemyBaseStats);
        }
    }
}