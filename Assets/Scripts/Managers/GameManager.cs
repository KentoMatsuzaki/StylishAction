using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public UnityEngine.Camera MainCamera { get; private set; }
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            MainCamera = UnityEngine.Camera.main;
        }
    }
}