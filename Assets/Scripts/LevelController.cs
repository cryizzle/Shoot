using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public int startLevel = 1;
	public GameDataManager manager;
	public GUIText levelText;
	private int currentLevel;
	private bool finishedLastLevel;
	public void RestartLevel()
	{
		currentLevel = startLevel;
		finishedLastLevel = false;
		StartCoroutine(UpdateLevel());
	}

	public void IncrementLevel()
	{
		currentLevel++;
		if (currentLevel > GetMaxLevel())
		{
			finishedLastLevel = true;
			return;		//do not update level text
		}
		StartCoroutine(UpdateLevel());
	}

	public bool FinishedLastLevel()
	{
		return finishedLastLevel;
	}
	public Level GetLevelData()
	{
		return manager.GetLevel(currentLevel);
	}

	public int GetLevel()
	{
		return currentLevel;
	}

	public int GetMaxLevel()
	{
		return manager.GetLevels().Count;
	}

	public bool IsStart()
	{
		return startLevel == currentLevel;
	}
	IEnumerator UpdateLevel()
	{
		levelText.text = "";
		yield return new WaitForSeconds(0.1f);
		levelText.text = "LEVEL "+ currentLevel.ToString();
		yield return new WaitForSeconds(0.1f);
		levelText.text = "";
		yield return new WaitForSeconds(0.1f);
		levelText.text = "LEVEL " + currentLevel.ToString();
	}
}
