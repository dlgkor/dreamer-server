using UnityEngine;

public enum RewardColorElement
{
    Red, Blue, Green, Black
}

public class RewardColor : MonoBehaviour
{
    [SerializeField] public RewardColorElement rewardColorElement;

    public Color GetColor()
    {
        switch (rewardColorElement)
        {
            case RewardColorElement.Red:
                return Color.red;
            case RewardColorElement.Blue:
                return Color.blue;
            case RewardColorElement.Green:
                return Color.green;
            case RewardColorElement.Black:
                return Color.black;
            default:
                return Color.white;
        }
    }

    // ApplyColor 메서드: RewardColor에서 색상 정보를 가져와서 적용
    public void ApplyColor()
    {
        var renderer = GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            Material tempMaterial;

            if (renderer.sharedMaterial != null)
            {
                tempMaterial = new Material(renderer.sharedMaterial);
            }
            else
            {
                tempMaterial = new Material(Shader.Find("Standard"));
            }

            tempMaterial.color = GetColor();
            renderer.sharedMaterial = tempMaterial;
        }
    }

    // OnValidate 메서드: 유니티 에디터에서 값이 변경될 때 호출됨
    private void OnValidate()
    {
        ApplyColor();
    }

    // Awake 메서드: 객체가 생성될 때 색상 적용
    private void Awake()
    {
        ApplyColor();
    }
}
