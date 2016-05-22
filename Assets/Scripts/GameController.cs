using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private bool debug = false;

	public GameObject[] hazard;
	public Vector3 spawnValues;
	public LevelController levelController;
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameoverText;

	public float start_interval;
#if debug
	public int minHazards, maxHazards, waves;
	public float spawn_interval, wave_interval, spawn_speed
#else
	private int minHazards, maxHazards, waves;
	private float spawn_interval, wave_interval, spawn_speed;
#endif

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
		if (levelController.IsStart())
		{
			yield return new WaitForSeconds(start_interval);
		}
		while (InitialiseLevel())
		{
			if (bGameover)
			{
				break;
			}
			for (int k = 0; k < waves; k++)
			{   //how many waves
				int nHazards = Random.Range(minHazards, maxHazards);

				if (bGameover)
				{
					break;
				}

				for (int i = 0; i < nHazards; i++)
				{   //spawn for each wave
					Vector3 spawnPos = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
					Quaternion spawnRot = Quaternion.identity;
					int j = Random.Range(0, hazard.Length);
					GameObject asteroid = Instantiate(hazard[j], spawnPos, spawnRot) as GameObject;
					asteroid.GetComponent<Rigidbody>().velocity *= spawn_speed;
					yield return new WaitForSeconds(spawn_interval);
				}
				yield return new WaitForSeconds(wave_interval);
			}
			levelController.IncrementLevel();
			if (levelController.FinishedLastLevel())
			{
				GameFinished();
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

	public void GameFinished()
	{
		gameoverText.text = "YOU WIN";
		bGameover = true;
		restartText.text = "PRESS 'R' TO RESTART";
		bRestart = true;
	}

	void Start()
	{
		//SpawnWaves();
		levelController.RestartLevel();
		bGameover = false;
		bRestart = false;
		gameoverText.text = "";
		restartText.text = "";
		StartCoroutine(SpawnWaves());
		score = 0;
		StartCoroutine(UpdateScore());

	}

	bool InitialiseLevel()
	{
#if !debug
		Level data = levelController.GetLevelData();
		if(data == null)
		{
			return false;
		}
		minHazards = data.GetMinHazards();
		maxHazards = data.GetMaxHazards();
		spawn_interval = data.GetSpawnInterval();
		wave_interval = data.GetWaveInterval();
		spawn_speed = data.GetSpeed();
		waves = data.GetWaves();
		return true;
#endif
	}

	public void addScore(int newScore)
	{
		score += newScore;
		//Debug.Log("newScore:"+newScore * scoreScale+", score:"+score);
		StartCoroutine(UpdateScore());
	}

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
