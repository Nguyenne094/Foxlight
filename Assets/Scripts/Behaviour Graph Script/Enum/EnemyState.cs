using System;
using Unity.Behavior;

[BlackboardEnum]
public enum EnemyState
{
    Idle,
	Patrol,
	Attack,
	Hurt,
	Die
}
