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
    [Header("PickUp")]
    public float pickUpRange;
    [Header("Throw")]
    public float throwForce;
    public float throwAngle;
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

    private void FixedUpdate() {
        int combo = GameManagerFSM.Instance.Combo;
        pickedup = false;
        HandleMove();
        HandleItemUse();
        HandlePickUp();
        HandleSwap();
        HandleThrow();

        if ((Input.GetButton("UseLeft") || Input.GetButton("UseRight")) &&
            combo == GameManagerFSM.Instance.Combo) {
            GameManagerFSM.Instance.Combo = 0;
        } else if (Input.GetButton("PickUp") && !pickedup && combo == GameManagerFSM.Instance.Combo) {
            GameManagerFSM.Instance.Combo = 0;
        }

        animator.SetFloat("XSpeed", Input.GetAxis("Horizontal"));
        animator.SetFloat("YSpeed", Input.GetAxis("Vertical"));
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
        rightItem = null;
    }

    private void HandleSwap() {
        if (!Input.GetButtonDown("Swap"))
            return;
        leftItem.Drop();
        rightItem.Drop();
        leftItem.Pick(rightHand);
        rightItem.Pick(leftHand);
    }

    private void HandlePickUp() {
        if (!Input.GetButton("PickUp"))
            return;
        Collider collider = Physics.OverlapSphere(transform.position, pickUpRange, LayerMask.GetMask("Item") | LayerMask.GetMask("Pal") | LayerMask.GetMask("Camera"))
            .Where(item => item != leftItem && item != rightItem)
            .OrderBy(item => (transform.position - item.transform.position).sqrMagnitude)
            .First();
        if (collider == null)
            return;

        Item item = collider.GetComponent<Item>();
        if (item != null) {
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
            pal.ImpactByItem(ItemID.None);
            return;
        }

        if (collider.CompareTag("Camera")) {
            collider.GetComponent<AudioSource>()?.Play();
            GameManagerFSM.Instance.cameraed = true;
            GameManagerFSM.Instance.SetTrigger(AI.FSM.FSMTriggerID.GameEnd);
            return;
        }
    }

    private void HandleItemUse() {
        if (Input.GetButton("UseLeft")) {
            leftItem?.Use(this);
        } else if (Input.GetButton("UseRight")) {
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
    }
}
