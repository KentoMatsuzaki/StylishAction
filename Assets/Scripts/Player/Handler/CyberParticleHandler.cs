using Definitions.Enum;
using Player.Interface;
using UnityEngine;
using ParticleManager = Managers.ParticleManager;

namespace Player.Handler
{
    /// <summary>
    /// Cyber（プレイヤー）のパーティクルを制御するクラス
    /// </summary>
    public class CyberParticleHandler : MonoBehaviour, IPlayerParticleHandler
    {
        //-------------------------------------------------------------------------------
        // 通常攻撃のパーティクル
        //-------------------------------------------------------------------------------
        public void PlayAtkN01Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.AtkN01);
        }
        
        public void PlayAtkN02Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.AtkN02);
        }
        
        public void PlayAtkN03Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.AtkN03);
        }
        
        public void PlayAtkN04Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.AtkN04);
        }
        
        public void PlayAtkN05Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.AtkN05);
        }
        
        //-------------------------------------------------------------------------------
        // 特殊攻撃のパーティクル
        //-------------------------------------------------------------------------------
        
        public void PlayAtkS01Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.AtkS01);
        }
        
        public void PlayAtkS02Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.AtkS02);
        }
        
        public void PlayAtkS03Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.AtkS03);
        }
        
        //-------------------------------------------------------------------------------
        // EX攻撃のパーティクル
        //-------------------------------------------------------------------------------
        
        public void PlayAtkE01Particle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.AtkE01);
        }

        public void StopAtkE01Particle()
        {
            ParticleManager.Instance.StopParticle(InGameEnums.ParticleType.AtkE01);
        }
        
        //-------------------------------------------------------------------------------
        // 汎用パーティクル
        //-------------------------------------------------------------------------------

        public void PlayParryParticle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.Parry);
        }

        public void PlayGuardParticle()
        {
            ParticleManager.Instance.PlayParticle(InGameEnums.ParticleType.Guard);
        }

        public void StopGuardParticle()
        {
            ParticleManager.Instance.StopParticle(InGameEnums.ParticleType.Guard);
        }

        public void PlayHitParticle(Vector3 position)
        {
            ParticleManager.Instance.PlayHitParticle(position);
        }
    }
}