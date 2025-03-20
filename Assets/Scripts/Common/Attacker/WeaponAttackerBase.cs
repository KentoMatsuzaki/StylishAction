using UnityEngine;

namespace Common.Attacker
{
    /// <summary>武器による攻撃の基底クラス</summary>
    public abstract class WeaponAttackerBase : MonoBehaviour
    {
        private Collider _collider;
        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false; // アニメーションと同期させるため最初から非アクティブ化する
            _collider.isTrigger = true;
        }
        
        protected abstract void OnTriggerEnter(Collider other);
        public void EnableCollider() => _collider.enabled = true;
        public void DisableCollider() => _collider.enabled = false;
    }
}