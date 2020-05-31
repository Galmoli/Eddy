using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] pipePositions;

    public GameObject[] closeAreaCols;

    bool activate;
    int currentWave;
    List<EnemyBlackboard> currentEnemies = new List<EnemyBlackboard>();

    bool wavesCompleted;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init()
    {
        closeAreaCols[0].SetActive(true);
        closeAreaCols[1].SetActive(true);
        activate = true;
        currentWave = 0;
        StartCoroutine(WaveProducer());
    }

    public void Reset()
    {
        if (!wavesCompleted)
        {
            currentWave = 0;
            closeAreaCols[0].SetActive(false);
            closeAreaCols[1].SetActive(false);
            activate = false;
        }
    }

    private void Update()
    {
        if (activate)
        {
            if (!wavesCompleted)
            {
                if (currentEnemies.Count > 0)
                {
                    for (int i = 0; i < currentEnemies.Count; i++)
                    {
                        if (currentEnemies[i].healthPoints <= 0)
                        {
                            currentEnemies.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    StartWaveCorutine();
                }
            }
        }
    }

    public void StartWaveCorutine()
    {
        if (currentWave < waves.Length)
        {
            StartCoroutine(WaveProducer());
        }
        else
        {
            wavesCompleted = true;
            closeAreaCols[0].SetActive(false);
            closeAreaCols[1].SetActive(false);
        }
    }

    public IEnumerator WaveProducer()
    {
        for (int i = 0; i < waves[currentWave].enemies.Length; i++)
        {
            int randomPipe = Random.Range(0, pipePositions.Length);
            GameObject go = Instantiate(waves[currentWave].enemies[i], pipePositions[randomPipe].transform.position, Quaternion.identity);
            EnemyBlackboard blackboard = go.GetComponent<EnemyBlackboard>();
            ChargingEnemyBlackboard chargingEnemyBlackboard = go.GetComponent<ChargingEnemyBlackboard>();
            ThrowHandsEnemyBlackboard throwHandsEnemyBlackboard = go.GetComponent<ThrowHandsEnemyBlackboard>();

            currentEnemies.Add(blackboard);

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

            if (i < waves[currentWave].enemies.Length - 1)
                yield return new WaitForSeconds(waves[currentWave].interval);
        }

        currentWave++;
    }

    
}

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;
    public float interval;
    public float duration;
}
