using UnityEngine;

// traverse an off-mesh link along a parabola, then resume the previous state
public class ZombieJumpState : ZombieStateBase
{
    private const float MinDuration = 0.01f;
    private const float ParabolaScalar = 4f; // makes the peak of height * 4t(1-t) equal to height

    private Vector3 _start;
    private Vector3 _end;
    private float _duration;
    private float _elapsed;
    private ZombieState _returnState;

    public ZombieJumpState(Zombie zombie) : base(zombie)
    {
    }

    public override void Enter()
    {
        UnityEngine.AI.OffMeshLinkData link = Zombie.Agent.currentOffMeshLinkData;

        _start = Zombie.transform.position;
        // link.endPos is on the NavMesh surface, the agent's transform sits baseOffset above it
        _end = link.endPos + Vector3.up * Zombie.Agent.baseOffset;
        _duration = Mathf.Max(Vector3.Distance(_start, _end) / Zombie.JumpSpeed, MinDuration);
        _elapsed = 0f;
        _returnState = Zombie.PreviousState;

        Vector3 direction = _end - _start;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0f)
        {
            Zombie.transform.rotation = Quaternion.LookRotation(direction);
        }

        //ToDo. Play Jump animation
    }

    public override void Tick()
    {
        _elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsed / _duration);

        Vector3 position = Vector3.Lerp(_start, _end, t);
        position.y += ParabolaScalar * Zombie.JumpHeight * t * (1f - t);
        Zombie.transform.position = position;

        if (t >= 1f)
        {
            Zombie.ChangeState(_returnState);
        }
    }

    public override void Exit()
    {
        // also covers being interrupted (e.g. death), so the agent never stays stuck on the link
        if (Zombie.Agent.isOnOffMeshLink)
        {
            Zombie.Agent.CompleteOffMeshLink();
        }
    }
}
