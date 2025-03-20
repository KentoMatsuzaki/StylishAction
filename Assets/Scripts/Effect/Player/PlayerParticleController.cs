using Enum.Player;
using UnityEngine;

namespace Effect.Player
{
    /// <summary>プレイヤーのパーティクルを制御するクラス</summary>
    public class PlayerParticleController : MonoBehaviour
    {
        [Header("攻撃の種類"), SerializeField] public PlayerEnum.PlayerAttackType type;
    }

}