using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackAnimationEnd", story: "When [Attack] Animation Ends Change [State] based on [Player] [Move]", category: "Action", id: "bd8b7425535fd18a1947347391287803")]
public partial class AttackAnimationEndAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyCombat> Attack;
    [SerializeReference] public BlackboardVariable<EnemyState> State;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<EnemyMovement> Move;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Attack.Value.AttackAnimationEnded)
        {
            //If the player left the attack range turn state
            if (!Move.Value.PlayerInAttackRange(out _))
            {
                State.Value = EnemyState.Patroling;
                return Status.Success;
            }
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

