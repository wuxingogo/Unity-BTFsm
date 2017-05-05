using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wuxingogo.btFsm;
using wuxingogo.Runtime;

[ActionTitle("BTFsm/RollBack Action")]
[Desc( "RollBack LastState" )]
public class RollbackAction : BTAction {

	public float secounds = 1.0f;
	[Disable]
	public float time = 0.0f;
	public override void OnEnter()
	{
		base.OnEnter();

		time = 0.0f;
	}
	public override void OnUpdate()
	{
		base.OnUpdate();
		time += Time.deltaTime;
		if( time > secounds )
		{
			Fsm.RollbackLastState();
		}

	}
}
