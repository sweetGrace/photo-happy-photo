using System.Linq;
using UnityEngine;

namespace AI.FSM {

    public class NoNearbyPalCryingTrigger : FSMTrigger {
        protected override void init() {
            TriggerID = FSMTriggerID.NoNearbyPalCrying;
        }

        public override bool HandleTrigger(FSMBase fsm) {
            PalManager manager = fsm.GetComponent<PalManager>();
            if (manager == null) {
                Debug.LogWarning("NoNearbyPalCryingTrigger: PalManager not found");
                return false;
            }

            return manager.impactPals.All(pal => pal.fsm.CurrentState != FSMStateID.PalCry);
        }
    }

}