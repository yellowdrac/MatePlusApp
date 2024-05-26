using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Zone : MonoBehaviour
    {
        [SerializeField] private int zoneId;
        [SerializeField] private GameObject initialAnim;
        [SerializeField] private GameObject finishedAnim;
        [SerializeField] private GameObject challengeBlock;
        [SerializeField] private eEnemyType enemyType;
        [SerializeField] private AnimationData enemyData;
        [SerializeField] private List<Transform> spawnEnemyPositions;
        public int ZoneID => zoneId;
        public GameObject ChallengeBlock => challengeBlock;
        public GameObject InitialAnim => initialAnim;
        public GameObject FinishedAnim => finishedAnim;
        public AnimationData EnemyData => enemyData;
        public eEnemyType EnemyType => enemyType;
        public  List<Transform> SpawnEnemyPositions => spawnEnemyPositions;
    }
}