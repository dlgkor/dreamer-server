using UnityEngine;
using System.Collections.Generic;

public class RewardGenerator : MonoBehaviour
{
    public GameObject rewardPrefab; // Reward 프리팹
    public int maxRewards = 10; // 최대 생성 가능한 Reward 개수
    public float minTime = 1.0f; // 최소 생성 간격
    public float maxTime = 5.0f; // 최대 생성 간격
    public Vector3 spawnAreaMin; // 생성 영역 최소값
    public Vector3 spawnAreaMax; // 생성 영역 최대값

    private List<GameObject> rewards = new List<GameObject>();
    private float nextSpawnTime;
    private int rewardCounter = 0; // 보상 고유 번호를 위한 카운터

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            // 현재 존재하는 Reward의 개수를 확인하고, 최대 개수보다 적으면 생성
            if (rewards.Count < maxRewards)
            {
                SpawnReward();
            }
            ScheduleNextSpawn();
        }
    }

    // 다음 Reward 생성 시간 관련
    void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(minTime, maxTime);
    }

    void SpawnReward()
    {
        // Reward 생성 위치
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );

        GameObject reward = Instantiate(rewardPrefab, spawnPosition, Quaternion.identity); //Reward Prefab Instantiate 하기
        rewards.Add(reward);

        // 보상 고유 번호 증가 및 설정
        rewardCounter++;
        int rewardID = rewardCounter;

        // 생성 시각과 위치를 콘솔에 출력
        float spawnTime = Time.time;
        //Debug.Log($"Reward{rewardID} 생성: 위치({spawnPosition.x}, {spawnPosition.y}, {spawnPosition.z}), 시각: {spawnTime}");

        // RewardLifetime 스크립트를 Reward에 추가하여, 삭제될 때 리스트에서 제거하도록 설정
        RewardLifetime rewardLifetime = reward.GetComponent<RewardLifetime>();
        if (rewardLifetime != null)
        {
            rewardLifetime.OnDestroyEvent += (deleteType) =>
            {
                rewards.Remove(reward);
                float destroyTime = Time.time;
                //Debug.Log($"Reward{rewardID} 삭제: 생성 시각: {spawnTime}, 삭제 시각: {destroyTime}, 삭제 원인: {deleteType}");
            };
            rewardLifetime.spawnTime = spawnTime; // RewardLifetime에 생성 시각 전달
        }
    }
}
