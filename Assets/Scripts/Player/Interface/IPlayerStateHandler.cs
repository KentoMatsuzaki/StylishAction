using System;
using Definitions.Enum;

namespace Player.Interface
{
    /// <summary>
    /// プレイヤーの状態を制御するインターフェース
    /// </summary>
    public interface IPlayerStateHandler
    {
        /// <summary>現在の状態</summary>
        PlayerState CurrentState { get; }
        
        /// <summary>指定した状態の各アクションを設定する</summary>
        /// <param name="stateType">状態の種類</param>
        /// <param name="onEnter">状態の開始時に呼ばれるアクション</param>
        /// <param name="onUpdate">状態の更新時に呼ばれるアクション</param>
        /// <param name="onExit">状態の終了時に呼ばれるアクション</param>
        void SetStateAction(InGameEnums.PlayerStateType stateType, Action onEnter = null, Action onUpdate = null, Action onExit = null);

        /// <summary>現在の状態を終了させて、指定した新しい状態に変更する</summary>
        /// <param name="newStateType">新しい状態の種類</param>
        void ChangeState(InGameEnums.PlayerStateType newStateType);

        /// <summary>指定した新しい状態に変更できるかどうかを判定する</summary>
        /// <param name="newStateType">新しい状態の種類</param>
        bool CanChangeState(InGameEnums.PlayerStateType newStateType);
    }
}