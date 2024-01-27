using UnityEngine;

namespace AI.FSM {

    public class PalStableLaughToCryTrigger : PalRandomCryBase {
        protected override void init() {
            TriggerID = FSMTriggerID.PalStableLaughToCry;
        }

        protected override float getProbability(PalManager manager) {
            return manager.stableToCryProbability;
        }
    }

}