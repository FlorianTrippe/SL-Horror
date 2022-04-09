using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class CanHearNoise : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject Agent;

    private EcchiEnemy _enemy;

    public override void OnStart()
    {
        _enemy = Agent.Value.GetComponent<EcchiEnemy>();
    }

	public override TaskStatus OnUpdate()
    {
        return _enemy.CheckForNoise() ? TaskStatus.Success : TaskStatus.Failure;
    }
}