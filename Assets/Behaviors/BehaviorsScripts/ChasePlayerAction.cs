using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChasePlayer", story: "Enemy [Moves] to chase the [Player] in a [Direction]", category: "Action", id: "3a609d37021cc5d8fb0c36aeb9b3601d")]
public partial class ChasePlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyMovement> Moves;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<EnemyAttackDirection> Direction;

    protected override Status OnUpdate()
    {
        //If the player is out of range he will be null
        if (Player.Value == null) 
        {
            Moves.Value.StopMoving();
            return Status.Failure;
            
        }
        //Chases the player in range
        Moves.Value.Chase(Player.Value.transform);
        //Checks if the player is in attack range
        bool success = Moves.Value.PlayerInAttackRange(out EnemyAttackDirection enemyAttackDirection);
        Direction.Value = enemyAttackDirection;
        //Returns success if the player was in atttack range else failure
        return success ? Status.Success : Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

