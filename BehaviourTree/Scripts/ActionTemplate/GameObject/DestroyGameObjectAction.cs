using UnityEngine;
using System.Collections;
using wuxingogo.btFsm;


[ActionTitle("GameObject/Destroy GameObject")]
public class DestroyGameObjectAction : BTAction {

	public override void OnEnter()
	{
		base.OnEnter();
		Destroy( Fsm.gameObject );
	}
}
