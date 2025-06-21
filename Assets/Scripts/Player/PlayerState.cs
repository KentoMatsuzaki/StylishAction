using System;
using Definitions.Enum;

namespace Player
{
    /// <summary>
    /// プレイヤーの状態を表すクラス
    /// </summary>
    public class PlayerState
    {
        public Action OnEnter;       // 状態の開始時に呼ばれるアクション
        public Action OnUpdate;      // 状態の更新時に呼ばれるアクション（Update）
        public Action OnExit;        // 状態の終了時に呼ばれるアクション
        public Action OnFixedUpdate; // 状態の更新時に呼ばれるアクション（FixedUpdate）
        
        public readonly InGameEnums.PlayerStateType StateType; // 状態の種類

        public PlayerState(InGameEnums.PlayerStateType stateType)
        {
            StateType = stateType;
        }
        
        /// <summary>各アクションを設定する</summary>
        public void SetAction(Action onEnter = null, Action onUpdate = null, Action onExit = null, Action onFixedUpdate = null)
        {
            OnEnter = onEnter;
            OnUpdate = onUpdate;
            OnExit = onExit;
            OnFixedUpdate = onFixedUpdate;
        }
    }
}