using Cysharp.Threading.Tasks;
using System.Linq;
using Definitions.Enum;
using Managers;

namespace Effect
{
    /// <summary>
    /// 一度だけ再生するパーティクル挙動を制御するクラス
    /// </summary>
    public class OneShotParticle : ParticleBase
    {
        public override void Play()
        {
            // 既にアクティブである場合
            if (gameObject.activeSelf)
            {
                foreach (var particle in Particles)
                {
                    particle.Stop();
                    particle.Clear();
                }
                
                HandleCustomBehaviour();
                
                foreach (var particle in Particles)
                {
                    particle.Play();
                }
            }
            
            // 非アクティブである場合
            else
            {
                gameObject.SetActive(true);
                
                HandleCustomBehaviour();
                
                foreach (var particle in Particles)
                {
                    particle.Play();
                }
            }
            
            StopAfterPlayAllParticles().Forget();
        }

        /// <summary>
        /// 全てのパーティクルの再生完了を待ってから非アクティブにする
        /// </summary>
        private async UniTaskVoid StopAfterPlayAllParticles()
        {
            await UniTask.WaitUntil(() => Particles.All(p => !p.isPlaying));
            gameObject.SetActive(false);
        }

        /// <summary>
        /// パーティクルごとの固有処理を行う
        /// </summary>
        private void HandleCustomBehaviour()
        {
            switch (ParticleType)
            {
                case InGameEnums.ParticleType.Meteor:
                case InGameEnums.ParticleType.Eclipse:
                case InGameEnums.ParticleType.Vortex:
                    var targetPosition = GameManager.Instance.Player.transform.position;
                    targetPosition.y = 0;
                    transform.position = targetPosition; 
                    break;
            }
        }
    }
}