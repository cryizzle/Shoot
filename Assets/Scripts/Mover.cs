using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public float fSpeed;
	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * fSpeed;
	}
}
