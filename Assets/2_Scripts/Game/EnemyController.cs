using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    
    public void GenerateEnemies()
    {
        foreach (Zone zone in GameController.Instance.ZonesInGame)
        {
            foreach (Transform spawnPos in zone.SpawnEnemyPositions)
            {
                EnemyModel goEnemy = Instantiate(enemyPrefab,new Vector3(spawnPos.position.x,spawnPos.position.y,0),Quaternion.identity).GetComponent<EnemyModel>();
                goEnemy.SetPlayer(GameController.Instance.Player);
                goEnemy.EnemyRendering.ChangeAnimationData(zone.EnemyData);
                goEnemy.EnemyRendering.PlayAnimation(eAnimation.Idle);
                goEnemy.SetEnemyType(zone.EnemyType);
            }
        }
    }
    
}
