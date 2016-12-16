using UnityEngine;
using UnityEditor;
using System.Collections;
using wuxingogo.Node;
using wuxingogo.Editor;
using System.Collections.Generic;
using wuxingogo.Runtime;
using wuxingogo.btFsm;
using UnityEditor.Graphs;


namespace wuxingogo.BTNode
{

    public class BTEditorWindow : DTEditorWindow<BTFsm>
    {
        static bool isHide
        {
            get
            {
                return EditorPrefs.GetBool( "BTFsm_isHide_Assets", false );
            }
            set
            {
                EditorPrefs.SetBool( "BTFsm_isHide_Assets", value );
            }
        }

        public BTEditorWindow()
        {
            instance = this;
        }
        [MenuItem( "Wuxingogo/BTEditor/ Open Window" )]
        static void Open()
        {
            instance = InitWindow<BTEditorWindow>();
        }
        [MenuItem( "Wuxingogo/BTEditor/ Hide All Assets" )]
        static void HideAsset()
        {
            isHide = true;
            ChangeAllFlag( HideFlags.HideInHierarchy );
           	
        }

		[MenuItem( "Wuxingogo/BTEditor/ Delete all selected Assets" )]
		static void DeleteAsset()
		{
			var objects = Selection.objects;
			for (int i = 0; i < objects.Length; i++) {
				DestroyImmediate(objects[i], true);
			}
			
		}

        [MenuItem( "Wuxingogo/BTEditor/ Show All Assets" )]
        static void ShowAsset()
        {
            isHide = false;
            ChangeAllFlag( HideFlags.None );
            
        }

        [MenuItem( "Wuxingogo/BTEditor/ Save Target" )]
        static void Save()
        {
            if( target != null )
            {
                var fileName = "Assets/" + target.gameObject.name + ".prefab";
                if( target.template == null )
                {
                    //	PrefabUtility.CreatePrefab( fileName, target.gameObject );

                    BTTemplate asset = new BTTemplate( target );

                    //	AddObjectToAsset( asset,fileName );

                    AssetDatabase.CreateAsset( asset, "Assets/" + target.gameObject.name + ".asset" );
                    for( int i = 0; i < target.totalState.Count; i++ )
                    {
                        var currState = target.totalState[i];
                        AddObjectToAsset( currState, asset );
                        for( int j = 0; j < currState.totalActions.Count; j++ )
                        {
                            var currAction = currState.totalActions[j];
                            AddObjectToAsset( currAction, currState );
                        }

                    }


                    target.template = asset;
                    EditorUtility.SetDirty( target );
                }
                else
                {
                    BTTemplate asset = target.template;
                    asset.totalState = target.totalState;

                    for( int i = 0; i < target.totalState.Count; i++ )
                    {
                        var currState = target.totalState[i];
                        var path = AssetDatabase.GetAssetPath( currState );
                        if( path == null )
                        {
                            AddObjectToAsset( currState, asset );
                        }
                        for( int j = 0; j < currState.totalActions.Count; j++ )
                        {
                            var currAction = currState.totalActions[j];
                            AddObjectToAsset( currAction, currState );
                        }
                    }
                    EditorUtility.SetDirty( target.template );

                }
            }
            else
            {
                Debug.Log( "Selet one component 'BTFsm' from BTEditor." );
            }
        }

        public static BTFsm target = null;

        public List<BTNode> totalNode = new List<BTNode>();

        [System.NonSerialized]
        public BTEvent currentEvent = null;
        public Rect currentEventRect;

        public static BTEditorWindow instance = null;

        VariableWindow variableWindow;

        #region implemented abstract members of DTEditorWindow

        public void OnEnable()
        {
            base.OnEnable();
            variableWindow = new VariableWindow();
        }

        public void Clear()
        {
            totalNode.Clear();
            if( target != null && target.OnFireEvent != null )
                target.OnFireEvent = null;

        }

