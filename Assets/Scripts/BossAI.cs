using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour {
	public float fSpeed;
	public Boundary boundary;
	public float fTilt;
	public GameObject bolt;
	//public GameObject[] weapons;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire = 0.0f;
	public float minDistance = 0.5f;
	private Vector3 target_position;

	// Use this for initialization
	void Start () {
	}

	void Update()
	{
		//firing
		if (Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(bolt, shotSpawn.position, shotSpawn.rotation);
			//Instantiate(bolt, shotSpawn.position, Quaternion.Euler(0.0f, 30.0f, 0.0f));
			//Instantiate(bolt, shotSpawn.position, Quaternion.Euler(0.0f, -30.0f, 0.0f));
			GetComponent<AudioSource>().Play();
		}

		DetectBoundary();
		/*
		if (GetComponent<Rigidbody>().transform.position == target_position)
		{
			if(GetComponent<Rigidbody>().transform.position == Vector3.zero){
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				return;
			}
			//move towards origin
			target_position = Vector3.zero;
			Vector3 new_pos = target_position - GetComponent<Rigidbody>().transform.position;
			Move(new_pos.normalized);
		}*/
	}

	void Move(Vector3 v_norm)
	{
		GetComponent<Rigidbody>().velocity = v_norm * fSpeed;

		GetComponent<Rigidbody>().position = new Vector3(
			Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax));
#if true
		if (GetComponent<Rigidbody>().velocity.x != 0.0f)
		{

			GetComponent<Rigidbody>().rotation = Quaternion.Euler(
			0.0f,
			GetComponent<Rigidbody>().rotation.eulerAngles.y,
			(GetComponent<Rigidbody>().velocity.x > 0.0f? 1.0f:0.0f) * -fTilt);
		}
#endif
		//yield return new WaitForSeconds(0.5f);
	}

	IEnumerator MoveHorizontal()
	{
		int direction = 1;
		while (true)
		{
			direction *= -1;
			GetComponent<Rigidbody>().velocity = Vector3.right * fSpeed * direction; ;

			GetComponent<Rigidbody>().position = new Vector3(
				Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
				0.0f,
				Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax));
#if true
			GetComponent<Rigidbody>().rotation = Quaternion.Euler(
				0.0f,
				0.0f,
				GetComponent<Rigidbody>().velocity.x * -fTilt);
			Vector3 boundaryVector = new Vector3(
				boundary.xMax, 0.0f, GetComponent<Rigidbody>().position.z);
			if (direction < 0)      //moving left
			{
				boundaryVector.x = boundary.xMin;
			}

			Vector3 dist = boundaryVector - GetComponent<Rigidbody>().position;
			Debug.Log("boundary vec: " + boundaryVector.ToString());
			float time_square = dist.sqrMagnitude / GetComponent<Rigidbody>().velocity.sqrMagnitude;
			Debug.Log("time square: " + time_square.ToString());
			yield return new WaitForSeconds(Mathf.Pow(time_square, 0.5f));
		}
