using UnityEngine;

public class PlayerFitness : MonoBehaviour
{
    // 초기값이 0인 float 자료형 fitness 멤버변수 생성
    public float fitness = 0.0f;

    // Update 멤버함수에서 매 프레임마다 0.001 더해줌
    void Update()
    {
        fitness += 0.001f;
    }
}
