using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wuxingogo.btFsm;

[ActionTitle("BTFsm/Random Action")]
public class RandomAction : BTAction {

    private int allEventCount = 0;
    public override void OnAwake()
    {
        base.OnAwake();
        allEventCount = Owner.totalEvent.Count;
    }
    public override void OnEnter()
    {
        base.OnEnter();

        int index = Random.Range(0, allEventCount);
        var randEvent = Owner.totalEvent[index];
        Fsm.FireEvent(randEvent);
    }
}
