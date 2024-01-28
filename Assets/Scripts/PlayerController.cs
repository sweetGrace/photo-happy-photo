using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    /************Properties************/
    [Header("Physics")]
    public Vector3 forward;
    private Vector3 right;
    public float speed;
    [Header("Items")]
    public Transform leftHand;
    public Transform rightHand;
    public Transform foot;
    [Header("PickUp")]
    public float pickUpRange;
    [Header("Throw")]
    public float throwForce;
    public float throwAngle;
    public float rotateForce;
    [Header("Steps")]
    public float detectDistance;
    public float maxStepHeight;
    [Header("Debug")]
    public bool drawPickupRange;
    /*--------------------------------*/

    /************Components************/
    private Rigidbody rb;
    private Animator animator;
    /*--------------------------------*/

    /************Variables************/
    private Item leftItem;
    private Item rightItem;
    private float faceDirection;
    private bool pickedup;
    /*--------------------------------*/

    private void Start() {
        forward.Normalize();
        right = -Vector3.Cross(forward, Vector3.up).normalized;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        leftItem = rightItem = null;
        faceDirection = 1;
    }

    private void Update() {
        int combo = GameManagerFSM.Instance.Combo;
        pickedup = false;
        HandleItemUse();
        HandlePickUp();
        HandleSwap();
        HandleThrow();

        if ((Input.GetButtonDown("UseLeft") || Input.GetButtonDown("UseRight")) &&
            combo == GameManagerFSM.Instance.Combo) {
            GameManagerFSM.Instance.Combo = 0;
        } else if (Input.GetButtonDown("PickUp") && !pickedup && combo == GameManagerFSM.Instance.Combo) {
            GameManagerFSM.Instance.Combo = 0;
        }

        animator.SetFloat("XSpeed", Input.GetAxis("Horizontal"));
        animator.SetFloat("YSpeed", Input.GetAxis("Vertical"));
    }

    private void FixedUpdate() {
        HandleMove();
    }

    private void HandleThrow() {
        if (!Input.GetButtonDown("Throw"))
            return;
        if (rightItem == null)
            return;

        float vertical = Input.GetAxis("Vertical");
        Vector3 direction;
        if (Mathf.Approximately(vertical, 0)) {
            direction = Quaternion.RotateTowards(Quaternion.identity, Quaternion.FromToRotation(right * faceDirection, Vector2.up), throwAngle * Mathf.Deg2Rad) * (right * faceDirection);
        } else {
            if (vertical < 0)
                direction = Vector3.zero;
            else
                direction = Vector3.up;
        }

        rightItem.Drop();
        rightItem.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
        if (direction != Vector3.zero)
            rightItem.GetComponent<Rigidbody>().AddTorque(Vector3.Cross(direction, Vector3.up) * rotateForce, ForceMode.Impulse);
        rightItem = null;
    }

    private void HandleSwap() {
        if (!Input.GetButtonDown("Swap"))
            return;
        leftItem?.Drop();
        rightItem?.Drop();
        leftItem?.Pick(rightHand);
        rightItem?.Pick(leftHand);
    }

    private void HandlePickUp() {
        if (!Input.GetButtonDown("PickUp"))
            return;
        Vector3 center = foot.position;
        var temp = Physics.OverlapSphere(center, pickUpRange, LayerMask.GetMask("Item") | LayerMask.GetMask("Pal") | LayerMask.GetMask("Camera"))
            .Where(item => item != leftItem && item != rightItem)
            .OrderBy(item => (center - item.transform.position).sqrMagnitude);

        if (temp.Count() == 0) {
            Debug.Log("Nothing in pick up range");
            return;
        }

        var collider = temp.First();

        Item item = collider.GetComponent<Item>();
        if (item != null) {
            Debug.Log("Found item");
            if (rightItem != null) {
                rightItem.Drop();
                rightItem = null;
            }

            item.Pick(rightHand);
            rightItem = item;

            pickedup = true;
            return;
        }

        PalManager pal = collider.GetComponent<PalManager>();
        if (pal != null) {
            Debug.Log("Found pal");
            pal.ImpactByItem(ItemID.None);
            return;
        }

        if (collider.CompareTag("Camera")) {
            Debug.Log("Found camera");
            collider.GetComponent<AudioSource>()?.Play();
            GameManagerFSM.Instance.cameraed = true;
            GameManagerFSM.Instance.SetTrigger(AI.FSM.FSMTriggerID.GameEnd);
            return;
        }
    }

    private void HandleItemUse() {
        if (Input.GetButtonDown("UseLeft")) {
            leftItem?.Use(this);
        } else if (Input.GetButtonDown("UseRight")) {
            rightItem?.Use(this);
        }
    }

    private void HandleMove() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (!Mathf.Approximately(horizontal, 0)) {
            faceDirection = Mathf.Sign(horizontal);
        }

        Vector3 direction;
        if (!Mathf.Approximately(horizontal + vertical, 0)) {
            direction = (horizontal * right + vertical * forward).normalized;
        } else {
            direction = Vector3.zero;
        }
        float y_speed = rb.velocity.y;
        rb.velocity = direction * speed;
        rb.velocity = new Vector3(rb.velocity.x, y_speed, rb.velocity.z);

        RaycastHit hit;
        if (Physics.Raycast(foot.position, direction, out hit, detectDistance, LayerMask.GetMask("Ground"))) {
            Debug.Log("Step detected");
            if (hit.transform.lossyScale.y <= maxStepHeight) {
                rb.position += Vector3.up * hit.transform.lossyScale.y;
            }
        }
    }

    private void OnDrawGizmos() {
        if (drawPickupRange) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(foot.position, pickUpRange);
        }
    }
}
