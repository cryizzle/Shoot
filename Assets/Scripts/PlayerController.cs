using UnityEngine;
using System.Collections.Generic;
using System;

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
	public GameObject[] weapons;
	public KeyCode[] keys;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire = 0.0f;
	private SortedDictionary<KeyCode, GameObject> weapon_key_map;
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

	void Start()
	{
		weapon_key_map = new SortedDictionary<KeyCode, GameObject>();
		for (int i=0; i < keys.Length; i++)
		{
			weapon_key_map.Add(keys[i], weapons[i]);
		}
	}

	void Update()
	{

		if(Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			if (Input.GetKey(KeyCode.Z)){
				Instantiate(bolt, shotSpawn.position, shotSpawn.rotation);
			}
			if (Input.GetKey(KeyCode.X))
			{	//tribolt
				Instantiate(bolt, shotSpawn.position, shotSpawn.rotation);
				Instantiate(bolt, shotSpawn.position, Quaternion.Euler(0.0f, 30.0f, 0.0f));
				Instantiate(bolt, shotSpawn.position, Quaternion.Euler(0.0f, -30.0f, 0.0f));	
			}
			GetComponent<AudioSource>().Play();
		}

		//}

	}

	GameObject GetWeapon(KeyCode key)
	{
		if (!weapon_key_map.ContainsKey(key))
		{
			return null;
		}
		return weapon_key_map[key];
	}
}
