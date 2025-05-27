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
        /// <summary>攻撃パーティクルのリスト</summary>
        [SerializeField] private List<AttackParticleBase> attackParticles;

        /// <summary>攻撃パーティクルの辞書</summary>
        private Dictionary<ParticleEnums.ParticleAttackType, AttackParticleBase> _attackParticleDic;
        
        public static ParticleManager Instance;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            
            _attackParticleDic = attackParticles.ToDictionary(k => k.attackType, v => v);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃パーティクルの処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃パーティクルを有効化する</summary>
        public void ActivateAttackParticle(ParticleEnums.ParticleAttackType attackType)
        {
            _attackParticleDic.GetValueOrDefault(attackType).Activate();
        }

        /// <summary>攻撃パーティクルを無効化する</summary>
        public void DeactivateAttackParticle(ParticleEnums.ParticleAttackType attackType)
        {
            _attackParticleDic.GetValueOrDefault(attackType).Deactivate();
        }
    }
}