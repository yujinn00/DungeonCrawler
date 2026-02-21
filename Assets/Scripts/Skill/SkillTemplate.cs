using System.Collections.Generic;
using UnityEngine;

// 스킬의 종류.
public enum SkillType { Buff = 0, Reinforce }

// 스킬의 속성.
public enum SkillElement { None = -1, Fire = 100, Ice, Wind, Light, Dark }

[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillAsset", order = 0)]
public class SkillTemplate : ScriptableObject
{
    [Header("Common")]
    public string skillName;                            // 스킬의 이름.
    public SkillType skillType;                         // 스킬의 타입.
    public SkillElement skillElement;                   // 스킬의 속성.
    public int maxLevel;                                // 스킬의 최대 레벨.
    [TextArea(1, 30)] public string description;        // 스킬의 상세 설명.
    public Sprite disableIcon;                          // 스킬을 배우기 전에 보여줄 아이콘 이미지.
    public Sprite enableIcon;                           // 스킬을 배운 후에 보여줄 아이콘 이미지.

    [Header("Buff")]
    public List<Stat> buffStatList;                     // 버프 스킬이 올려줄 스탯의 목록.
    
    [Header("Reinforce")]
    public List<Stat> reinforceStatList;                // 강화 스킬이 올려줄 스탯의 목록.
}
