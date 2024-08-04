using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{
    public float maxPlayerPoints = 1.0f; // 플레이어 포인트 최대값
    public float pointDeductionRate = 0.0001f; // 플레이어 포인트 프레임당 감소량
    private float playerPoints;  // 플레이어의 포인트

    private void Start()
    {
        playerPoints = maxPlayerPoints; //플레이어 포인트 최대값으로 초기화
    }

    private void Update()
    {
        if (playerPoints > 0)
        {
            playerPoints -= pointDeductionRate; //플레이어 포인트 양수면 감소시킴

            //포인트가 0보다 작아지면 플레이어 오브젝트 파괴
            //하지만 여기서 파괴할 경우 이 플레이어 오브젝트를 참조하는 다른 오브젝트들이 에러를 쏟아냄
            //다른 스크립트에서 플레이어 오브젝트 사망 여부를 판별하고 기록한 후에 Client.cs 스크립트에서 접근할 예정
            /*
            if (playerPoints < 0)
            {
                Destroy(this.gameObject); 
            }
            */
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌 감지");
        Debug.Log(other.gameObject.name);
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Reward"))
        {
            Debug.Log("Reward와 충돌");
            Reward reward = other.gameObject.GetComponent<Reward>();  
            if (reward != null)
            {
                AddPoints(reward.RewardScore);  // 수정된 프로퍼티 사용
                Debug.Log("Reward 객체 삭제");
                Destroy(other.gameObject);
            }
        }
    }

    public void AddPoints(float pointsToAdd)
    {
        playerPoints += pointsToAdd;  // 포인트를 추가
        if(playerPoints > maxPlayerPoints)
        {
            playerPoints = maxPlayerPoints;
        }
        Debug.Log("포인트 추가: " + pointsToAdd + ", 현재 포인트: " + playerPoints);
    }

    public float getPoint()
    {
        return playerPoints;
    }
}
