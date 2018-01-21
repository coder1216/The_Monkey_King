using UnityEngine;
using System.Collections;

public class MySpawner : MonoBehaviour
{
	public float spawnTime = 5f;		// The amount of time between each spawn.
	public float spawnDelay = 3f;		// The amount of time before spawning starts.
	public GameObject[] enemies;		// Array of enemy prefabs.
	public int numToSpawn;			// num of enemies to be spawned
	public GameObject gameCtrl;			// reference to the gameControl script


	private bool stopped = false;
	private int curSpawns;				// the currentNum of the enemies spawned
	void Start ()
	{
		curSpawns = 0;

	}

	void Update(){
		if (curSpawns >= numToSpawn) {
			CancelInvoke ();
		}
	}

	public void StartSpawn(){
		// spawn enemy until the num reach the limitaion
		if (curSpawns < numToSpawn)
			InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}


	void Spawn ()
	{
		if (!stopped) {
			// Instantiate a random enemy.
			int enemyIndex = Random.Range (0, enemies.Length);

			GameObject obj = Instantiate (enemies [enemyIndex], transform.position, transform.rotation) as GameObject;
			if (obj != null)
				Debug.Log ("spawned");
			obj.GetComponent<EnemyHealth> ().SetGameCtrl (gameCtrl);
			++curSpawns;
		}
	}

	public void stopSpawn(){
		stopped = true;
	}

	public void resumeSpawn(){
		stopped = false;
	}


}
