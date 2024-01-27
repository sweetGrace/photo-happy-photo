using AI.FSM;
using System.Linq;
using UnityEngine;

public class PalManager : MonoBehaviour {
    public PalType palType;
    public ItemID[] impactItemIDs;
    public PalType[] impactPalTypes;
    public float impactRange;
    [Range(0, 1)]
    public float stableToCryProbability;
    [Range(0, 1)]
    public float unstableToCryProbability;

    public PalFSM fsm { get; private set; }
    public PalManager[] impactPals { get; private set; }

    private void Start() {
        fsm = GetComponent<PalFSM>();
    }

    public void ImpactByItem(ItemID itemID) {
        if (impactItemIDs.Contains(itemID)) {
            fsm.SetTrigger(FSMTriggerID.ImpactByItem);
        }
    }

    // should be called after all PalManager are initialized
    public void UpdateNearbyPals() {
        impactPals = Physics.OverlapSphere(transform.position, impactRange, LayerMask.GetMask("Pal"))
            .Select(collider => collider.GetComponent<PalManager>())
            .Where(manager => manager != null && impactPalTypes.Contains(manager.palType))
            .ToArray();
    }
}
