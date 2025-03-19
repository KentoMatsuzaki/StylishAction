using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    /// <summary>エフェクトを管理するクラス</summary>
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        [Header("プレイヤーの攻撃エフェクト"), SerializeField]
        private List<ParticleSystem> playerAttackEffects;

        [Header("敵の攻撃エフェクト"), SerializeField] 
        private List<ParticleSystem> enemyAttackEffects;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        /// <summary>エフェクトを生成する汎用メソッド</summary>
        private void InstantiateEffectPrefab(ParticleSystem particle, Vector3 position, Quaternion rotation)
        {
            Instantiate(particle, position, rotation);
        }

        /// <summary>プレイヤーの攻撃エフェクトを生成する</summary>
        /// <param name="attackNumber">攻撃の番号(1~)</param>
        /// <param name="position">生成する座標</param>
        /// <param name="rotation">プレハブの回転</param>
        public void InstantiatePlayerAttackEffect(int attackNumber, Vector3 position, Quaternion rotation)
        {
            if (attackNumber < 1 || attackNumber > playerAttackEffects.Count) return;
            InstantiateEffectPrefab(playerAttackEffects[attackNumber - 1], position, rotation);
        }

        /// <summary>敵の攻撃エフェクトを生成する</summary>
        /// <param name="skillNumber">スキルの番号(2~)</param>
        /// <param name="position">生成する座標</param>
        /// <param name="rotation">プレハブの回転</param>
        public void InstantiateEnemyAttackEffect(int skillNumber, Vector3 position, Quaternion rotation)
        {
            if (skillNumber < 2 || skillNumber > enemyAttackEffects.Count + 1) return;
            InstantiateEffectPrefab(enemyAttackEffects[skillNumber - 2], position, rotation);
            
            Debug.Log("a");
        }
    }

}