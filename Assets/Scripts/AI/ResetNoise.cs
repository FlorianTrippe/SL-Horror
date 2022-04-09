using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ResetNoise : Action
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
        _enemy.ResetNoise();
		return TaskStatus.Success;
	}
}