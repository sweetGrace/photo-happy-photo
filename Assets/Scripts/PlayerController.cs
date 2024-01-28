using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    /************Properties************/
    [Header("Physics")]
    public Vector3 forward;
    private Vector3 right;
    public Vector2 speed;
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
    public float extraHeight;
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
    private int comboAtFrameStart;
    /*--------------------------------*/

    public static PlayerController Instance { get; private set; } = null;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogWarning("PlayerController instance already exists. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private void Start() {
        forward.Normalize();
        right = -Vector3.Cross(forward, Vector3.up).normalized;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        leftItem = rightItem = null;
        faceDirection = 1;
    }

    private void Update() {
        comboAtFrameStart = GameManagerFSM.Instance.Combo;
        pickedup = false;
        HandleItemUse();
        HandlePickUp();
        HandleSwap();
        HandleThrow();

        animator.SetFloat("XSpeed", Input.GetAxis("Horizontal"));
        animator.SetFloat("YSpeed", Input.GetAxis("Vertical"));
    }

    private void LateUpdate() {
        if ((Input.GetButtonDown("UseLeft") || Input.GetButtonDown("UseRight")) &&
            comboAtFrameStart == GameManagerFSM.Instance.Combo) {
            GameManagerFSM.Instance.Combo = 0;
        }
    }

    private void FixedUpdate() {
        HandleMove();
    }

    private void HandleThrow() {
        if (!Input.GetButtonDown("Throw"))
            return;
        if (rightItem == null && leftItem == null)
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
        if (rightItem != null) {
            rightItem.Drop();
            rightItem.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
            if (direction != Vector3.zero)
                rightItem.GetComponent<Rigidbody>().AddTorque(Vector3.Cross(direction, Vector3.up) * rotateForce, ForceMode.Impulse);
            rightItem = null;
        } else {
            leftItem.Drop();
            leftItem.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
            if (direction != Vector3.zero)
                leftItem.GetComponent<Rigidbody>().AddTorque(Vector3.Cross(direction, Vector3.up) * rotateForce, ForceMode.Impulse);
            leftItem = null;
        }
    }

    private void HandleSwap() {
        if (!Input.GetButtonDown("Swap"))
            return;
        leftItem?.Drop();
        rightItem?.Drop();
        leftItem?.Pick(rightHand);
        rightItem?.Pick(leftHand);
        Item temp = leftItem;
        leftItem = rightItem;
        rightItem = temp;
    }

    private void HandlePickUp() {
        if (!Input.GetButtonDown("PickUp"))
            return;
        Vector3 center = foot.position;
        var temp = Physics.OverlapSphere(center, pickUpRange, LayerMask.GetMask("Item") | LayerMask.GetMask("Camera"))
            .Where(item => (leftItem == null || item.name != leftItem.name) && (rightItem == null || item.name != rightItem.name))
            .OrderBy(item => (
                new Vector2(center.x, center.z) - new Vector2(item.transform.position.x, item.transform.position.z)
            ).sqrMagnitude);

        if (temp.Count() == 0) {
            Debug.Log("Nothing in pick up range");
            return;
        }

        var collider = temp.First();

        Item item = collider.GetComponent<Item>();
        if (item != null) {
            Debug.Log("Found item");
            if (rightItem != null) {
                if (leftItem != null) {
                    rightItem.Drop();
                    rightItem = null;
                } else {
                    item.Pick(leftHand);
                    leftItem = item;
                    pickedup = true;
                    return;
                }
            }

            item.Pick(rightHand);
            rightItem = item;

            pickedup = true;
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
        if (Input.GetButtonDown("UseLeft") && leftItem != null) {
            leftItem.Use(this);
            return;
        }
        if (Input.GetButtonDown("UseRight") && rightItem != null) {
            rightItem.Use(this);
            return;
        }
        if (!Input.GetButtonDown("UseLeft") && !Input.GetButtonDown("UseRight"))
            return;

        Vector3 center = transform.position;
        var temp = Physics.OverlapSphere(center, pickUpRange, LayerMask.GetMask("Pal"))
            .OrderBy(item => (
                new Vector2(center.x, center.z) - new Vector2(item.transform.position.x, item.transform.position.z)
            ).sqrMagnitude);

        if (temp.Count() == 0) {
            Debug.Log("Nothing in pick up range");
            return;
        }

        var collider = temp.First();
        PalManager pal = collider.GetComponent<PalManager>();
        if (pal != null) {
            Debug.Log("Found pal " + pal.name);
            pal.ImpactByItem(ItemID.None);
            return;
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
        rb.velocity = new Vector3(direction.x * speed.x, y_speed, direction.z * speed.y);

        RaycastHit hit;
        if (Physics.Raycast(foot.position, direction, out hit, detectDistance, LayerMask.GetMask("Ground"))) {
            Debug.Log("Step detected");
            if (hit.collider.bounds.size.y <= maxStepHeight) {
                rb.position += Vector3.up * (hit.collider.bounds.size.y + extraHeight);
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