#endif
	}

	void DetectBoundary()
	{

		GetComponent<Rigidbody>().position = new Vector3(
				Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
				0.0f,
				Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax));
	}

	void OnTriggerExit(Collider other)
	{
		/*
		if (GetComponent<Rigidbody>().transform.position == Vector3.zero)
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			return;
		}
		//move towards origin
		target_position = Vector3.zero;
		Vector3 new_pos = target_position - GetComponent<Rigidbody>().transform.position;
		Move(new_pos.normalized);*/
	}

	void Hunt (Collider other)
	{

	}

	void Flee(Collider other)
	{
		if (other.tag != "Asteroid")
		//if (other.tag != "Bolt")
		{
			return;
		}
		Debug.Log("own pos: " + GetComponent<Rigidbody>().position.ToString());
		Debug.Log("hazard: " + other.tag.ToString());
		Debug.Log("hazard pos: " + other.GetComponent<Rigidbody>().position.ToString());

		VectorLine hazard = new VectorLine(other.GetComponent<Rigidbody>().transform.position, other.GetComponent<Rigidbody>().velocity);
		//bool collision = hazard.VectorOnLineFront(GetComponent<Rigidbody>().transform.position);

		/*Debug.Log("Collision eminent: " + collision.ToString());
		if (!collision)
		{
			return;
		}*/
		Vector3 relative_velocity = other.GetComponent<Rigidbody>().velocity - GetComponent<Rigidbody>().velocity;
		Vector3 relative_displacement = other.GetComponent<Rigidbody>().transform.position - GetComponent<Rigidbody>().transform.position;
		//Vector3 relative_displacement = other.GetComponent<Rigidbody>().transform.position + GetComponent<Rigidbody>().transform.position;
		/*if (relative_displacement.magnitude > minDistance)
		{
			return;
		}*/

		if (GetComponent<Rigidbody>().velocity.magnitude > 0)
		{
			VectorLine self = new VectorLine(GetComponent<Rigidbody>().transform.position, GetComponent<Rigidbody>().velocity);
			bool collision = DetectCollision(self, hazard);
			if (collision)
			{
				Vector3 collision_pt = VectorLine.GetIntersectionPoint(self, hazard);
				relative_displacement = collision_pt - GetComponent<Rigidbody>().transform.position;
			}

		}

		float self_time = relative_displacement.magnitude / hazard.GetSpeed();

		Vector3 new_pos = other.GetComponent<Rigidbody>().position - relative_velocity * self_time;

		target_position = new Vector3(
				Mathf.Clamp(new_pos.x, boundary.xMin, boundary.xMax),
				0.0f,
				Mathf.Clamp(new_pos.z, boundary.zMin, boundary.zMax));
		/*target_position = new Vector3(
				Mathf.Clamp(-1* relative_displacement.x, boundary.xMin, boundary.xMax),
				0.0f,
				Mathf.Clamp(-1* relative_displacement.z, boundary.zMin, boundary.zMax));*/

		Move(target_position.normalized);
	}

	void OnTriggerEnter(Collider other)
	{
		/*if (other.tag == "Boundary" || other.tag == "Player")
		{
			return;
		}*/
		Flee(other);
	}

	bool DetectCollision(VectorLine self, VectorLine other)
	{
		float[] constants = VectorLine.GetIntersectConstants(self, other);
		if (constants[0] < 0 || constants[1] < 0) //both have to be positive for eminent collision
		{
			return false;
		}
		float collision_threshold = 0.5f;   //give and take 0.5s
		Vector3 collision_pt = VectorLine.GetIntersectionPoint(self, other);

		//calculate time it takes for self
		float self_disp = Mathf.Pow((collision_pt - self.GetPoint()).sqrMagnitude, 0.5f);
		float self_time = self_disp / self.GetSpeed();

		//calculate time it takes for other
		float other_disp = Mathf.Pow((collision_pt - other.GetPoint()).sqrMagnitude, 0.5f);
		float other_time = other_disp / other.GetSpeed();

		float time_diff = Mathf.Abs(self_time - other_time);

		Debug.Log("self time: " + self_time + "other time: " + other_time);
		Debug.Log("time diff: " + time_diff);

		return time_diff <= collision_threshold;    //collision detected if they collide within 0.5s of each other
	}
}

public class VectorLine
{
	private Vector3 point;
	private Vector3 unit_direction;
	private float speed;

	public Vector3 GetPoint()
	{
		return point;
	}

	public Vector3 GetUnitDirection()
	{
		return unit_direction;
	}

	public float GetSpeed()
	{
		return speed;
	}

	public VectorLine(Vector3 P, Vector3 V)
	{
		point = P;
		unit_direction = V.normalized;
		speed = Mathf.Pow(V.sqrMagnitude, 0.5f);
	}

	public bool VectorOnLineFront(Vector3 v)
	{
		Debug.Log("v: "+v.ToString());
		float lambda1, lambda2, lambda3;
		lambda1 = (v.x - point.x) / unit_direction.x;
		//lambda2 = (v.y - point.y) / unit_direction.y;	//y = 0
		lambda3 = (v.z - point.z) / unit_direction.z;

		Debug.Log("l1: " + lambda1 /*+ " l2: " + lambda2*/ + " l3: " + lambda3);
		if(/*(lambda1 == lambda2) && (lambda2 == lambda3) &&*/ (lambda1 == lambda3)){
			return lambda1 >= 0;
		};
		return false;
	}

	public static float[] GetIntersectConstants(VectorLine self, VectorLine other)
	{
		Vector3 a = self.point;
		Vector3 d = self.unit_direction;

		Vector3 b = other.point;
		Vector3 e = other.unit_direction;

		Debug.Log("a: " + a.ToString() +" d: " + d.ToString());
		Debug.Log("b: " + a.ToString() + " e: " + d.ToString());
		float lambda, mu;   //where lambda is self's constant, mu is other's constant
		float lambda_numerator = (e.x * (a.z - b.z)) - (e.z * (a.x - b.x));
		float lambda_denominator = (e.z * d.x) - (e.x * d.z);
		lambda = lambda_numerator / lambda_denominator;

		float mu_numerator = a.z - b.z + (lambda * d.z);
		mu = mu_numerator / e.z;

		float[] ret = new float[2] { lambda, mu };
		Debug.Log("lambda: " + lambda + ", mu: " + mu);

		return ret;
	}

	public static Vector3 GetIntersectionPoint(VectorLine self, VectorLine other)
	{
		float[] constants = GetIntersectConstants(self, other);
		Vector3 intersect = self.point + self.unit_direction * constants[0];    //a+ lambda * d
		Vector3 intersect2 = other.point + other.unit_direction * constants[1];
		Debug.Log("intersect 1: " + intersect.ToString());
		Debug.Log("intersect 2: " + intersect2.ToString());

		return intersect;
	}
}
