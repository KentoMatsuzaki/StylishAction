using Cysharp.Threading.Tasks;
using Enemy.AI;
using Enum;
using Player;
using Sound;
using UI;
using UnityEngine;

namespace System
{
    /// <summary>ゲーム全体の状態を管理するクラス</summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private PlayerController player;
        public PlayerController Player => player;

        [SerializeField] private EnemyAIBase enemy;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
        }
        
        //-------------------------------------------------------------------------------
        // ゲームの進行に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>ゲームを開始する</summary>
        public async UniTask StartGameAsync()
        {
            SoundManager.Instance.PlayLoopSound(OutGameEnums.SoundType.Bgm);
            await enemy.Initialize();
            player.gameObject.SetActive(true);
            player.Initialize();
            enemy.StartBehaviour();
            UIManager.Instance.ShowMainUI();
        }
        
        /// <summary>ゲームをリスタートする</summary>
        public async UniTask RestartGameAsync()
        {
            SoundManager.Instance.PlayLoopSound(OutGameEnums.SoundType.Bgm);
            player.Restart();
            enemy.ResetBehaviour();
            await enemy.Initialize();
            player.gameObject.SetActive(true);
            player.Initialize();
            enemy.StartBehaviour();
            UIManager.Instance.ShowMainUI();
        }

        /// <summary>ゲームを終了する</summary>
        public void QuitGame()
        {
            # if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            # else
            Application.Quit();
            # endif
        }
    }
}