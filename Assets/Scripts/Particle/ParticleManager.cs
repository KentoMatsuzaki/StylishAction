using System.Collections.Generic;
using System.Linq;
using Enum;
using UnityEngine;

namespace Particle
{
    /// <summary>パーティクルを管理するクラス</summary>
    public class ParticleManager : MonoBehaviour
    {
        /// <summary>再利用するパーティクルのリスト</summary>
        [SerializeField] private List<ParticleBase> reusableParticles;

        /// <summary>再利用するパーティクルのマップ</summary>
        private Dictionary<ParticleEnums.ParticleType, ParticleBase> _reusableParticleMap;
        
        /// <summary>DeathエネミーのPhoton攻撃のパーティクル</summary>
        [SerializeField] private List<ParticleBase> photonParticles;
        
        public static ParticleManager Instance;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            
            _reusableParticleMap = reusableParticles.ToDictionary(k => k.type, v => v);
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルの処理
        //-------------------------------------------------------------------------------

        /// <summary>パーティクルを有効化する</summary>
        public void ActivateParticle(ParticleEnums.ParticleType type)
        {
            _reusableParticleMap.GetValueOrDefault(type).Activate();
        }

        /// <summary>パーティクルを無効化する</summary>
        public void DeactivateParticle(ParticleEnums.ParticleType type)
        {
            _reusableParticleMap.GetValueOrDefault(type).Deactivate();
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルの固有処理
        //-------------------------------------------------------------------------------

        public void ActivatePhotonParticles()
        {
            foreach (var photonParticle in photonParticles)
            {
                photonParticle.Activate();
            }
        }

        public void ActivateHitParticle(Vector3 particlePosition)
        {
            var hitParticle = _reusableParticleMap.GetValueOrDefault(ParticleEnums.ParticleType.Hit);
            hitParticle.transform.position = particlePosition;
            hitParticle.Activate();
        }
    }
}