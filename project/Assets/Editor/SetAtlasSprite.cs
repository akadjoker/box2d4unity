using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
//------------------------------------------------
public class SetAtlasSprite : EditorWindow
{
	//Reference to atlas data game object
	public GameObject AtlasDataObject = null;
	
	//Reference to atlas data
	public AtlasData AtlasDataComponent = null;
	
	//Popup Index
	public int PopupIndex = 0;
	
	//------------------------------------------------
	[MenuItem ("Window/Atlast Texture Editor")]
	static void Init () 
	{
		GetWindow (typeof(SetAtlasSprite),false,"Texture Atlas", true);
	}

	//------------------------------------------------
	void OnEnable ()
	{
	}
	//------------------------------------------------
	void OnGUI () 
	{
		
		//Draw Atlas Object Selector
		GUILayout.Label ("Atlas Generation", EditorStyles.boldLabel);
		AtlasDataObject = (GameObject) EditorGUILayout.ObjectField("Atlas Object", AtlasDataObject, typeof (GameObject), true);
		
		if(AtlasDataObject == null)
			return;
		
		AtlasDataComponent = AtlasDataObject.GetComponent<AtlasData>();
		
		if(!AtlasDataComponent)
			return;
		
		PopupIndex = EditorGUILayout.Popup(PopupIndex, AtlasDataComponent.TextureNames);
		
		if(GUILayout.Button("Select Sprite From Atlas"))
		{
			//Update UVs for selected meshes
			if(Selection.gameObjects.Length > 0)
			{
				foreach(GameObject Obj in Selection.gameObjects)
				{
					//Is this is a mesh object?
					if(Obj.GetComponent<MeshFilter>())
						UpdateUVs(Obj, AtlasDataComponent.UVs[PopupIndex]);
				}
			}
		}
	}
	//------------------------------------------------
	void OnInspectorUpdate()
	{
		Repaint();
	}
	//------------------------------------------------
	//Function to update UVs of selected mesh object
	void UpdateUVs(GameObject MeshOject, Rect AtlasUVs, bool Reset = false)
	{
		//Get Mesh Filter Component
		MeshFilter MFilter = MeshOject.GetComponent<MeshFilter>();
		Mesh MeshObject = MFilter.sharedMesh;
		
		//Vertices
		Vector3[] Vertices = MeshObject.vertices;
		Vector2[] UVs = new Vector2[Vertices.Length];
		
		//Bottom-left
		UVs[0].x=(Reset) ? 0.0f : AtlasUVs.x;
		UVs[0].y=(Reset) ? 0.0f : AtlasUVs.y;
		
		//Bottom-right
		UVs[1].x=(Reset) ? 1.0f : AtlasUVs.x+AtlasUVs.width;
		UVs[1].y=(Reset) ? 0.0f : AtlasUVs.y;
		
		//Top-left
		UVs[2].x=(Reset) ? 0.0f : AtlasUVs.x;
		UVs[2].y=(Reset) ? 1.0f : AtlasUVs.y+AtlasUVs.height;
		
		//Top-right
		UVs[3].x=(Reset) ? 1.0f : AtlasUVs.x+AtlasUVs.width;
		UVs[3].y=(Reset) ? 1.0f : AtlasUVs.y+AtlasUVs.height;
		
		MeshObject.uv = UVs;
		MeshObject.vertices = Vertices;
		
		AssetDatabase.Refresh();
    	AssetDatabase.SaveAssets();
	}
	//------------------------------------------------
}