/// <summary>
/// ���� ���� + ��ȯ + ������Ʈ�� �����ϴ� ���� ���¸ӽ� ����.
/// - ���� å��(SRP)���� Companion�� ����ϰ� ����
/// </summary>
public class PetStateMachine
{
    private PetState _current;

    public void Initialize(PetState start)
    {
        _current = start;
        _current.Enter();
    }

    public void Change(PetState next)
    {
        if (_current == next) return;
        _current?.Exit();
        _current = next;
        _current?.Enter();
    }

    public void TickUpdate(float dt) => _current?.Update(dt);
    public void TickFixedUpdate(float dt) => _current?.FixedUpdate(dt);
}
