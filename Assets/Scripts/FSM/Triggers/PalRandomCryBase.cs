using UnityEngine;

namespace AI.FSM {

    public abstract class PalRandomCryBase : UpdateByInterval {
        protected abstract float getProbability(PalManager manager);

        protected override bool Update(FSMBase fsm) {
            PalManager manager = fsm.GetComponent<PalManager>();
            if (manager == null) {
                Debug.LogWarning("PalRandomCryBase.HandleTrigger: manager is null");
                return false;
            }
            return Random.value < getProbability(manager);
        }
    }

}