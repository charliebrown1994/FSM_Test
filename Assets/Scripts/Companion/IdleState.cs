using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Idle(���) ����
/// - �÷��̾���� �Ÿ��� followStartDistance�� ������ Follow�� ��ȯ
/// - �̵�/������ �������� ����
/// </summary>
public class IdleState : PetState
{
    public IdleState(Companion comp) : base(comp) { }

    public override void Enter()
    {
        // ���� ���� �ӵ� ����(��¦ �̲������� ���� ����)
        comp.rb.velocity = Vector3.zero;
        comp.rb.angularVelocity = Vector3.zero;
    }

    public override void Update(float dt)
    {
        if (!comp.player) return;

        float dist = Vector3.Distance(comp.transform.position, comp.player.position);
        if (dist > comp.followStartDistance)
        {
            comp.ChangeState(new FollowState(comp));
        }
    }

    
}
