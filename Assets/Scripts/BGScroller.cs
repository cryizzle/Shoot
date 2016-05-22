using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour {
	public float scrollSpeed;
	public float tile_z;

	private Vector3 start_pos;
	void Start()
	{
		start_pos = transform.position;
	}


	// Update is called once per frame
	void Update () {
		float new_pos = Mathf.Repeat(Time.time * scrollSpeed, tile_z);
		transform.position = start_pos + Vector3.forward * new_pos; 
	}
}
