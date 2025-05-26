using System;
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

        /// <summary>プレイヤーのパーティクル</summary>
        private Dictionary<ParticleEnums.PlayerParticleType, PlayerParticleController> _playerParticleDic;

        /// <summary>敵のパーティクル</summary>
        private Dictionary<ParticleEnums.EnemyParticleType, EnemyParticleController> _enemyParticleDic;
        
        public static ParticleManager Instance;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            
            _playerParticleDic = playerParticleList.ToDictionary(e => e.particleType, e => e);
            _enemyParticleDic = enemyParticleList.ToDictionary(e => e.particleType, e => e);
        }
        
        //-------------------------------------------------------------------------------
        // プレイヤーのパーティクルの処理
        //-------------------------------------------------------------------------------

        /// <summary>プレイヤーのパーティクルを有効化する</summary>
        public void ActivatePlayerParticle(ParticleEnums.PlayerParticleType particleType)
        {
            _playerParticleDic.GetValueOrDefault(particleType).Activate();
        }

        /// <summary>プレイヤーのパーティクルを無効化する</summary>
        public void DeactivatePlayerParticle(ParticleEnums.PlayerParticleType particleType)
        {
            _playerParticleDic.GetValueOrDefault(particleType).Deactivate();
        }
        
        //-------------------------------------------------------------------------------
        // 敵のパーティクルの処理
        //-------------------------------------------------------------------------------

        /// <summary>敵のパーティクルを有効化する</summary>
        public void ActivateEnemyParticle(ParticleEnums.EnemyParticleType particleType)
        {
            _enemyParticleDic.GetValueOrDefault(particleType).Activate();
        }

        /// <summary>敵のパーティクルを無効化する</summary>
        public void DeactivateEnemyParticle(ParticleEnums.EnemyParticleType particleType)
        {
            _enemyParticleDic.GetValueOrDefault(particleType).Deactivate();
        }
    }
}