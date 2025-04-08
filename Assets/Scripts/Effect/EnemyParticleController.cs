using Enum.Enemy;

namespace Effect
{
    /// <summary>敵のパーティクルを制御するクラス</summary>
    public class EnemyParticleController : ParticleControllerBase
    {
        public EnemyEnum.EnemyAttackType type;

        public override void OnReactivated()
        {
            base.OnReactivated();
            
            switch (type)
            {
                case EnemyEnum.EnemyAttackType.Meteor :
                    transform.position = EnemyController.player.transform.position; break;
            }
        }
    }
}