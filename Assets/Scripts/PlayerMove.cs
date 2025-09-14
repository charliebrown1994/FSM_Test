using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 필요한 변수 선언하기
    // [SerializeField] Rigidbody rigidbody; // private 변수로 선언하면서 인스펙터 창에서 값을 설정할 수 있도록 하는 속성 (오브젝트를 드래그해서 사용) 
    private Rigidbody rigidbody; // Rigidbody 컴포넌트를 저장할 변수, 이동을 rigidbody로 처리하기 위해 필요
    public float speed = 10f; // 이동 속도 변수
    public float jumpHight = 10f; // 점프 높이 변수
    public float dash = 10f; // 대시를 얼마나 멀리 하는지 변수
    public float rotSpeed = 6f; // 회전 속도 변수

    private Vector3 dir = Vector3.zero; // 어느방향으로 이동할지 저장하는 변수
    // Vector3는 (x, y, z)세 축 값을 담는 구조체입니다. (주로 위치, 방향, 속도 등을 표현할 때 사용)
    // Vector3.zero는 x, y, z 값이 모두 0인 벡터입니다. (즉, (0, 0, 0))

    private bool ground = false; // 땅에 닿아있는지 확인하는 변수
    public LayerMask layer; // 땅 레이어를 설정할 변수

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>(); 
        // 시작할 때 Rigidbody 컴포넌트를 가져와서 변수에 저장

    }
    void Update()
    {
        dir.x = Input.GetAxis("Horizontal"); // 좌우 방향키 입력값을 dir의 x축에 저장 a, d를 눌렀을 때 -1, 1 값이 들어감
        dir.z = Input.GetAxis("Vertical"); // 상하 방향키 입력값을 dir의 z축에 저장 w, s를 눌렀을 때 -1, 1 값이 들어감
        dir.Normalize(); // dir 벡터의 크기를 1로 만듦 (즉, 방향만 나타내고 크기는 1로 고정)

        CheckGround(); // 땅에 닿아있는지 확인하는 함수 호출

        if (Input.GetButtonDown("Jump") && ground)
        {
            Vector3 jumpPower = Vector3.up * jumpHight; // 점프 힘을 위쪽 방향으로 설정
            rigidbody.AddForce(jumpPower, ForceMode.VelocityChange); // 점프 힘을 리지드바디에 추가
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 dashPower = this.transform.forward * -Mathf.Log(1/rigidbody.drag) * dash; // 대시 힘을 플레이어가 바라보는 방향으로 설정
            rigidbody.AddForce(dashPower, ForceMode.VelocityChange); // 대시 힘을 리지드바디에 추가
        }
    }

    private void FixedUpdate()
    {
        if (dir != Vector3.zero)
        {
            // 지금 바라보는 방향의 부호 != 나아갈 방향의 부호
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                   transform.Rotate(0, 1, 0);                  
            }
            // this.transform.forward = dir; // 이동 방향으로 플레이어의 앞 방향을 설정
            // transform.forward는 오브젝트의 앞 방향을 나타내는 벡터입니다. (즉, 오브젝트가 바라보는 방향)
            // 이 코드는 플레이어가 이동하는 방향으로 항상 바라보도록 설정합니다.
            // 해당 코드는 오브젝트가 바라보는 방향을 변경하는 역할을 합니다.
            // this 해당 스크립트가 붙어있는 오브젝트에 transform.forward 앞 방향을 dir 방향으로 설정 (즉 입력하는 값에 방향으로 회전)

            // 자연스러운 회전을 위해 lerp 사용
            this.transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
            // Vector3.Lerp는 두 벡터 사이를 선형 보간하는 함수입니다. (즉, 두 벡터 사이를 부드럽게 이동)
            // Time.deltaTime은 이전 프레임과 현재 프레임 사이의 시간 간격을 나타냅니다. (즉, 프레임 간의 시간 차이)
        }

        rigidbody.MovePosition(this.gameObject.transform.position + dir * speed * Time.deltaTime); // 이동 처리
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
