using AI.FSM;
using System.Linq;
using UnityEngine;

public class PalManager : MonoBehaviour {
    public ItemID[] itemIDs;

    private PalFSM fsm;

    private void Start() {
        fsm = GetComponent<PalFSM>();
    }

    public void ImpactByItem(ItemID itemID) {
        if (itemIDs.Contains(itemID)) {
            fsm.SetTrigger(FSMTriggerID.ImpactByItem);
        }
    }
}
