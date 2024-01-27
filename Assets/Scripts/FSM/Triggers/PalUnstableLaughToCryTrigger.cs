using UnityEngine;

namespace AI.FSM {

    public class PalUnstableLaughToCryTrigger : PalRandomCryBase {
        protected override void init() {
            TriggerID = FSMTriggerID.PalUnstableLaughToCry;
        }

        protected override float getProbability(PalManager manager) {
            return manager.unstableToCryProbability;
        }
    }

}