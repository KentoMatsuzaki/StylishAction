using Definitions.Enum;
using Enemy.Interface;
using Managers;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>Death（敵）のパーティクルを制御するクラス</summary>
    public class DeathParticleHandler : MonoBehaviour, IEnemyParticleHandler
    {
        //-------------------------------------------------------------------------------
        // アニメーションイベント
        //-------------------------------------------------------------------------------

        public void PlayMeteorParticle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.Meteor);
        }

        public void PlaySeraph1Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.Seraph1);
        }
        
        public void PlaySeraph2Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.Seraph2);
        }

        public void PlayEclipseParticle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.Eclipse);
        }

        public void PlayVortexParticle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.Vortex);
        }

        public void PlayPhotonParticle()
        {
            ParticleManager.Instance.PlayPhotonParticle();
        }

        public void PlayExplosionParticle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.Explosion);
        }

        public void PlayWaterfall1Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.WaterFall1);
        }
        
        public void PlayWaterfall2Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.WaterFall2);
        }
        
        //-------------------------------------------------------------------------------
        // 汎用パーティクル
        //-------------------------------------------------------------------------------

        public void PlayOnParriedParticle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.Parried);
        }

        public void StopOnParriedParticle()
        {
            ParticleManager.Instance.StopParticle(InGameEnums.ParticleType.Parried);
        }

        public void PlayHitParticle(Vector3 position)
        {
            ParticleManager.Instance.PlayHitParticle(position);
        }
    }
}