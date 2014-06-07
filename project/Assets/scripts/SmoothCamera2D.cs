using UnityEngine;
using System.Collections;



public class SmoothCamera2D : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public float pixelsPerUnit=100;
	
	public int zoom = 1;
	
	
	
	void Start () { 
	//	
		//Camera.main.orthographicSize = Screen.height / 2f / pixelsPerUnit / zoom;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target)
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			// Set this to the Y position you want the camera locked to
		//	destination.y = 0; 
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		Debug.DrawLine (new Vector3 (0, 0, 0), new Vector3 (800/100, 0, 0), Color.red);
		Debug.DrawLine (new Vector3 (0, 0, 0), new Vector3 (0, 640/100, 0), Color.red);
        Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(1280 / 100, 0, 0), Color.red);
	}
}