        public void OnFireEvent( BTState state, BTEvent btEvent )
        {
            //XLogger.Log( state.ToString(), state );
            var currNode = FindBTNode( state );
            currNode.FireEvent( btEvent );
        }

        public override void OnInitialization( params object[] args )
        {
            base.OnInitialization( args );
            instance.Clear();
        }

        public override DragNode[] DragNodes()
        {
            if( target == null )
                return new DragNode[0];
            if( totalNode.Count > 0 )
                return totalNode.ToArray();
            int count = target.totalState.Count;
            for( int i = 0; i < count; i++ )
            {
                AddNewBTNode( target.totalState[i] );
            }

			ConnectAllSlot();

            target.OnFireEvent = OnFireEvent;
            return totalNode.ToArray();

        }
		public void ConnectAllSlot()
		{
			int count = totalNode.Count;
			for( int i = 0; i < count; i++ )
			{
				var state = totalNode[i];
				state.FindSlot();
			}
		}
		public void ConnectSlot(Slot lhs,  Slot rhs)
		{
			stateMachineGraph.Connect(lhs, rhs);
		}
		
		public BTNode FindBTNode( BTState currNode )
        {
            for( int i = 0; i < totalNode.Count; i++ )
            {
                if( totalNode[i].BtState == currNode )
                {
                    return totalNode[i];
                }
            }
            return null;
        }

        public BTNode AddNewBTNode( BTState newBTState )
        {
            var newNode = new BTNode( newBTState );
            
            totalNode.Add( newNode );
            stateMachineGraph.AddNode( newNode );
			newNode.position = newBTState.Bounds;
            return newNode;

        }

        public override void OnXGUI()
        {
            DrawGraphGUI();
            //Event e = Event.current;
            if( target == null )
            {

                if( GUI.Button(
                        new Rect( position.width / 2 - 50, Screen.height / 2, 100, 50 ),
                        new GUIContent( "Create Fsm" ) ) )
                {
                    BTFsmEditor.CreateFsm();

                }

                return;
            }
            if( target != null )
            {
//                BeginWindows();
//                Event e = Event.current;
//                variableWindow.Draw( target );
//                if( variableWindow.isPointInContainer( e.mousePosition ) )
//                {
//                    e = null;
//                }
//
//
//                EndWindows();
				DragNodes();
//                EditorGUI.LabelField( new Rect( position.width / 2 - 50, 200, 100, 100 ), new GUIContent( target.Name ));
//
//                if( currentEvent != null )
//                {
//                    Rect mouseRect = new Rect( AdsorptionNodePosition(), new Vector2( 10, 10 ) );
//                    DragNode.DrawBesizeFromRect( currentEventRect, mouseRect );
//                }
//
//				DrawGlobalEvent();
//                base.OnXGUI();

            }
        }

		private void ChangeGlobalEvent(BTState targetState)
		{
			currentEvent.TargetState = targetState;
			if(currentEvent.Name == target.startEvent.Name)
			{
				target.startEvent.TargetState = targetState;
				target.template.UpdateGlobalEvent(currentEvent, targetState);

			}
		}

        public override void OnNoneSelectedNode()
        {
            if( target != null )
                Selection.objects = new Object[] { target.gameObject };
            if( currentEvent != null && currentEvent.Name != target.startEvent.Name )
            {
				if(currentEvent.isGlobal){
					currentEvent.TargetState = null;
					currentEvent = null;
					currentEventRect = new Rect( 0, 0, 0, 0 );
				}else{
	                currentEvent.TargetState = null;
	                currentEvent = null;
	                currentEventRect = new Rect( 0, 0, 0, 0 );
				}
            }else{
				currentEvent = null;
			}
        }

        public override BTFsm targetNode
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        public static void AddObjectToAsset( XScriptableObject lhs, Object rhs )
        {
            lhs.hideFlags = isHide ? HideFlags.HideInHierarchy : HideFlags.None;
            AssetDatabase.SetLabels( lhs, new string[] { "BTFsm" });
            AssetDatabase.AddObjectToAsset( lhs, rhs );
        }

