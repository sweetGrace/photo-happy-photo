using AI.FSM;
using UnityEngine;

public class PalFSM : FSMBase {
    [Range(0, 1)]
    public float startCryProbability;
    protected override void SetUpFSM() {
        base.SetUpFSM();

        PalStableLaughState palStableLaughState = new PalStableLaughState();
        palStableLaughState.AddMap(FSMTriggerID.PalStableLaughToCry, FSMStateID.PalCry);
        palStableLaughState.AddMap(FSMTriggerID.NearbyPalCrying, FSMStateID.PalUnstableLaugh);
        _states.Add(palStableLaughState);

        PalUnstableLaughState palUnstableLaughState = new PalUnstableLaughState();
        palUnstableLaughState.AddMap(FSMTriggerID.PalUnstableLaughToCry, FSMStateID.PalCry);
        palUnstableLaughState.AddMap(FSMTriggerID.NoNearbyPalCrying, FSMStateID.PalStableLaugh);
        _states.Add(palUnstableLaughState);

        PalCryState palCryState = new PalCryState();
        palCryState.AddMap(FSMTriggerID.ImpactByItem, FSMStateID.PalStableLaugh);
        _states.Add(palCryState);
    }

    protected override void Init() {
        if (Random.value < startCryProbability)
            this.defaultStateID = FSMStateID.PalCry;
        else
            this.defaultStateID = FSMStateID.PalStableLaugh;
    }
}