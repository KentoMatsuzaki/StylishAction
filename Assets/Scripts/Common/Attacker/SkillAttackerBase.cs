using UnityEngine;

namespace Common.Attacker
{
    /// <summary>スキルによる攻撃の基底クラス</summary>
    public abstract class SkillAttackerBase : MonoBehaviour
    {
        private Collider _collider;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = true; // エフェクトと同期させるため最初からアクティブ化する
            _collider.isTrigger = true;
        }

        protected abstract void OnTriggerEnter(Collider other);
    }
}