using UnityEngine;
using System.Collections;



[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
	public float fSpeed;
	public Boundary boundary;
	public float fTilt;
	public GameObject bolt;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire = 0.0f;
	void FixedUpdate()
	{
		float hMove = Input.GetAxis("Horizontal");
		float vMove = Input.GetAxis("Vertical");
	
		Vector3 newPos = new Vector3(
			hMove,
			0.0f,
			vMove);
	
		GetComponent<Rigidbody>().velocity = newPos * fSpeed;
	
		GetComponent<Rigidbody>().position = new Vector3(
			Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax));
#if true
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(
			0.0f,
			0.0f,
			GetComponent<Rigidbody>().velocity.x * -fTilt);
#endif
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.Z) && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(bolt, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play();

		}

	}
}
