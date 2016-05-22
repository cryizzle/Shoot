using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour {

	public TextAsset levelCSV;

	private SortedDictionary<int,Level> levels;
	public SortedDictionary<int,Level> GetLevels() { return levels; }
	public Level GetLevel(int level) {
		if (levels.ContainsKey(level))
		{
			return levels[level];
		}
		return null; }

	public void Parse<T>(TextAsset csvFile, SortedDictionary<int,T> container) where T: GameData, new()
	{
		container.Clear();
		string[] lines = csvFile.text.Split('\n');
		for (int i=1; i< lines.Length;i++)
		{
			if(lines[i].Length == 0)
			{
				continue;
			}
			//Debug.Log("line: "+lines[i]);
			T g = new T();
			g.ParseLine(lines[i]);
			//Debug.Log("g type: "+g.GetType().Name);
			container.Add(g.GetID(), g);
			//Debug.Log("container size: " + container.Count);
		}

	}
	void Start()
	{
		levels = new SortedDictionary<int, Level>();
		Parse<Level>(levelCSV,levels);
	}
	
}