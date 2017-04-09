using UnityEngine;
using System.Collections;
using wuxingogo.btFsm;
using wuxingogo.tools;

[ActionTitle("GameObject/Active Child GameObject")]
public class ActiveObjectAction : BTAction
{
    public GameObject gameObject = null;
    public bool active = true;
    public override void OnEnter()
    {
        base.OnEnter();
        if( Fsm.isPrefab )
            gameObject.SetActive( active );
        else
        {
			var targetTransform = GameObjectUtilities.FindByName( Fsm.transform, gameObject.name );
            targetTransform.gameObject.SetActive( active );
        }
    }
}
