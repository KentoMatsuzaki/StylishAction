using System;

namespace Player
{
    /// <summary>プレイヤーの状態を表すクラス</summary>
    /// <summary>状態ごとにEnter,Update,Exit時の処理を登録する</summary>
    public class PlayerState
    {
        /// <summary>状態開始時に呼ばれる処理</summary>
        public Action OnEnter;
        
        /// <summary>毎フレーム呼ばれる処理</summary>
        public Action OnUpdate;
        
        /// <summary>状態終了時に呼ばれる処理</summary>
        public Action OnExit;
        
        public void Enter() => OnEnter?.Invoke();
        public void Update() => OnUpdate?.Invoke();
        public void Exit() => OnExit?.Invoke();
    }
}