namespace Effect
{
    /// <summary>
    /// ループ再生するパーティクル挙動を制御するクラス
    /// </summary>
    public class LoopParticle : ParticleBase
    {
        public override void Play()
        {
            gameObject.SetActive(true);
            
            foreach (var particle in Particles)
            {
                particle.Play();
            }
        }
    }
}