using UnityEngine;
using System.Collections;
using wuxingogo.tools;


namespace wuxingogo.btFsm
{
    [ActionTitle("Variable/GameObject Variable")]
    public class GameObjectVar : BTVariableT<GameObject>
    {
        public bool isSearchWhenRuntime = false;
        public override void OnAwake()
        {
            base.OnAwake();

            if( isSearchWhenRuntime )
            {
                var source = GetSource();
                var t = source.Value;
				var n = GameObjectUtilities.GetRelativePath(source.Owner.gameObject, Value );
                XLogger.Log( n );
                Value = Owner.transform.Find( n ).gameObject;
            }
        }
    }
}
