namespace Particle
{
    /// <summary>永続的に再生され続けるパーティクルを制御するクラス</summary>
    public class PermanentParticle : ParticleBase
    {
        public override void Activate()
        {
            // ゲームオブジェクトを有効化する
            gameObject.SetActive(true);
            // 全てのパーティクルを再生する
            PlayAllParticles();
        }
    }
}