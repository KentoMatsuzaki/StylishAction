using Enemy;
using Enum.Enemy;

namespace Effect
{
    /// <summary>敵のパーティクルを制御するクラス</summary>
    public class EnemyParticleController : ParticleControllerBase
    {
        public EnemyEnum.EnemyAttackType type;
        
        /// <summary>敵の制御クラス</summary>
        private EnemyController _enemyController;

        protected override void Awake()
        {
            base.Awake();
            _enemyController = GetComponentInParent<EnemyController>();
        }

        public override void OnReactivated()
        {
            base.OnReactivated();
            
            switch (type)
            {
                case EnemyEnum.EnemyAttackType.Meteor :
                    transform.position = _enemyController.player.transform.position; break;
            }
        }
    }
}