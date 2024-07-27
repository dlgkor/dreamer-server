using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField] private float rewardScore;  // 리워드의 점수

    public float RewardScore
    {
        get { return rewardScore; }
        set { rewardScore = value; }
    }
}
