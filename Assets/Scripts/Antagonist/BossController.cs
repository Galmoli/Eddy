using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Wave[] bossWaves;
    public AntagonistBossFSM antagonistBossFSM;
    public AntagonistEndingFSM antagonistEndingFSM;
    public Transform[] pipePositions;

    public GameObject bossCollider1, bossCollider2;

    bool activate;
    int currentWave;

    // Start is called before the first frame update
    void Start()
    {
        bossCollider1.SetActive(true);
        bossCollider2.SetActive(true);
        activate = false;
        currentWave = 0;
        StartCoroutine(WaveProducer());
    }

    public void StartWaveCorutine()
    {
        if (currentWave < bossWaves.Length)
        {
            StartCoroutine(WaveProducer());
        }
        else
        {
            bossCollider1.SetActive(false);
            bossCollider2.SetActive(false);
            antagonistBossFSM.enabled = false;
            antagonistEndingFSM.enabled = true;
        }
    }

    public IEnumerator WaveProducer()
    {
        for (int i = 0; i < bossWaves[currentWave].enemies.Length; i++)
        {
            int randomPipe = Random.Range(0, pipePositions.Length);
            GameObject go = Instantiate(bossWaves[currentWave].enemies[i], pipePositions[randomPipe].transform.position, Quaternion.identity);
            ChargingEnemyBlackboard chargingEnemyBlackboard = go.GetComponent<ChargingEnemyBlackboard>();
            ThrowHandsEnemyBlackboard throwHandsEnemyBlackboard = go.GetComponent<ThrowHandsEnemyBlackboard>();

            if (chargingEnemyBlackboard)
            {
                chargingEnemyBlackboard.detectionDistanceOnSight = 50;
                chargingEnemyBlackboard.detectionDistanceOffSight = 50;
                chargingEnemyBlackboard.playerOutOfRangeDistance = 100;
            }

            if (throwHandsEnemyBlackboard)
            {
                throwHandsEnemyBlackboard.detectionDistanceOnSight = 50;
                throwHandsEnemyBlackboard.detectionDistanceOffSight = 50;
                throwHandsEnemyBlackboard.playerOutOfRangeDistance = 100;
            }

            if (i < bossWaves[currentWave].enemies.Length-1)
                yield return new WaitForSeconds(bossWaves[currentWave].interval);
        }

        yield return new WaitForSeconds(bossWaves[currentWave].duration);

        currentWave++;

        antagonistBossFSM.ChangeState(AntagonistBossFSM.States.FALLING);
    }

}
