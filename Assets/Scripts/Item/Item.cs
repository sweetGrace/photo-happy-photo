using UnityEngine;

public class Item : MonoBehaviour {
    public float impactRange;
    public ItemID id;
    [Header("Debug")]
    public bool drawImpactRange;

    public void Use(PlayerController player) {
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, impactRange, LayerMask.GetMask("Pal"));
        foreach (Collider collider in colliders) {
            PalManager pal = collider.GetComponent<PalManager>();
            pal?.ImpactByItem(id);
        }
    }

    public void Drop() {
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Pick(Transform hand) {
        transform.parent = hand;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnDrawGizmos() {
        if (drawImpactRange) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, impactRange);
        }
    }
}
