using UnityEngine;

namespace Player.Interface
{
    /// <summary>
    /// プレイヤーの移動を制御するインターフェース
    /// </summary>
    public interface IPlayerMovementHandler
    {
        /// <summary>入力方向に基づいて移動方向を設定する</summary>
        /// <param name="inputDirection">入力方向</param>
        void SetMoveDirection(Vector2 inputDirection);
        
        /// <summary>カメラの位置を基準に入力方向へ回転させる</summary>
        /// <param name="cameraTransform">カメラの位置</param>
        void RotateTowardsCameraRelativeDir(Transform cameraTransform);
    }
}