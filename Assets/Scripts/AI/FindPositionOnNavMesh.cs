using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class FindPositionOnNavMesh : Action
{
    [UnityEngine.Tooltip("The NavMeshAgent destination")]
    public SharedVector3 Destination;

public override void OnStart()
	{
		
	}

	public override TaskStatus OnUpdate()
    {
        Destination.Value = RandomNavmeshLocation(4);
		return TaskStatus.Success;
	}

	private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}