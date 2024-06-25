using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
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
		[SerializeField] private CanvasGroup gameObjectBlacInit;
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
			StartCoroutine(InitializeGame());
		}
		private IEnumerator InitializeGame()
		{
			// Espera a que DownloadRemote termine
			yield return remoteController.DownloadRemote();
        
			// Ahora ejecuta GenerateZones
			GenerateZones();

			PlayerLevelInfo.totalQuestions = 0;
			PlayerLevelInfo.ansCorrectQuestionsPerZone = new int[8];
			PlayerLevelInfo.totalAnsCorrectQuestions = 0;
			PlayerLevelInfo.timePerZone = new float[8];
			PlayerLevelInfo.questionsPerZone = new int[8];
			
			PlayerLevelInfo.ansCorrectQuestionsPerArea = new int[5];
			PlayerLevelInfo.timePerArea = new float[5];
			PlayerLevelInfo.questionsPerArea = new int[5];
			
			enemyController.GenerateEnemies();
			yield return new WaitForSeconds(5f);
			gameObjectBlacInit.DOFade(0, 1);
			yield return new WaitForSeconds(1f);
		}

		public void GetZoneId()
		{
			Debug.Log(zone);
		}
		public void DoFade()
		{
			gameUIController.FadeDeath();
		}
		public void StartChallenge(Zone zone)
		{
			gameUIController.StartChallenge(zone);
		}
		private void GenerateZones()
		{
			zonesToBePlayed = new List<GameObject>();
			Debug.Log(remoteSister.activities.Length);
			zonesInGame = new List<Zone>();
			foreach (var activity in remoteSister.activities)
			{
				foreach (var challenge in activity.challenges)
				{
					Debug.Log(activity.learningActivity);
					if (activity.learningActivity.StartsWith("A"))
					{
						if (int.TryParse(activity.learningActivity.Substring(1), out int zoneIndex) && zoneIndex <= zones.Length)
						{
							Debug.Log("zoneIndex");
							Debug.Log(zoneIndex);
							zonesToBePlayed.Add(zones[zoneIndex-1]);
						}
					}
				}
				
			}
			Debug.Log("zonesToBePlayed.Count");
			
			Debug.Log(zonesToBePlayed.Count);
			

			float originX = 0;
			float originY = 0;
			int auxNumOfExercises = remoteSister.totalActivities;
			GameObject lastZone = null;
			for (int i = 0; i < remoteSister.totalActivities; i++)
			{
				
				int id = Random.Range(0, zonesToBePlayed.Count);

				originX += zonesToBePlayed[id].transform.GetScaleX() / 2;
				originY = zonesToBePlayed[id].transform.GetScaleY() / 2;
				GameObject goZone= Instantiate(zonesToBePlayed[id], new Vector2(originX, originY), Quaternion.identity);
				zonesInGame.Add(goZone.GetComponent<Zone>());
				
				Zone zoneComponent = goZone.GetComponent<Zone>();
				zoneComponent.HideZone();
				Debug.Log(zoneComponent.ZoneID);
			
				originX += zonesToBePlayed[id].transform.GetScaleX() / 2;
				if (i == remoteSister.totalActivities - 1)
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
				lastZone.GetComponent<Zone>().DestroyPortal();
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
			trigger.size = new Vector2(1, 30);
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
