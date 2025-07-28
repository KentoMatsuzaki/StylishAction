using Definitions.Enum;
using Managers;
using Player.Attack;
using UnityEngine;
using Player.Interface;

namespace Player.Handler
{
    /// <summary>
    /// Cyber（プレイヤー）の攻撃を制御するクラス
    /// </summary>
    public class CyberAttackHandler : MonoBehaviour, IPlayerAttackHandler
    {
        [SerializeField] private PlayerAttacker atkSAttacker;
        [SerializeField] private PlayerAttacker atkEAttacker;
        
        //-------------------------------------------------------------------------------
        // 攻撃制御に関する処理
        //-------------------------------------------------------------------------------

        public void RotateTowardsEnemyInstantly()
        {
            var enemyPos = GameManager.Instance.Enemy.transform.position;
            var dir = new Vector3(enemyPos.x, 0, enemyPos.z) - new Vector3(transform.position.x,
                0, transform.position.z);
            transform.rotation = Quaternion.LookRotation(dir);
        }

        //-------------------------------------------------------------------------------
        // アニメーションイベント
        //-------------------------------------------------------------------------------

        public void EnableAtkSAttacker()
        {
            atkSAttacker.EnableCollider();
        }

        public void DisableAtkSAttacker()
        {
            atkSAttacker.DisableCollider();
        }

        public void EnableAtkEAttacker()
        {
            atkEAttacker.EnableCollider();
        }

        public void DisableAtkEAttacker()
        {
            atkEAttacker.DisableCollider();
        }

        public void PlayAtkNSound()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.AttackN);
        }

        public void PlayAtkSSound1()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.AttackS1);
        }
        
        public void PlayAtkSSound2()
        {
            SoundManager.Instance.PlaySe(OutGameEnums.SoundType.AttackS2);
        }
    }
}