using UnityEngine;

namespace Player.Interface
{
    /// <summary>
    /// プレイヤーの移動制御に関するインターフェース
    /// </summary>
    public interface IPlayerMovementHandler
    {
        /// <summary>移動方向</summary>
        Vector3 MoveDirection { get; } 
        
        /// <summary>入力方向に基づいて移動方向を設定する</summary>
        /// <param name="inputDirection">入力方向</param>
        void SetMoveDirection(Vector2 inputDirection);
        
        /// <summary>移動方向を初期化する</summary>
        void ResetMoveDirection();
        
        /// <summary>正面方向に力を加えて移動させる</summary>
        /// <param name="moveForce">加える力の大きさ</param>
        void MoveForward(float moveForce);

        /// <summary>入力方向に力を加えて移動させる</summary>
        /// <param name="moveForce">加える力の大きさ</param>
        void MoveStrafe(float moveForce);

        /// <summary>ルートモーションによる移動を物理演算を用いて再現する</summary>
        /// <param name="deltaPosition">アニメーションによる移動の変化量</param>
        void ApplyRootMotion(Vector3 deltaPosition);
        
        /// <summary>カメラの位置を基準に入力方向へ回転させる</summary>
        /// <param name="cameraTransform">カメラの位置</param>
        void RotateTowardsCameraRelativeDir(Transform cameraTransform);

        /// <summary>敵の方向へ即座に回転させる</summary>
        void RotateTowardsEnemyInstantly();
    }
}