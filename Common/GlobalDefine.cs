

#region Item용 Enum Type
/// <summary>
/// EQUIPMENT,  CONSUMABLE, MATERIAL, QUESTITEM,
/// </summary>
public enum ItemCategoryType
{
    EQUIPMENT,
    CONSUMABLE,
    MATERIAL,
    QUESTITEM
}

/// <summary>
///  NONE,  WEAPON, ARMOR, ACCESSORIES, TITLE
/// </summary>
public enum EquipmentTpye
{
    //NONE = -1,
    WEAPON,
    ARMOR,
    ACCESSORIES,
    TITLE,
}


/// <summary>
/// NONE, POSION, ENCHANT
/// </summary>
public enum ConsumableType
{
    //NONE = -1,
    POSION = 0,
    ENCHANT = 1,

}

public enum MaterialType
{
    CRAFT = 0,
    EXTRA = 1,
}

public enum QuestIType
{
    QUEST=0,
}

/// <summary>
///  NONE,  LOW, NORMAL, RARE, EPIC, UNIQUE, LEGENDARY, RELIC, MYTHIC
/// </summary>
public enum ItemRankType
{
    NONE = -1,
    LOW,
    NORMAL,
    RARE,
    EPIC,
    UNIQUE,
    LEGENDARY,
    RELIC,
    MYTHIC,
}


#endregion


#region Potential Enum Type

/// <summary>
///  NONE,  NORMAL, RARE, UNIQUE, LEGENDARY,
/// </summary>
public enum ItemPotentialRankType
{
    NONE = -1,
    NORMAL = 0,
    RARE = 1,
    UNIQUE = 2,
    LEGENDARY = 3,

}



/// <summary>
/// potential clip 타입
/// </summary>
public enum PotentialSelectType
{
    COMMON = 0,
    WEAPON = 1,
    ARMOR = 2,
    ACCESSORY = 3,
    Title = 4,

}


#endregion


#region Editor용 Enum Type


/// <summary>
/// Editor에서 사용되는 enum Type
/// </summary>
public enum AIEditorCategoryType
{
    ALL = 0,
    COMMON = 1,
    ELITE = 2,
    BOSS = 3,

}

/// <summary>
/// Potential Tool 에서 사용되는 구분 enum. NONE,  ALL, COMMON, WEAPON, ARMOR, 
/// </summary>
public enum PotentialEditorCategoryType
{
    NONE = -1,
    ALL = 0,
    COMMON = 1,
    WEAPON = 2,
    ARMOR = 3,
    ACCESSORY = 4,
    Title = 5,
}


/// <summary>
/// item tool 에서 사용되는 아이템 분류용 enum 
/// </summary>
public enum ItemEditorCategoryType
{
    ALL = 0,
    WEAPON = 1,
    ARMOR = 2,
    ACCESSORY = 3,
    TITLE = 4,
    POSION = 5,
    ENCHANT = 6,
    CRAFT = 7,
    EXTRA = 8,
    QUEST = 9,
    EQUIPMENT = 10,
    CONSUMABLE = 11,
    MATERIAL = 12,
}

/// <summary>
/// 아이템 분류용 enum 
/// </summary>
public enum ItemSortType
{
    NONE =-1,
    WEAPON = 0,
    ARMOR = 1,
    ACCESSORY = 2,
    TITLE = 3,
    POSION = 4,
    ENCHANT = 5,
    CRAFT = 6,
    EXTRA = 7,
    QUEST = 8,

    
}


#endregion


#region Sound & Effect Enum Type
public enum SoundPlayType
{
    BGM = 0,
    Effect = 1,
    UI = 2,
    ETC =3,
}


public enum SoundPlayTypeBGM
{
    NONE = -1,
    BATTLE = 0,
    TITLE = 1,
    MAIN = 2,
    DUNGEON = 3,
}

public enum SoundPlayTypeEffect
{
    NONE = -1,
    MONSTER_ATTACK_VOICE = 0,
    MONSTER_ROAR ,
    MONSTER_DIE,
    MONSTER_HURT ,
    MONSTER_FINDNEAR ,
    ATTACK_BOW,
    ATTACK_MAGIC_SLASH,
    ATTACK_SWING,
    BUFF ,
    COUNTER ,
    DEFENSE ,
    DOORS , 
    HIT ,
    PROJECTILE ,
    EXPLOSION,

}

