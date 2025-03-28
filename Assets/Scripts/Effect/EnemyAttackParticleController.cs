using Enemy;
using Enum.Enemy;

namespace Effect
{
    /// <summary>敵の攻撃パーティクルを制御するクラス</summary>
    public class EnemyAttackParticleController : ParticleControllerBase
    {
        public EnemyEnum.EnemyAttackType type;

        private void Start()
        {
            switch (type)
            {
                case EnemyEnum.EnemyAttackType.Meteor :
                    transform.position = GetComponentInParent<EnemyController>().player.transform.position; break;
                default: break;
            }
        }
    }
}