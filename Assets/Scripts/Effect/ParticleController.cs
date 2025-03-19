using UnityEngine;

namespace Effect
{
    /// <summary>パーティクルの再生を制御するクラス</summary>
    public class ParticleController : MonoBehaviour
    {
        private ParticleSystem[] _particles;

        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
        }

        private void Update()
        {
            var allStopped = true;

            foreach (var particle in _particles)
            {
                if (particle.isPlaying)
                {
                    allStopped = false; break;
                }
            }
            
            if (allStopped) gameObject.SetActive(false);
        }
    }
}
