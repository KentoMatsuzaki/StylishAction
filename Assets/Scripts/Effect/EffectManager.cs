using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    /// <summary>エフェクトを管理するクラス</summary>
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        [Header("プレイヤーの攻撃エフェクト"), SerializeField]
        private List<ParticleSystem> playerAttackEffect;

        [Header("敵の攻撃エフェクト"), SerializeField] 
        private List<ParticleSystem> enemyAttackEffect;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
    }

}