public enum SoundPlayTypeUI
{
    NONE = -1,
    BUTTON = 0,
    CHARACTERS = 1,
    COLLECT = 2,
    HUD =3,
}
public enum SoundPlayTypeETC
{
   NONE = -1,
   BACKGROUND = 0,

}


public enum EffectType
{
    NONE = 0,
    HIT =1,
    PROJECTILE = 2,
    COLLISION = 3,
    SLASH = 4,
    ATTACKSKILL = 5,
    EXPLOSION = 6,
    MAGIC = 7,
    CASTING =8,
    FLASH = 9,
    ETC = 10,
    BUFF = 11,

}


#endregion


#region Inventory Enum Type

/// <summary>
/// Slot에 들어갈수있는지 검사하는 종류 enum 
/// </summary>
public enum SlotAllowType
{
    NONE = 0,
    TITLE,
    HEAD,
    UPPER,
    LOWER,
    HAND,
    LEG,
    EARING,
    BELT,
    BRACELET,
    RING,
    SOUL,
    WEAPON,
    CLOAK,

    POSION,
    ENCHANT,
    CRAFT,
    EXTRA,
    QUESTITEM,
    SKILL,
}


#endregion


#region Camera
public enum CamStrength
{
    NONE = -1,
    WEAK = 0,
    NORMARL = 1,
    STRONG = 2,
    ABSULUTE =3,
}


public enum CamShakeType
{
    NONE = -1,
    SMOOTH_TIME =0,
    SMOOTH_COUNT =1,
    IMMEDIATE_TIME = 2,
    IMMEDIATE_COUNT =3,
    SMOOTH_REDUCE_TIME =4,
    IMMEDIATE_REDUCE_TIME = 5,
    CURVE_Z = 6,
    CURVE_VECTOR3 = 7,
}


#endregion

public enum DirectionType
{
    FRNOT =0,
    BACK = 1,
    RIGHT = 2,
    LEFT = 3,
}


public enum AttackStrengthType
{
    NONE = -1,
    WEAK = 0,
    NORMAL = 1,
    STRONG = 2,
    FLYDOWN = 3,
}

/// <summary>
/// Skill 분류 Enum
/// </summary>
public enum SkillType
{
    NONE =-1,
    ATTACK = 0,
    MAGIC =1,
    BUFF = 2,
    COMBO = 3,
    PASSIVE =4,
    COUNTER =5,
    DASH = 6,

}

/// <summary>
/// AI용 현재 공격 타입 Enum
/// </summary>
public enum CurrentCombatType
{
    NONE = -1,
    ATTACK = 0,
    SKILL = 1,
}

/// <summary>
/// 버프 목록 Enum
/// </summary>
public enum BuffType
{
    HP_BUFF = 0,
    HP_IMMEDIATE_HEAL = 1,
    HP_DURATION_HEAL = 2,
    HP_REGENERATION_BUFF =3,
    STAMINA_BUFF = 4,
    STAMINA_IMMEDIATE_HEAL = 5,
    STAMINA_DURATION_HEAL = 6,
    STAMINA_REGENERATION_BUFF = 7,
    ATK_BUFF = 8,
    ATK_SPEED_BUFF = 9,
    ALLSTATS_BUFF = 10,
    STR_BUFF = 11,
    DEX_BUFF = 12,
    INT_BUFF = 13,
    LUCK_BUFF = 14,
    DEFENSE_BUFF = 15,
    MAGIC_DEFENSE_BUFF =16,
    EVASION_BUFF = 17,
    CRITICAL_CHANCE = 18,
    CRITICAL_DAMAGE = 19,
    SKILL_INCREASE_DAMAGE_PERCENTAGE = 20,
    HIT_PER_HP_HEAL = 21,
}

/// <summary>
/// CombineAndSortEditor에서 사용. 분류할 때 사용.
/// </summary>
public enum SortKeyWordType
{
    AND = 0,
    OR = 1,
    PERFECT =2,
    ANY = 3,
}

