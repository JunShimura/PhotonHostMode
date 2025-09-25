using UnityEngine;
using Fusion;


public class Player : NetworkBehaviour
{
    [SerializeField] private Ball _prefabBall;
    [SerializeField] private PhysxBall _prefabPhysxBall;
    [Networked] private TickTimer delay { get; set; }

    [Networked] public bool spawnedProjectile { get; set; }

    private NetworkCharacterController _cc;
    private Vector3 _forward;

    public Material _material;

    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }
    private void Awake()
    {
        _material = GetComponentInChildren<MeshRenderer>().material;
        _cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;
    }
    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(spawnedProjectile):
                    _material.color = Color.white;
                    break;
            }
        }
        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);
    }


    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0)
                _forward = data.direction;

            if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
            {
                // Left click to spawn a simple Ball
                if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0))
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(
                        _prefabBall,
                        transform.position + _forward, Quaternion.LookRotation(_forward),
                        Object.InputAuthority, (runner, o) =>
                        {
                            // Initialize the Ball before synchronizing it
                            o.GetComponent<Ball>().Init();
                        }
                        );
                    spawnedProjectile = !spawnedProjectile;
                }
                // Right click to spawn a PhysxBall that uses physics
                else if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON1))
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(_prefabPhysxBall,
                      transform.position + _forward,
                      Quaternion.LookRotation(_forward),
                      Object.InputAuthority,
                      (runner, o) =>
                      {
                          o.GetComponent<PhysxBall>().Init(10 * _forward);
                      });
                    spawnedProjectile = !spawnedProjectile;
                }

            }
        }
    }
}