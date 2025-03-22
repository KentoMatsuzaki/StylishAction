using UnityEngine;

namespace Common.Attacker
{
    /// <summary>攻撃の基底クラス</summary>
    public abstract class AttackerBase : MonoBehaviour
    {
        private Collider _collider;

        /// <summary>コライダーを最初から有効化するか</summary>
        public bool isColliderActivateOnStart;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            _collider.enabled = isColliderActivateOnStart;
        }
        
        protected abstract void OnTriggerEnter(Collider other);
        public void EnableCollider() => _collider.enabled = true;
        public void DisableCollider() => _collider.enabled = false;
    }
}