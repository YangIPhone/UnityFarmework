using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChangQFramework{
    public abstract  class FSMState
    {
        public int StateID;
        public MonoBehaviour mono;
        public FSMManager fsmManager;

        public FSMState(int stateID, MonoBehaviour mono, FSMManager manager)
        {
            this.StateID = stateID;
            this.mono = mono;
            this.fsmManager = manager;
        }

        protected FSMState()
        {
        }

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}
