using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // �ʿ��� ���� �����ϱ�
    // [SerializeField] Rigidbody rigidbody; // private ������ �����ϸ鼭 �ν����� â���� ���� ������ �� �ֵ��� �ϴ� �Ӽ� (������Ʈ�� �巡���ؼ� ���) 
    private Rigidbody rigidbody; // Rigidbody ������Ʈ�� ������ ����, �̵��� rigidbody�� ó���ϱ� ���� �ʿ�
    public float speed = 10f; // �̵� �ӵ� ����
    public float jumpHight = 10f; // ���� ���� ����
    public float dash = 10f; // ��ø� �󸶳� �ָ� �ϴ��� ����
    public float rotSpeed = 6f; // ȸ�� �ӵ� ����

    private Vector3 dir = Vector3.zero; // ����������� �̵����� �����ϴ� ����
    // Vector3�� (x, y, z)�� �� ���� ��� ����ü�Դϴ�. (�ַ� ��ġ, ����, �ӵ� ���� ǥ���� �� ���)
    // Vector3.zero�� x, y, z ���� ��� 0�� �����Դϴ�. (��, (0, 0, 0))

    private bool ground = false; // ���� ����ִ��� Ȯ���ϴ� ����
    public LayerMask layer; // �� ���̾ ������ ����

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>(); 
        // ������ �� Rigidbody ������Ʈ�� �����ͼ� ������ ����

    }
    void Update()
    {
        dir.x = Input.GetAxis("Horizontal"); // �¿� ����Ű �Է°��� dir�� x�࿡ ���� a, d�� ������ �� -1, 1 ���� ��
        dir.z = Input.GetAxis("Vertical"); // ���� ����Ű �Է°��� dir�� z�࿡ ���� w, s�� ������ �� -1, 1 ���� ��
        dir.Normalize(); // dir ������ ũ�⸦ 1�� ���� (��, ���⸸ ��Ÿ���� ũ��� 1�� ����)

        CheckGround(); // ���� ����ִ��� Ȯ���ϴ� �Լ� ȣ��

        if (Input.GetButtonDown("Jump") && ground)
        {
            Vector3 jumpPower = Vector3.up * jumpHight; // ���� ���� ���� �������� ����
            rigidbody.AddForce(jumpPower, ForceMode.VelocityChange); // ���� ���� ������ٵ� �߰�
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 dashPower = this.transform.forward * -Mathf.Log(1/rigidbody.drag) * dash; // ��� ���� �÷��̾ �ٶ󺸴� �������� ����
            rigidbody.AddForce(dashPower, ForceMode.VelocityChange); // ��� ���� ������ٵ� �߰�
        }
    }

    private void FixedUpdate()
    {
        if (dir != Vector3.zero)
        {
            // ���� �ٶ󺸴� ������ ��ȣ != ���ư� ������ ��ȣ
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                   transform.Rotate(0, 1, 0);                  
            }
            // this.transform.forward = dir; // �̵� �������� �÷��̾��� �� ������ ����
            // transform.forward�� ������Ʈ�� �� ������ ��Ÿ���� �����Դϴ�. (��, ������Ʈ�� �ٶ󺸴� ����)
            // �� �ڵ�� �÷��̾ �̵��ϴ� �������� �׻� �ٶ󺸵��� �����մϴ�.
            // �ش� �ڵ�� ������Ʈ�� �ٶ󺸴� ������ �����ϴ� ������ �մϴ�.
            // this �ش� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� transform.forward �� ������ dir �������� ���� (�� �Է��ϴ� ���� �������� ȸ��)

            // �ڿ������� ȸ���� ���� lerp ���
            this.transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
            // Vector3.Lerp�� �� ���� ���̸� ���� �����ϴ� �Լ��Դϴ�. (��, �� ���� ���̸� �ε巴�� �̵�)
            // Time.deltaTime�� ���� �����Ӱ� ���� ������ ������ �ð� ������ ��Ÿ���ϴ�. (��, ������ ���� �ð� ����)
        }

        rigidbody.MovePosition(this.gameObject.transform.position + dir * speed * Time.deltaTime); // �̵� ó��
    }

    void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, layer))
        {
            ground = true;
        }
        else
        {
            ground = false;
        }
    }

}
