using System.Collections.Generic;
using System.Linq;
using Enum;
using UnityEngine;

namespace Particle
{
    /// <summary>パーティクルを管理するクラス</summary>
    public class ParticleManager : MonoBehaviour
    {
        [Header("プレイヤーのパーティクルのリスト"), SerializeField]
        private List<PlayerParticleController> playerParticleList;

        [Header("敵の攻撃エフェクトのリスト"), SerializeField] 
        private List<EnemyParticleController> enemyParticleList;

        /// <summary>プレイヤーのパーティクルのディクショナリー</summary>
        private Dictionary<InGameEnum.PlayerParticleType, PlayerParticleController> _playerParticleDic;

        /// <summary>敵のパーティクルのディクショナリー</summary>
        private Dictionary<InGameEnum.EnemyParticleType, EnemyParticleController> _enemyParticleDic;
        
        public static ParticleManager Instance;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            _playerParticleDic = playerParticleList.ToDictionary(e => e.playerParticleType, e => e);
            _enemyParticleDic = enemyParticleList.ToDictionary(e => e.enemyParticleType, e => e);
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルの処理
        //-------------------------------------------------------------------------------

        /// <summary>プレイヤーのパーティクルを有効化する</summary>
        public void ActivatePlayerParticle(InGameEnum.PlayerParticleType type)
        {
            if (_playerParticleDic.TryGetValue(type, out var particle))
            {
                particle.Activate();
            }
            else
            {
                Debug.LogWarning($"Particle not found : {type}");
            }
        }

        /// <summary>敵のパーティクルを有効化する</summary>
        public void ActivateEnemyParticle(InGameEnum.EnemyParticleType type)
        {
            if (_enemyParticleDic.TryGetValue(type, out var particle))
            {
                particle.Activate();
            }
            else
            {
                Debug.LogWarning($"Particle not found : {type}");
            }
        }
    }
}