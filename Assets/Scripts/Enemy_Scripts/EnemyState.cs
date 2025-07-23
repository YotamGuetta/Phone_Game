using System;
using Unity.Behavior;

[BlackboardEnum]
public enum EnemyState
{
    Idle,
	Chasing,
	Attacking,
	Knockedback,
	Patroling
}
