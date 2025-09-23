using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PotentialList
{
	None = -1,
	
    [InspectorName("Common/COMMON_STR_PERCENTAGE")]
    COMMON_STR_PERCENTAGE = 0,
    [InspectorName("Common/COMMON_DEX_PERCENTAGE")]
    COMMON_DEX_PERCENTAGE = 1,
    [InspectorName("Common/COMMON_INT_PERCENTAGE")]
    COMMON_INT_PERCENTAGE = 2,
    [InspectorName("Common/COMMON_LUCK_PERCENTAGE")]
    COMMON_LUCK_PERCENTAGE = 3,
    [InspectorName("Weapon/WEAPON_ATK")]
    WEAPON_ATK = 4,
    [InspectorName("Weapon/WEAPON_ATK_PERCENTAGE")]
    WEAPON_ATK_PERCENTAGE = 5,
    [InspectorName("Common/COMMON_STR")]
    COMMON_STR = 6,
    [InspectorName("Common/COMMON_DEX")]
    COMMON_DEX = 7,
    [InspectorName("Common/COMMON_INT")]
    COMMON_INT = 8,
    [InspectorName("Common/COMMON_LUCK")]
    COMMON_LUCK = 9,
    [InspectorName("Armor/ARMOR_HEALTH")]
    ARMOR_HEALTH = 10,
    [InspectorName("Armor/ARMOR_HEALTH_PERCENTAGE")]
    ARMOR_HEALTH_PERCENTAGE = 11,
    [InspectorName("Accessory/ACCESSORY_ALLSTATS")]
    ACCESSORY_ALLSTATS = 12,
    [InspectorName("Accessory/ACCESSORY_ALLSTATS_PERCENTAGE")]
    ACCESSORY_ALLSTATS_PERCENTAGE = 13,
    [InspectorName("Accessory/ACCESSORY_HEALTH")]
    ACCESSORY_HEALTH = 14,
    [InspectorName("Accessory/ACCESSORY_HEALTH_PERCENTAGE")]
    ACCESSORY_HEALTH_PERCENTAGE = 15,
    [InspectorName("Accessory/ACCESSORY_CRITICAL_CAHNCE")]
    ACCESSORY_CRITICAL_CAHNCE = 16,
    [InspectorName("Accessory/ACCESSORY_CRITICAL_DAMAGE")]
    ACCESSORY_CRITICAL_DAMAGE = 17,
    [InspectorName("Title/TITLE_CRITICAL_DAMAGE")]
    TITLE_CRITICAL_DAMAGE = 18,
    [InspectorName("Title/TITLE_CRITICAL_CAHNCE")]
    TITLE_CRITICAL_CAHNCE = 19,
    [InspectorName("Title/TITLE_HEALTH")]
    TITLE_HEALTH = 20,
    [InspectorName("Title/TITLE_HEALTH_PERCENTAGE")]
    TITLE_HEALTH_PERCENTAGE = 21,
    [InspectorName("Accessory/ACCESSORY_ATK")]
    ACCESSORY_ATK = 22,
    [InspectorName("Title/TITLE_ATK")]
    TITLE_ATK = 23,
    [InspectorName("Common/COMMON_HP_REGEN")]
    COMMON_HP_REGEN = 24,
    [InspectorName("Armor/ARMOR_STEMINAREGEN")]
    ARMOR_STEMINAREGEN = 25,
    [InspectorName("Armor/ARMOR_STEMINARE")]
    ARMOR_STEMINARE = 26,
    [InspectorName("Armor/ACCESSORY_STEMINAREGEN")]
    ACCESSORY_STEMINAREGEN = 27,
    [InspectorName("Accessory/ACCESSORY_STEMINARE")]
    ACCESSORY_STEMINARE = 28,
    [InspectorName("Accessory/TITLE_STEMINARE")]
    TITLE_STEMINARE = 29,
    [InspectorName("Title/TITLE_STEMINAREGEN")]
    TITLE_STEMINAREGEN = 30,
    [InspectorName("Weapon/WEAPON_CRITICAL_DAMAGE")]
    WEAPON_CRITICAL_DAMAGE = 31,
    [InspectorName("Weapon/WEAPON_CRITICAL_CAHNCE")]
    WEAPON_CRITICAL_CAHNCE = 32,
    [InspectorName("Armor/ARMOR_DEFENSE")]
    ARMOR_DEFENSE = 33,
    [InspectorName("Accessory/ACCESSORY_DEFENSE")]
    ACCESSORY_DEFENSE = 34,
    [InspectorName("Title/TITLE_DEFENSE")]
    TITLE_DEFENSE = 35,
    [InspectorName("Title/TITLE_MAGIC_DEFENSE")]
    TITLE_MAGIC_DEFENSE = 36,
    [InspectorName("Armor/ARMOR_MAGIC_DEFENSE")]
    ARMOR_MAGIC_DEFENSE = 37,
    [InspectorName("Accessory/ACCESSORY_MAGIC_DEFENSE")]
    ACCESSORY_MAGIC_DEFENSE = 38,

}