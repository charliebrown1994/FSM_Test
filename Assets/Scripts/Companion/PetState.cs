/// <summary>
/// ��� ���°� ����ϴ� �߻� ���̽�.
/// - Enter / Update / FixedUpdate / Exit �����ֱ� ����
/// - Companion(���ؽ�Ʈ)�� �����ϰ� ������ �� �ֵ��� ����
/// </summary>
public abstract class PetState
{
    protected readonly Companion comp;

    protected PetState(Companion comp) => this.comp = comp;

    public virtual void Enter() { }
    public virtual void Update(float dt) { }
    public virtual void FixedUpdate(float dt) { }
    public virtual void Exit() { }
}
