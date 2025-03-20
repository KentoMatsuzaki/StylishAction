using Enum.Enemy;
using UnityEngine;

namespace Effect.Enemy
{
    /// <summary>敵のパーティクルを制御するクラス</summary>
    public class EnemyParticleController : ParticleControllerBase
    {
        [Header("スキルの種類"), SerializeField] public EnemyEnum.EnemySkillType type;
    }
}