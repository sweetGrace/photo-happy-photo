using System.Linq;
using UnityEngine;

namespace AI.FSM {

    public class NearbyPalCryingTrigger : FSMTrigger {
        protected override void init() {
            TriggerID = FSMTriggerID.NearbyPalCrying;
        }

        public override bool HandleTrigger(FSMBase fsm) {
            PalManager manager = fsm.GetComponent<PalManager>();
            if (manager == null) {
                Debug.LogWarning("NearbyPalCryingTrigger: PalManager not found");
                return false;
            }

            return manager.impactPals.Any(pal => pal.fsm.CurrentState == FSMStateID.PalCry);
        }
    }

}