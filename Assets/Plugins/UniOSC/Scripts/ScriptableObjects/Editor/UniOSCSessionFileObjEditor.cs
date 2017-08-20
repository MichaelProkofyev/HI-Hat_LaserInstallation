/*
* UniOSC
* Copyright © 2014-2015 Stefan Schlupek
* All rights reserved
* info@monoflow.org
*/
using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace UniOSC{

	[CustomEditor (typeof(UniOSCSessionFileObj))]
	public class UniOSCSessionFileObjEditor : Editor {

		private static int minCols = 1;
		private static int maxCols = 5;//Is changed later
		private UniOSCSessionFileObj _target;
		[SerializeField]
		private static Texture2D _tex;
		public static GUIStyle style;
	
		public static void Init(){
			if(_tex == null) _tex = UniOSCUtils.MakeTexture(2,2, new Color(0.95f,0.95f,0.95f,0.25f));
		}

		void OnEnable () {
			if(target  !=_target) _target = target as UniOSCSessionFileObj;
		}

	
		override public void OnInspectorGUI(){

			serializedObject.Update();
			OnGUI_OSCSessionData_Inspector(_target,Screen.width-20f,Screen.height*0.75f);
			serializedObject.ApplyModifiedProperties();
			if (GUI.changed) {
				EditorUtility.SetDirty (_target);
			}

		}


		//
		public static void OnGUI_OSCSessionData_Editor(UniOSCSessionFileObj obj, float screenWidth,float screenHeight){

			EditorGUILayout.BeginVertical(GUILayout.MaxHeight (screenHeight));

			#region Path
			EditorGUILayout.BeginHorizontal();
			GUIContent guic = new GUIContent("Path: "+AssetDatabase.GUIDToAssetPath(obj.my_guid)+"  .","");
			Rect area = GUILayoutUtility.GetRect(guic, GUIStyle.none,GUILayout.MinHeight(30f));//GUILayout.MaxWidth(400f),
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUI.Label(area,guic);
			if(UniOSCUtils.IsMouseUpInArea(area)){UniOSCUtils.SelectObjectInHierachyFromGUID(obj.my_guid);}
			_StartDrag(area,obj);
			EditorGUILayout.EndHorizontal();
			#endregion Path

			#region Button
			_OnGUI_DrawButtons(obj,true);
			#endregion Button

			#region Header
			EditorGUILayout.BeginHorizontal();

			GUIContent con0 = new GUIContent("OSC Address ");
			Rect rect0 = GUILayoutUtility.GetRect(con0,GUI.skin.label,GUILayout.MaxWidth(300f));
			GUI.Label(rect0,con0);

			GUIContent con1 = new GUIContent("");//Learn
			Rect rect1 =  GUILayoutUtility.GetRect(con1,GUI.skin.toggle,GUILayout.Width(80f));
			GUI.Label(rect1,con1);

			GUIContent con2 = new GUIContent("");//Delete  
			Rect rect2 =  GUILayoutUtility.GetRect(con2,GUI.skin.label,GUILayout.Width(70f));
			GUI.Label(rect2,con2);

			GUIContent con3 = new GUIContent("Data[0]");
			Rect rect3 =  GUILayoutUtility.GetRect(con3,GUI.skin.label,GUILayout.MaxWidth(50f));
			GUI.Label(rect3,con3);
			
			GUIContent con4 = new GUIContent("Data[1]");
			Rect rect4 =  GUILayoutUtility.GetRect(con4,GUI.skin.label,GUILayout.MaxWidth(50f));
			GUI.Label(rect4,con4);

			GUIContent con5 = new GUIContent("Data[2]");
			Rect rect5 =  GUILayoutUtility.GetRect(con5,GUI.skin.label,GUILayout.MaxWidth(50f));
			GUI.Label(rect5,con5);

			GUIContent con6 = new GUIContent("Data[3]");
			Rect rect6 =  GUILayoutUtility.GetRect(con6,GUI.skin.label,GUILayout.MaxWidth(50f));
			GUI.Label(rect6,con6);

			if(Event.current.type == EventType.Repaint){
				UniOSCUtils.SESSIONLISTLABELRECTS[0] =  rect0;
				UniOSCUtils.SESSIONLISTLABELRECTS[1] =  rect1;
				UniOSCUtils.SESSIONLISTLABELRECTS[2] =  rect2;
				UniOSCUtils.SESSIONLISTLABELRECTS[3] =  rect3;
				UniOSCUtils.SESSIONLISTLABELRECTS[4] =  rect4;
				UniOSCUtils.SESSIONLISTLABELRECTS[5] =  rect5;
				UniOSCUtils.SESSIONLISTLABELRECTS[6] =  rect6;
			}

			EditorGUILayout.EndHorizontal();
			#endregion Header


			#region List

			style = new GUIStyle(GUI.skin.box);
			style.normal.background =_tex;
			style.margin = new RectOffset(0,0,0,2);

			//draw the session items
			EditorGUILayout.BeginHorizontal();

			if(Event.current.type == EventType.Repaint){screenWidth = UniOSCUtils.SESSIONLISTHEADERLABELWIDTH;}

			obj.scrollpos = EditorGUILayout.BeginScrollView(obj.scrollpos,GUILayout.ExpandHeight(true),  GUILayout.Width(screenWidth),GUILayout.MaxWidth(UniOSCUtils.SESSIONLISTHEADERLABELWIDTH) );
			int rowIndex = 0;

			try{
				for(var i = 0; i<obj.oscSessionItemList.Count;i++){
					GUI.backgroundColor = rowIndex % 2 == 0  ?  UniOSCUtils.ITEM_LIST_COLOR_A : UniOSCUtils.ITEM_LIST_COLOR_B;
					UniOSCSessionItem c = obj.oscSessionItemList[i];

					GUILayout.BeginHorizontal(style);
					UniOSCSessionItemEditor.OnGUI_Editor(c);
					GUILayout.EndHorizontal();

					rowIndex++;
				}
			}catch(Exception){
				//drag&drop Exception handling
			}
		
			GUI.backgroundColor = Color.white;
			EditorGUILayout.EndScrollView();
			
			EditorGUILayout.EndHorizontal();
			

			#endregion List

			EditorGUILayout.EndVertical();
		}


		public static void OnGUI_OSCSessionData_Inspector(UniOSCSessionFileObj obj, float screenWidth,float screenHeight){
		
			EditorGUILayout.BeginVertical(GUILayout.MaxHeight (screenHeight));

			#region Path

			EditorGUILayout.BeginHorizontal();
			GUIContent guic = new GUIContent("Path: "+AssetDatabase.GUIDToAssetPath(obj.my_guid)+"  .","");
			Rect area = GUILayoutUtility.GetRect(guic, GUIStyle.none,GUILayout.MinHeight(30f));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUI.Label(area,guic);
			if(UniOSCUtils.IsMouseUpInArea(area)){UniOSCUtils.SelectObjectInHierachyFromGUID(obj.my_guid);}
			_StartDrag(area,obj);
			EditorGUILayout.EndHorizontal();

			#endregion Path

			_OnGUI_DrawButtons(obj,false);

			#region ScrollView

			obj.scrollposInspector = EditorGUILayout.BeginScrollView(obj.scrollposInspector,GUILayout.ExpandHeight(true),  GUILayout.Width(screenWidth));//GUILayout.Width (100) 

			EditorGUILayout.BeginHorizontal();
			//Hardcoding layout for wrapping
			maxCols =(int) Math.Max( minCols, Math.Floor( (screenWidth -20f)/(UniOSCSessionItem.MAXWIDTH*1.0f)) );//Floor Ceiling
			for(var i = 0; i<obj.oscSessionItemList.Count;i++){
				// Begin new row?
				if (i % maxCols == 0 && i > 0) {
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
				}
				UniOSCSessionItem c = obj.oscSessionItemList[i];

				UniOSCSessionItemEditor.OnGUI_Inspector(c);
			}//for

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndScrollView();
			#endregion ScrollView


			EditorGUILayout.EndVertical();

		}


		private static void _OnGUI_DrawButtons(UniOSCSessionFileObj obj,bool dropAble){
			
			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button ("Add OSC Session Item" ,GUILayout.Width(170),GUILayout.Height(30f) ) ){
				obj.AddOSCSessionItem();
				EditorUtility.SetDirty(obj);//Without this after a restart of unity all data would be gone
			}
			
			GUIContent guicAdd = new GUIContent("Add Items from File","Select a Session or Mapping file to add items with the addresses from the file.");
			Rect dropArea = GUILayoutUtility.GetRect(guicAdd, "button",GUILayout.Width(170),GUILayout.Height(30f));
			if ( GUI.Button(dropArea,guicAdd)){
				_AddItemsFromFile(null,obj);
			}
			if(dropAble){
				Event evt = Event.current;
				_DropArea(evt, dropArea,obj);
			}
			
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(10f);
			
		}


		private static void _StartDrag(Rect area,UniOSCSessionFileObj obj)
		{
			Event evt = Event.current;
			if(!evt.isMouse) return;
			if (evt.type == EventType.MouseDrag && area.Contains(evt.mousePosition)) {
				DragAndDrop.PrepareStartDrag ();
				UnityEngine.Object[] objectReferences = {obj};
				DragAndDrop.objectReferences = objectReferences;//
				DragAndDrop.paths = null;
				DragAndDrop.StartDrag ("OSC_InitFile_Drag");
				Event.current.Use();
			}
		}

		private static void _DropArea (Event evt, Rect area,UniOSCSessionFileObj sfo)
		{
			switch (evt.type) {
				
			case EventType.MouseDown :
				if (area.Contains(evt.mousePosition)) {}
				break;
				
			case EventType.DragUpdated:
			case EventType.DragPerform:
				if (!area.Contains (evt.mousePosition))return;
				
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				
				if (evt.type == EventType.DragPerform) {
					DragAndDrop.AcceptDrag ();
					foreach (UnityEngine.Object dragged_object in DragAndDrop.objectReferences) {

						if(dragged_object.GetType() == typeof(UniOSCMappingFileObj) ){
							_AddItemsFromFile(dragged_object,sfo);
						}						
						if(dragged_object.GetType() == typeof(UniOSCSessionFileObj) ){
							_AddItemsFromFile(dragged_object,sfo);
						}	

					}//foreach
				}//DragPerform
				break;
			}//switch
		}


		private static void _AddItemsFromFile (UnityEngine.Object obj, UniOSCSessionFileObj sfo)
		{
			string filepath = String.Empty;

			if(obj == null){
				filepath = EditorUtility.OpenFilePanel("Select Mapping or Session file","",UniOSCUtils.MAPPINGFILEEXTENSION);
			}else{
				filepath = AssetDatabase.GetAssetPath(obj);
			}

			if (String.IsNullOrEmpty(filepath)) return;

			string relativFilepath = filepath.Replace(Application.dataPath, "Assets");
			ScriptableObject go = AssetDatabase.LoadAssetAtPath(relativFilepath,typeof(ScriptableObject)) as ScriptableObject ;

			if (go == null) return;
					
			if(go is UniOSCSessionFileObj){

				foreach(var si in ((UniOSCSessionFileObj)go).oscSessionItemList){

					var sil = sfo.oscSessionItemList.FindAll (o => o.address ==  si.address);

					if(sil.Count == 0) {
						UniOSCSessionItem _si = new UniOSCSessionItem(sfo);
						_si.address = si.address;
						sfo.oscSessionItemList.Add(_si);
					}
				}//foreach

			}

			if(go is UniOSCMappingFileObj){

				foreach(var mi in ((UniOSCMappingFileObj)go).oscMappingItemList){
					
					var sil = sfo.oscSessionItemList.FindAll (o => o.address ==  mi.address);
					
					if(sil.Count == 0) {
						UniOSCSessionItem _si = new UniOSCSessionItem(sfo);
						_si.address = mi.address;
						sfo.oscSessionItemList.Add(_si);
					}
				}//foreach

			}

			EditorUtility.SetDirty(sfo);//Without this after a restart of unity all data would be gone

			
		}




	}
}