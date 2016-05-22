using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject[] hazard;
	public Vector3 spawnValues;
	public int minHazards, maxHazards;
	public float spawn_interval, start_interval, wave_interval;
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameoverText;

	private bool bGameover;
	private bool bRestart;
	private int score;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R) && bRestart)
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(start_interval);
		while (true)
		{
			int nHazards = Random.Range(minHazards, maxHazards);

			for (int i = 0; i < nHazards; i++)
			{
				Vector3 spawnPos = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRot = Quaternion.identity;
				int j = Random.Range(0, hazard.Length);
				Instantiate(hazard[j], spawnPos, spawnRot);
				yield return new WaitForSeconds(spawn_interval);
			}
			yield return new WaitForSeconds(wave_interval);

			if (bGameover)
			{
				break;
			}
		}
	}

	public void GameOver()
	{
		gameoverText.text = "GAME OVER";
		bGameover = true;
		restartText.text = "PRESS 'R' TO RESTART";
		bRestart = true;
	}

	void Start()
	{
		//SpawnWaves();
		bGameover = false;
		bRestart = false;
		gameoverText.text = "";
		restartText.text = "";
		StartCoroutine(SpawnWaves());
		score = 0;
		StartCoroutine(UpdateScore());

	}

	public void addScore(int newScore)
	{
		score += newScore;
		//Debug.Log("newScore:"+newScore * scoreScale+", score:"+score);
		StartCoroutine(UpdateScore());
	}

	/*void UpdateScore()
	{
		scoreText.text = score.ToString();
	}
	*/
	IEnumerator UpdateScore()
	{
		scoreText.text = "";
		yield return new WaitForSeconds(0.1f);
		scoreText.text = score.ToString();
		yield return new WaitForSeconds(0.1f);
		scoreText.text = "";
		yield return new WaitForSeconds(0.1f);
		scoreText.text = score.ToString();
	}
}
