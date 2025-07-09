using System.Collections.Generic;
using System.Linq;
using Definitions.Enum;
using Effect;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// パーティクル全体の管理を行うクラス
    /// </summary>
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance;
        
        [SerializeField] private List<ParticleBase> particles;
        private Dictionary<InGameEnums.ParticleType, ParticleBase> _particleMap;
        
        /// <summary>Death（敵）の Photon 攻撃のパーティクル</summary>
        [SerializeField] private List<ParticleBase> photonParticles;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            
            _particleMap = particles.ToDictionary(k => k.ParticleType, v => v);
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルの汎用処理
        //-------------------------------------------------------------------------------

        public void PlayParticle(InGameEnums.ParticleType particleType)
        {
            _particleMap[particleType].Play();
        }

        public void StopParticle(InGameEnums.ParticleType particleType)
        {
            _particleMap[particleType].Stop();
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルの固有処理
        //-------------------------------------------------------------------------------

        /// <summary>
        /// Death（敵）の Photon 攻撃のパーティクルを再生する
        /// </summary>
        public void PlayPhotonParticle()
        {
            foreach (var photonParticle in photonParticles)
            {
                photonParticle.Play();
            }
        }

        /// <summary>
        /// ヒット演出のパーティクルを生成して再生する
        /// </summary>
        /// <param name="position">攻撃がヒットしたワールド座標</param>
        public void PlayHitParticle(Vector3 position)
        {
            var hitParticle = Instantiate(_particleMap[InGameEnums.ParticleType.Hit], position, Quaternion.identity, transform);
            hitParticle.Play();
        }
    }
}