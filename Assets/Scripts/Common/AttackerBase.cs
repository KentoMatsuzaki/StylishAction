using UnityEngine;

namespace Common
{
    /// <summary>
    /// プレイヤーと敵に共通する攻撃判定を制御する基底クラス
    /// 共通の制御ロジックを提供し、各種攻撃クラスが継承して実装する
    /// </summary>
    public abstract class AttackerBase : MonoBehaviour
    {
        protected Collider Collider;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            Collider = GetComponent<Collider>();
            Collider.isTrigger = true;
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃判定に関する処理
        //-------------------------------------------------------------------------------

        public void EnableCollider()
        {
            Collider.enabled = true;
        }

        public void DisableCollider()
        {
            Collider.enabled = false;
        }
        
        protected abstract void OnTriggerEnter(Collider other);
    }
}