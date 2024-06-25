using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class Zone : MonoBehaviour
    {
        [SerializeField] private int zoneId;
        [SerializeField] private GameObject initialAnim;
        [SerializeField] private GameObject finishedAnim;
        [SerializeField] private GameObject finishedCrocAnim;
        [SerializeField] private GameObject challengeBlock;
        [SerializeField] private eEnemyType enemyType;
        [SerializeField] private AnimationData enemyData;
        [SerializeField] private List<Transform> spawnEnemyPositions;
        [SerializeField] private GameObject blackZone;
        [SerializeField] private GameObject blackZoneLast;
        [SerializeField] private GameObject environmentEffect;
        [SerializeField] private GameObject blackPortal;
        [SerializeField] private GameObject portal;
        public int ZoneID => zoneId;
        public GameObject ChallengeBlock => challengeBlock;
        public GameObject InitialAnim => initialAnim;
        public GameObject FinishedAnim => finishedAnim;
        public GameObject FinishedCrocAnim => finishedCrocAnim;
        public GameObject BlackZone => blackZone;
        public GameObject EnvironmentEffect => environmentEffect;
        public GameObject BlackZoneLast => blackZoneLast;
        public AnimationData EnemyData => enemyData;
        public eEnemyType EnemyType => enemyType;
        public  List<Transform> SpawnEnemyPositions => spawnEnemyPositions;
        public void HideZone()
        {
            if (blackZone != null)
            {
                blackZone.SetActive(true);
                
            }
        }
       

        public void ShowZone()
        {
            
            if (blackZone != null)
            {
                blackZoneLast.SetActive(true);
                blackZone.SetActive(false);
                if (environmentEffect != null)
                {
                    environmentEffect.SetActive(false);    
                }
                
            }
        }
        public void DestroyPortal()
        {
            blackZone.SetActive(false);
            blackZoneLast.SetActive(false);
            portal.GetComponent<BoxCollider2D>().enabled = false;
        }
        public void ShowEnvEffects()
        {
            if (environmentEffect != null)
            {
                EnvironmentEffect.SetActive(false);    
            }
            
        }
    }
}