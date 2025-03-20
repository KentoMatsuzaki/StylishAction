using System.Collections.Generic;
using System.Linq;
using Effect.Enemy;
using Effect.Player;
using Enum.Enemy;
using Enum.Player;
using UnityEngine;

namespace Effect
{
    /// <summary>エフェクトを管理するクラス</summary>
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        [Header("プレイヤーの攻撃エフェクト"), SerializeField]
        private List<PlayerParticleController> playerAttackEffectList;

        [Header("敵の攻撃エフェクト"), SerializeField] 
        private List<EnemyParticleController> enemyAttackEffectList;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        /// <summary>プレイヤーの攻撃エフェクトを有効化する</summary>
        /// <param name="type">攻撃の種類</param>
        public void PlayPlayerAttackEffect(PlayerEnum.PlayerAttackType type)
        {
            playerAttackEffectList.FirstOrDefault(effect => effect.type == type)?.gameObject.SetActive(true);
        }

        /// <summary>敵の攻撃エフェクトを有効化する</summary>
        /// <param name="type">スキルの種類</param>
        public void PlayEnemyAttackEffect(EnemyEnum.EnemySkillType type)
        {
            enemyAttackEffectList.FirstOrDefault(effect => effect.type == type)?.gameObject.SetActive(true);
        }
    }

}