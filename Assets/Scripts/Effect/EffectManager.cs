using System.Collections.Generic;
using System.Linq;
using Enum.Enemy;
using Enum.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Effect
{
    /// <summary>エフェクトを管理するクラス</summary>
    public class EffectManager : MonoBehaviour
    {
        [Header("プレイヤーの攻撃エフェクトのリスト"), SerializeField]
        private List<PlayerParticleController> playerAttackEffectList;

        [Header("敵の攻撃エフェクトのリスト"), SerializeField] 
        private List<EnemyParticleController> enemyAttackEffectList;
        
        [Header("プレイヤーのパリィエフェクト"), SerializeField]
        private PlayerParticleController playerParryEffect;

        /// <summary>プレイヤーの攻撃エフェクトの辞書</summary>
        private Dictionary<PlayerEnum.PlayerParticleType, PlayerParticleController> _playerAttackEffectDic;

        /// <summary>敵の攻撃エフェクトの辞書</summary>
        private Dictionary<EnemyEnum.EnemyAttackType, EnemyParticleController> _enemyAttackEffectDic;
        
        public static EffectManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            _playerAttackEffectDic = playerAttackEffectList.ToDictionary(e => e.type, e => e);
            _enemyAttackEffectDic = enemyAttackEffectList.ToDictionary(e => e.type, e => e);
        }

        /// <summary>プレイヤーの攻撃エフェクトを有効化する</summary>
        /// <param name="type">攻撃の種類</param>
        public void ActivatePlayerAttackEffect(PlayerEnum.PlayerParticleType type)
        {
            if (_playerAttackEffectDic.TryGetValue(type, out var effect))
            {
                // 既にパーティクルが有効化されている場合は
                if (effect.gameObject.activeSelf)
                {
                    effect.RestartParticles();
                }
                else
                {
                    effect.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.LogWarning($"Attack Effect Not Found : {type}");
            }
        }

        /// <summary>敵の攻撃エフェクトを有効化する</summary>
        /// <param name="type">攻撃の種類</param>
        public void ActivateEnemyAttackEffect(EnemyEnum.EnemyAttackType type)
        {
            if (_enemyAttackEffectDic.TryGetValue(type, out var effect))
            {
                if (effect.gameObject.activeSelf)
                {
                    effect.RestartParticles();
                }
                else
                {
                    effect.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.LogWarning($"Attack Effect Not Found : {type}");
            }
        }

        /// <summary>プレイヤーのパリィエフェクトを有効化する</summary>
        public void ActivatePlayerParryEffect()
        {
            if (playerParryEffect.gameObject.activeSelf)
            {
                playerParryEffect.RestartParticles();
            }
            else
            {
                playerParryEffect.gameObject.SetActive(true);
            }
        }
        
    }
}