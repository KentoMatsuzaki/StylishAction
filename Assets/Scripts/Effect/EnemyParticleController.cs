using Enemy;
using Enum.Enemy;

namespace Effect
{
    /// <summary>敵のパーティクルを制御するクラス</summary>
    public class EnemyParticleController : ParticleControllerBase
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