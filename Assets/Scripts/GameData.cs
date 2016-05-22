using UnityEngine;
public interface GameData
{
	int GetID();
	void ParseLine(string line);

}

public class Level : GameData
{
	private int nlevel;
	private float wave_interval;
	private float spawn_interval;
	private int min_Hazards;
	private int max_Hazards;
	private int nWaves;
	private float nSpeed;

	public int GetID() { return nlevel; }		//identify by level
	public int GetLevel() { return nlevel; }
	public float GetWaveInterval() { return wave_interval; }
	public float GetSpawnInterval() { return spawn_interval; }
	public int GetMinHazards() { return min_Hazards; }
	public int GetMaxHazards() { return max_Hazards; }
	public int GetWaves() { return nWaves; }
	public float GetSpeed() { return nSpeed; }

	public Level() {
		nlevel = 1;
		wave_interval = 1;
		spawn_interval = 1;
		min_Hazards = 1;
		max_Hazards = 1;
		nWaves = 1;
		nSpeed = 1;
	}

	public void ParseLine(string line )
	{

		if (line.Length == 0)
		{
			return;
		}
		string[] items = line.Split(',');
		nlevel = int.Parse(items[0]);
		wave_interval = float.Parse(items[1]);
		min_Hazards = int.Parse(items[2]);
		max_Hazards = int.Parse(items[3]);
		nWaves = int.Parse(items[4]);
		nSpeed = float.Parse(items[5]);
		spawn_interval = float.Parse(items[6]);
	}
}
