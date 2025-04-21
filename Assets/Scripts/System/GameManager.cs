using Enemy;
using Player;
using UnityEngine;

namespace System
{
    /// <summary>ゲーム全体の状態を管理するクラス</summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        public PlayerController Player => player;

        [SerializeField] private EnemyController enemy;

        public EnemyController Enemy => enemy;
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}