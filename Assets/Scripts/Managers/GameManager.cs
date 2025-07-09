using System;
using Cysharp.Threading.Tasks;
using Definitions.Data;
using Definitions.Enum;
using Enemy.AI;
using Player.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public UnityEngine.Camera MainCamera { get; private set; }

        [SerializeField] private PlayerControllerBase player;
        public PlayerControllerBase Player => player;

        [SerializeField] private EnemyAIBase enemy;

        public EnemyAIBase Enemy => enemy;
        
        [SerializeField] private PlayerBaseStats playerBaseStats;

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
            SoundManager.Instance.PlayBGM();
        }
        
        //-------------------------------------------------------------------------------
        // ゲームの進行に関する処理
        //-------------------------------------------------------------------------------

        public void StartGame()
        {
            player.gameObject.SetActive(true);
            player.Initialize(playerBaseStats);
            enemy.gameObject.SetActive(true);
            enemy.Initialize(enemyBaseStats);
            UIManager.Instance.Initialize();
            UIManager.Instance.ShowMainUI();
            Cursor.visible = false;
        }

        public void QuitGame()
        {
            # if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            # else
            Application.Quit();
            # endif
        }

        private void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public async void OnGameClear()
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.Clear);
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            ResetGame();
        }

        public async void OnGameOver()
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.GameOver);
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            ResetGame();
        }
    }
}