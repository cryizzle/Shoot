using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float minTumble, maxTumble, minSpeed, maxSpeed, minSize, maxSize;
	//public float speedScale = 3.0f;
	//private float size;
	void Start()
	{
		float fTumble = Random.Range(minTumble, maxTumble);
		float fSpeed = Random.Range(minSpeed, maxSpeed) /** speedScale*/;
		float size = Random.Range(minSize, maxSize);
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * fTumble;
		Vector3 movement = new Vector3(
			Random.Range(-0.3f, 0.3f),
			0.0f,
			-1
			);
		GetComponent<Rigidbody>().velocity = movement * fSpeed;
		GetComponent<Rigidbody>().transform.localScale *= size;
	}
}