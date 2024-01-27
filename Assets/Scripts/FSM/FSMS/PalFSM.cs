using AI.FSM;

public class PalFSM : FSMBase {
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
}