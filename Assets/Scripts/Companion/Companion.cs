using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// ������(Companion)�� ��ü.
/// - ���� ���� ���� �� ������Ʈ ����
/// - ���� ������(�÷��̾� ����, �Ÿ�/�ӵ� ��) ����
/// - Rigidbody �̵��� ���� ����/���� ����
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Companion : MonoBehaviour
{
    [Header("Follow ���")]
    [Tooltip("���� �÷��̾�(�Ǵ� Ÿ��) Transform")]
    public Transform player;

    [Header("�Ÿ� �Ķ����")]
    [Tooltip("�� �Ÿ� �����̸� ����(Idle�� ��ȯ)")]
    public float desiredFollowDistance = 2.5f;
    [Tooltip("�� �Ÿ� �̻��̸� Follow ����")]
    public float followStartDistance = 4.0f;

    [Header("�̵� �Ķ����")]
    [Tooltip("�ִ� �̵� �ӵ�(m/s)")]
    public float moveSpeed = 4.0f;
    [Tooltip("��ǥ�� �ٶ󺸴� ȸ�� ������")]
    public float turnLerp = 10.0f;

    [Header("���� ����")]
    [Tooltip("����鿡���� ���󰡰� ������ üũ(����(y) �ӵ� ����)")]
    public bool constrainToXZPlane = true;

    [Header("���� ����(���� �����׸��ý�)")]
    [Tooltip("desiredFollowDistance���� �̸�ŭ �� ������ �־�� �ٽ� �����Դϴ�.")]
    public float stopBuffer = 0.2f;

    // ������Ʈ
    [HideInInspector] public Rigidbody rb;

    // FSM
    private PetStateMachine _fsm;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // ���� ���� ����(������ ���� ����)
        rb.interpolation = RigidbodyInterpolation.Interpolate; // �ε巯�� �̵�
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        // �ʿ� �� ȸ�� ����: rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Start()
    {
        // ���¸ӽ� �ʱ�ȭ(Idle�� ����)
        _fsm = new PetStateMachine();
        _fsm.Initialize(new IdleState(this));
    }

    private void Update()
    {
        // ����/����(Update �迭)
        _fsm.TickUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // ���� �̵�/����(FixedUpdate �迭)
        _fsm.TickFixedUpdate(Time.fixedDeltaTime);
    }

    /// <summary>�ܺ�/���¿��� ���� ��ȯ�� ��û�� �� ���</summary>
    public void ChangeState(PetState next) => _fsm.Change(next);

    /// <summary>
    /// Rigidbody.MovePosition�� �̿��� �̵�(���� ģȭ��).
    /// ���¿��� �������� ������ ���۷� ����.
    /// </summary>
    public void MoveTowards(Vector3 targetPos, float speed, float dt)
    {
        //Vector3 current = rb.position;
        //Vector3 to = targetPos - current;
        //if (constrainToXZPlane) to.y = 0f;              // ����� �̵�
        //Vector3 dir = to.sqrMagnitude > 0.0001f ? to.normalized : Vector3.zero;
        //Vector3 next = current + dir * speed * dt;
        //rb.MovePosition(next);

        //// ���� ȸ��(����): �̵� ������ �ε巴�� �ٶ󺸰�
        //if (dir.sqrMagnitude > 0.0001f)
        //{
        //    Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        //    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, turnLerp * dt));
        //}

        Vector3 current = rb.position;
        Vector3 to = targetPos - current;
        if (constrainToXZPlane) to.y = 0f;
        Vector3 dir = to.sqrMagnitude > 0.0001f ? to.normalized : Vector3.zero;

        Vector3 next = current + dir * speed * dt;
        next.y = current.y;

        // �÷��̾� �ּ� �Ÿ� ����(������ ������ ������ġ)
        if (player)
        {
            Vector3 fromPlayerNext = next - player.position;
            fromPlayerNext.y = 0f;
            float d = fromPlayerNext.magnitude;
            if (d < desiredFollowDistance)
            {
                // ���ѷ��� ������ �ּ� �Ÿ� ����
                next = player.position + fromPlayerNext.normalized * desiredFollowDistance;
            }
        }

        rb.MovePosition(next);

        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, turnLerp * dt));
        }
    }

    private void OnValidate()
    {
        // �����Ϳ��� �� ��ȣ
        followStartDistance = Mathf.Max(followStartDistance, 0f);
        desiredFollowDistance = Mathf.Clamp(desiredFollowDistance, 0f, followStartDistance);
        moveSpeed = Mathf.Max(moveSpeed, 0f);
        turnLerp = Mathf.Max(turnLerp, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        if (!player) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, desiredFollowDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, followStartDistance);
    }
}
