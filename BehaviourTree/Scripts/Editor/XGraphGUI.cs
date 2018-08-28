
using System;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace wuxingogo.Node
{
    public class XGraphGUI : GraphGUI
    {
        public static GUIStyle _pinIn = null;
        public static GUIStyle _pinOut = null;

		public void Init()
		{
			_pinIn = new GUIStyle( UnityEditor.Graphs.Styles.triggerPinIn );
			_pinIn.stretchWidth = false;
			_pinOut = new GUIStyle( UnityEditor.Graphs.Styles.triggerPinOut );
			_pinOut.stretchWidth = false;
		}
        public override void NodeGUI( UnityEditor.Graphs.Node node )
        {
            SelectNode( node );

            foreach( var slot in node.inputSlots )
                LayoutSlot( slot, slot.title, false, true, true, _pinIn );

            node.NodeUI( this );

            foreach( var slot in node.outputSlots )
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                LayoutSlot( slot, slot.title, true, false, true, _pinOut );
                EditorGUILayout.EndHorizontal();
            }
            DragNodes();
        }

        public override void OnGraphGUI()
        {
            // Show node subwindows.
            m_Host.BeginWindows();

            foreach( var node in graph.nodes )
            {
                // Recapture the variable for the delegate.
                var node2 = node as BTNode.BTNode;

                // Subwindow style (active/nonactive)
                var isActive = selection.Contains( node );
                //var style = UnityEditor.Graphs.Styles.GetNodeStyle( node.style, node.color, isActive );
				var style = node2.isCustomState ? BTStyle.skin.FindStyle( "RedBox" ) : BTStyle.skin.FindStyle( "GreyBox" );
                // Show the subwindow of this node.
                node.position = GUILayout.Window(
                    node.GetInstanceID(), node2.position,
                    delegate {
                        NodeGUI( node2 );
                    },
                    node.title, style
                );
            }

            // Workaround: If there is no node in the graph, put an empty
            // window to avoid corruption due to a bug.
//            if( graph.nodes.Count == 0 )
//                GUILayout.Window( 0, new Rect( 0, 0, 1, 1 ), delegate {
//                }, "", "MiniLabel" );

            m_Host.EndWindows();

            // Graph edges
            edgeGUI.DoEdges();
            edgeGUI.DoDraggedEdge();

            // Mouse drag
            //DragSelection( new Rect( -5000, -5000, 10000, 10000 ) );
            DragSelection();

            // Context menu
            ShowCustomContextMenu();
            HandleMenuEvents();
        }

        private void ShowCustomContextMenu()
        {
           
        }
    }
}
