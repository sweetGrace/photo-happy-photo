namespace AI.FSM {
    /// <summary>
    /// <para>有限状态机条件类</para>
    /// <para>子类命名规范为: AI.FSM.\<FSMTriggerID\>Trigger</para>
    /// </summary>
    public abstract class FSMTrigger {
        public FSMTrigger() {
            Init();
        }

        /// <summary>
        /// 处理条件
        /// </summary>
        /// <returns>满足条件返回true, 否则返回false</returns>
        public abstract bool HandleTrigger(FSMBase fsm);

        /// <summary>
        /// 子类必须初始化TriggerID
        /// </summary>
        protected abstract void Init();

        public FSMTriggerID TriggerID { get; set; }
    }

}