using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject asteroid_explosion;
	public GameObject player_explosion;
	private bool GodMode = false;
	private GameController gameController;
	public int scoreScale;
	public GUIText addScore;
	//public float dampen;		//energy loss during collision;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Asteroid") return;
#if false //collision stuff for asteroids
		if (other.tag == "Asteroid")
		{
			//do collision kinematics
			Vector3 otherVel = other.GetComponent<Rigidbody>().velocity * dampen;
			float massRatio = 1;
			GetComponent<Rigidbody>().velocity += (massRatio * otherVel);
			return;
		}
#endif
		if (other.tag == "Player" && !GodMode)
		{
			Instantiate(player_explosion, transform.position, transform.rotation);
			gameController.GameOver();
		}else
		{
			int score = (int)(scoreScale * GetComponent<Rigidbody>().transform.localScale.magnitude);
			gameController.addScore(score);
			ShowAddedScore(score);
		}
		Instantiate(asteroid_explosion, transform.position, transform.rotation);
		if (other.tag != "Player" || !GodMode)
		{ Destroy(other.gameObject); }
		Destroy(gameObject);

	}

	void ShowAddedScore(int score)
	{
		string updateText = "+" + score;
		addScore.text = updateText;
		//Debug.Log("text:" + addScore.text.ToString());
		Instantiate(addScore, Camera.main.WorldToViewportPoint(transform.position),Quaternion.identity);
	}
}
