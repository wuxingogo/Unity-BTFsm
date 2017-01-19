using UnityEngine;
using System.Collections;
using wuxingogo.btFsm;
[ActionTitle("BTFsm/Destory Action")]
public class DestroyFsmAction : BTAction {

	public override void OnEnter()
	{
		base.OnEnter();
		Destroy( Fsm );
	}
}
