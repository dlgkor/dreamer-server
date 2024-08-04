using UnityEngine;

public enum RewardColor
{
    Red, Blue, Green, Black
}

public class RewardColorComponent : MonoBehaviour
{
    [SerializeField] private RewardColor rewardColor;

    public Color GetColor()
    {
        switch (rewardColor)
        {
            case RewardColor.Red:
                return Color.red;
            case RewardColor.Blue:
                return Color.blue;
            case RewardColor.Green:
                return Color.green;
            case RewardColor.Black:
                return Color.black;
            default:
                return Color.white;
        }
    }

// ApplyColor 메서드: RewardColorComponent에서 색상 정보를 가져와서 적용
    
    private void OnValidate()
    {
        ApplyColor();
    }
    
 // OnValidate 메서드 추가: 유니티 에디터에서 값이 변경될 때 호출됨
    private void ApplyColor()
    {
        var renderer = GetComponent<MeshRenderer>();
        var tempMaterial = new Material(renderer.sharedMaterial);

        if (renderer != null)
        {
            tempMaterial.color = GetColor();
            renderer.sharedMaterial = tempMaterial;
        }
    }
}