        public void SetCurrentEvent( BTEvent currentEvent )
        {
            this.currentEvent = currentEvent;
        }
        public void SetCurrentRect( Rect currentEventRect )
        {
            this.currentEventRect = currentEventRect;
        }
        public static void ChangeAllFlag( HideFlags hideFlags )
        {
            var allState = AssetsUtilites.FindAssetsByTags<BTState>("BTFsm");
            var allTemplate = AssetsUtilites.FindAssetsByTags<BTTemplate>( "BTFsm" );
            var allAction = AssetsUtilites.FindAssetsByTags<BTAction>( "BTFsm" );
            var allVariable = AssetsUtilites.FindAssetsByTags<BTVariable>( "BTFsm" );

            for( int i = 0; i < allState.Length; i++ )
            {
                allState[i].hideFlags = hideFlags;
                EditorUtility.SetDirty( allState[i] );

            }
            for( int i = 0; i < allTemplate.Length; i++ )
            {
                allTemplate[i].hideFlags = hideFlags;
                EditorUtility.SetDirty( allTemplate[i] );
            }
            for( int i = 0; i < allAction.Length; i++ )
            {
                allAction[i].hideFlags = hideFlags;
                EditorUtility.SetDirty( allAction[i] );
            }

            for( int i = 0; i < allVariable.Length; i++ )
            {
                allVariable[i].hideFlags = hideFlags;
                EditorUtility.SetDirty( allVariable[i] );

            }
            AssetDatabase.Refresh();
        }
        public void RemoveState( BTNode currentState )
        {
            totalNode.Remove( currentState );
            target.RemoveState( currentState.BtState );
            if( target.template != null )
            {
                target.template.totalState.Remove( currentState.BtState );
                DestroyImmediate( currentState.BtState, true );
            }
            else
            {
                DestroyImmediate( currentState.BtState );
            }

            EditorUtility.SetDirty( target );

        }

        public override DTGenericMenu<BTFsm> GetGenericMenu()
        {
            return new BTGenericMenu();
        }

        protected override void OnSelectNode( DragNode node )
        {
            if( currentEvent != null )
            {
                var currentNode = node as BTNode;
				ChangeGlobalEvent(currentNode.BtState);
                currentEvent = null;
                Dirty();
            }
            else
            {
                base.OnSelectNode( node );
            }

        }


        public void Dirty()
        {
            EditorUtility.SetDirty( target );
        }

        private void DrawGlobalEvent()
        {
            for( int i = 0; i < target.totalEvent.Count; i++ )
            {
                var targetEvent = target.totalEvent[i];
                for( int j = 0; j < totalNode.Count; j++ )
                {
                    var targetState = totalNode[j].BtState;
					if( targetEvent.TargetState != null && targetEvent.TargetState.Name == targetState.Name )
                    {
						bool globalEvent = targetEvent == currentEvent;
						DrawEvent( i,globalEvent, j );
                        break;
                    }
                }
            }
        }
		private BTEvent currGlobalEvent = null;
        protected void DrawEvent( int eventIndex, bool isGlobalEvent, int stateIndex )
        {
            var targetEvent = target.totalEvent[eventIndex];
            var targetState = totalNode[stateIndex];

            var bounds = new Rect( targetState.DrawBounds.position + new Vector2( 0, -30 ), new Vector2( 100, 35 ) );

			if(isGlobalEvent)
				BTEditorWindow.instance.SetCurrentRect( bounds );
            var arrow = new Rect( targetState.DrawBounds.position + new Vector2( 50, -15 ), new Vector2( 0, 35 ) );
			GUI.Box( arrow, "" );

			if(GUI.Button( bounds, target.totalEvent[eventIndex].Name, XStyles.GetInstance().button )){
				currentEvent = target.totalEvent[eventIndex];
			}

        }


        public static bool HasPrefab( BTFsm refFsm )
        {
            return PrefabUtility.GetPrefabObject( refFsm.gameObject ) != null;
        }

        #endregion

    }

}
