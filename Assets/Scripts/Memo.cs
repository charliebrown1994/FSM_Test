using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo : MonoBehaviour
{
    // Collider 는 출동을 담당하고
    // Rigidbody 는 물리 연산을 담당한다. 즉 이동, 회전, 중력 등을 관련 연산을 처리한다.
    // Mass는 질량, Drag는 공기 저항, Angular Drag는 회전 저항
    // isKinematic 은 물리 연산을 하지 않도록 설정하는 옵션이다. 충돌이 일어나도 물리 연산을 하지 않는다. (코드로 제어는 가능합니다)
    // Interpolate 는 물리 연산과 렌더링 프레임이 맞지 않을 때 보간 처리를 해주는 옵션이다. (부드럽게 움직이도록)
    // Collision Detection 은 충돌 감지 옵션이다.
    // Discrete 는 기본값으로 충돌이 일어나는 시점에만 충돌을 감지한다.
    // Continuous 는 빠르게 움직이는 물체가 충돌하는 경우에도 충돌을 감지한다.

}
