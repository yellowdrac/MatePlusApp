using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Game
{
	public class GameController : MonoBehaviour
	{
		private static GameController instance;
		[SerializeField] private GameUIController gameUIController;
		[SerializeField] private RemoteController remoteController;
		[SerializeField] private EnemyController enemyController;
		
		[SerializeField] private GameObject[] zones;
		[SerializeField] private bool[] zonesActivation;
		
		
		[SerializeField] private int numOfExercises;
		
		private RemoteSister remoteSister;
		private Zone zone;
		[SerializeField] private PlayerModel player;
		
		private List<GameObject> zonesToBePlayed;
		private List<Zone> zonesInGame;
		
		
		
		
		void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			remoteController.DownloadRemote();
			GenerateZones();
			enemyController.GenerateEnemies();
		}

		public void StartChallenge(Zone zone)
		{
			gameUIController.StartChallenge(zone);
		}
		private void GenerateZones()
		{
			zonesToBePlayed = new List<GameObject>();
			zonesInGame = new List<Zone>();
			for (int i = 0; i < zones.Length; i++)
			{
				if (zonesActivation[i] == true)
				{
					zonesToBePlayed.Add(zones[i]);
				}
			}

			float originX = 0;
			float originY = 0;
			int auxNumOfExercises = numOfExercises;
			GameObject lastZone = null;
			for (int i = 0; i < numOfExercises; i++)
			{
				int id = Random.Range(0, zonesToBePlayed.Count);

				originX += zonesToBePlayed[id].transform.GetScaleX() / 2;
				originY = zonesToBePlayed[id].transform.GetScaleY() / 2;
				GameObject goZone= Instantiate(zonesToBePlayed[id], new Vector2(originX, originY), Quaternion.identity);
				zonesInGame.Add(goZone.GetComponent<Zone>());
				
				
				
				originX += zonesToBePlayed[id].transform.GetScaleX() / 2;
				if (i == numOfExercises - 1)
				{
					lastZone = goZone;
				}

				if (auxNumOfExercises <= zonesToBePlayed.Count)
				{
					zonesToBePlayed.Remove(zonesToBePlayed[id]);
				}
				auxNumOfExercises--;
			}
			if (lastZone != null)
			{
				AddEndTrigger(lastZone, originX);
			}
			
		}
		private void AddEndTrigger(GameObject lastZone, float finalXPosition)
		{
			// Crear un nuevo GameObject para el trigger
			GameObject endTrigger = new GameObject("EndTrigger");
			endTrigger.transform.position = new Vector2(finalXPosition, lastZone.transform.position.y);
			BoxCollider2D trigger = endTrigger.AddComponent<BoxCollider2D>();
			trigger.isTrigger = true;
			endTrigger.AddComponent<ZoneEndTrigger>();
		}
		public RemoteSister RemoteData
		{
			set { remoteSister = value; }
			get { return remoteSister; }
		}
		public GameUIController GameUIController => gameUIController;
		public List<Zone> ZonesInGame => zonesInGame;
		public PlayerModel Player => player;
		public Zone Zone
		{
			set { zone = value; }
			get { return zone; }
		}
		void OnDestroy()
		{
			instance = null;
		}

		public static GameController Instance
		{
			get { return instance; }
		}
		
	}
}
