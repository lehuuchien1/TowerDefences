using UnityEngine;

public enum SkillType
{
    Invisibility,
    Revival,
    // Thêm các loại kỹ năng khác ở đây
}
[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public SkillType skillType; // Loại kỹ năng
    public float effectDuration; // Thời gian hiệu ứng
    public float revivalMultiplier; // Hệ số buff (dùng cho kỹ năng Revival)
    public float cooldown; // Thời gian hồi chiêu
    public float activationHealthThreshold; // Ngưỡng máu kích hoạt kỹ năng (nếu cần)
}
