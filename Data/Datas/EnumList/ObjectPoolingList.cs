using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ObjectPoolingList
{
	None = -1,
	
    [InspectorName("UIs/DungeonDetailConditionUITask")]
    DungeonDetailConditionUITask = 0,
    [InspectorName("UIs/GlobalNotifer")]
    GlobalNotifer = 1,
    [InspectorName("UIs/GlobalSimpleNotifer")]
    GlobalSimpleNotifer = 2,
    [InspectorName("UIs/GlobalBattleNotifer")]
    GlobalBattleNotifer = 3,
    [InspectorName("UIs/ItemGainNotifer")]
    ItemGainNotifer = 4,
    [InspectorName("UIs/ItemInfomationBorderLineTask")]
    ItemInfomationBorderLineTask = 5,
    [InspectorName("UIs/ItemInfomationLargeTask")]
    ItemInfomationLargeTask = 6,
    [InspectorName("UIs/ItemInfomationNormalTask")]
    ItemInfomationNormalTask = 7,
    [InspectorName("UIs/ItemInfomationSmallTask")]
    ItemInfomationSmallTask = 8,
    [InspectorName("UIs/FloatingDamageText")]
    FloatingDamageText = 9,
    [InspectorName("UIs/DungeonSelectTask")]
    DungeonSelectTask = 10,
    [InspectorName("UIs/DungeonSelectArea")]
    DungeonSelectArea = 11,
    [InspectorName("UIs/DungeonChapterTask")]
    DungeonChapterTask = 12,
    [InspectorName("UIs/DungeonItemReward")]
    DungeonItemReward = 13,
    [InspectorName("EnemyAI/Enemy_Orc")]
    Enemy_Orc = 14,
    [InspectorName("EnemyAI/Rush_Enemy_Orc")]
    Rush_Enemy_Orc = 15,
    [InspectorName("EnemyAI/Enemy1_Skeleton_Archor")]
    Enemy1_Skeleton_Archor = 16,
    [InspectorName("EnemyAI/Enemy2_Skeleton")]
    Enemy2_Skeleton = 17,
    [InspectorName("EnemyAI/Enemy3_Skeleton")]
    Enemy3_Skeleton = 18,
    [InspectorName("EnemyAI/Enemy4_Wolf")]
    Enemy4_Wolf = 19,
    [InspectorName("EnemyAI/Enemy5_RockGolem")]
    Enemy5_RockGolem = 20,
    [InspectorName("EnemyAI/Enemy6_ForestMan1")]
    Enemy6_ForestMan1 = 21,
    [InspectorName("EnemyAI/Enemy6_ForestMan2")]
    Enemy6_ForestMan2 = 22,
    [InspectorName("EnemyAI/Enemy6_ForestMan3_Archor")]
    Enemy6_ForestMan3_Archor = 23,
    [InspectorName("EnemyAI/Enemy18_Rome_Archor")]
    Enemy18_Rome_Archor = 24,
    [InspectorName("EnemyAI/Enemy8_Rome_FemaleCitizen1")]
    Enemy8_Rome_FemaleCitizen1 = 25,
    [InspectorName("EnemyAI/Enemy13_Rome_FemaleCitizen2")]
    Enemy13_Rome_FemaleCitizen2 = 26,
    [InspectorName("EnemyAI/Enemy15_Rome_FemaleCitizen3")]
    Enemy15_Rome_FemaleCitizen3 = 27,
    [InspectorName("EnemyAI/Enemy10_Rome_Hero")]
    Enemy10_Rome_Hero = 28,
    [InspectorName("EnemyAI/Enemy9_Rome_MaleCitizen1")]
    Enemy9_Rome_MaleCitizen1 = 29,
    [InspectorName("EnemyAI/Enemy11_Rome_MaleCitizen2")]
    Enemy11_Rome_MaleCitizen2 = 30,
    [InspectorName("EnemyAI/Enemy14_Rome_MaleCitizen3")]
    Enemy14_Rome_MaleCitizen3 = 31,
    [InspectorName("EnemyAI/Enemy16_Rome_MaleCitizen4")]
    Enemy16_Rome_MaleCitizen4 = 32,
    [InspectorName("EnemyAI/Enemy7_Rome_ParadinType1")]
    Enemy7_Rome_ParadinType1 = 33,
    [InspectorName("EnemyAI/Enemy12_Rome_ParadinType2")]
    Enemy12_Rome_ParadinType2 = 34,
    [InspectorName("EnemyAI/Enemy17_Rome_ParadinHero")]
    Enemy17_Rome_ParadinHero = 35,
    [InspectorName("EnemyAI/Zombie1")]
    Zombie1 = 36,
    [InspectorName("EnemyAI/Zombie2")]
    Zombie2 = 37,
    [InspectorName("EnemyAI/Zombie3")]
    Zombie3 = 38,
    [InspectorName("EnemyAI/Zombie4")]
    Zombie4 = 39,
    [InspectorName("EnemyAI/Zombie5")]
    Zombie5 = 40,
    [InspectorName("EnemyAI/Zombie6")]
    Zombie6 = 41,
    [InspectorName("EnemyAI/Zombie7")]
    Zombie7 = 42,
    [InspectorName("EnemyAI/Zombie8")]
    Zombie8 = 43,
    [InspectorName("EnemyAI/Zombie9")]
    Zombie9 = 44,
    [InspectorName("EnemyAI/Zombie10")]
    Zombie10 = 45,
    [InspectorName("BossAI/Boss1_DarkKnight")]
    Boss1_DarkKnight = 46,
    [InspectorName("BossAI/Boss2_RockGolem")]
    Boss2_RockGolem = 47,
    [InspectorName("BossAI/Boss3_Anubis")]
    Boss3_Anubis = 48,
    [InspectorName("BossAI/Boss4_Type1_Mars")]
    Boss4_Type1_Mars = 49,
    [InspectorName("BossAI/Bos4_Type2_Vulcanus")]
    Bos4_Type2_Vulcanus = 50,
    [InspectorName("BossAI/Boss5_Zombie")]
    Boss5_Zombie = 51,
    [InspectorName("BossAI/Boss6_Zombie")]
    Boss6_Zombie = 52,
    [InspectorName("BossAI/Boss7_Archor_Diana")]
    Boss7_Archor_Diana = 53,
    [InspectorName("ProtectedAI/Rescuer_Enemy_Orc")]
    Rescuer_Enemy_Orc = 54,
    [InspectorName("ProtectedAI/FollowProtected_Enemy_Orc")]
    FollowProtected_Enemy_Orc = 55,
    [InspectorName("ProtectedAI/ProtectAI_2")]
    ProtectAI_2 = 56,
    [InspectorName("ProtectedAI/Stand_AI")]
    Stand_AI = 57,
    [InspectorName("ProtectedAI/ProtectAI_1")]
    ProtectAI_1 = 58,
    [InspectorName("PlayerableAI/PlayableAI_1_Runner")]
    PlayableAI_1_Runner = 59,
    [InspectorName("PlayerableAI/PlayableAI_2_Intermediate")]
    PlayableAI_2_Intermediate = 60,
    [InspectorName("PlayerableAI/PlayableAI_3_Expert")]
    PlayableAI_3_Expert = 61,
    [InspectorName("PlayerableAI/PlayableAI_4_Master")]
    PlayableAI_4_Master = 62,
    [InspectorName("PlayerableAI/PlayableAI_5_GrandMaster_Ver1")]
    PlayableAI_5_GrandMaster_Ver1 = 63,
    [InspectorName("PlayerableAI/PlayableAI_5_GrandMaster_Ver2")]
    PlayableAI_5_GrandMaster_Ver2 = 64,
    [InspectorName("PlayerableAI/PlayableAI_6_Magician")]
    PlayableAI_6_Magician = 65,
    [InspectorName("Projectile/CounterReflect")]
    CounterReflect = 66,
    [InspectorName("DungeonObject/DungeonBarrier")]
    DungeonBarrier = 67,
    [InspectorName("GameObjects/ItemSpawnObject")]
    ItemSpawnObject = 68,
    [InspectorName("AI_ETC/StandTower")]
    StandTower = 69,
    [InspectorName("RescuerAI/Female1_RescuerAI")]
    Female1_RescuerAI = 70,
    [InspectorName("RescuerAI/Female2_RescuerAI")]
    Female2_RescuerAI = 71,
    [InspectorName("RescuerAI/Male1_RescuerAI")]
    Male1_RescuerAI = 72,
    [InspectorName("NPC/NPC_Normal_Citizen1")]
    NPC_Normal_Citizen1 = 73,
    [InspectorName("NPC/NPC_Normal_Citizen2")]
    NPC_Normal_Citizen2 = 74,
    [InspectorName("NPC/NPC_Normal_Citizen3")]
    NPC_Normal_Citizen3 = 75,
    [InspectorName("NPC/NPC_Normal_Citizen4")]
    NPC_Normal_Citizen4 = 76,
    [InspectorName("NPC/NPC_Normal_Citizen5")]
    NPC_Normal_Citizen5 = 77,
    [InspectorName("NPC/NPC_Normal_Citizen6")]
    NPC_Normal_Citizen6 = 78,
    [InspectorName("NPC/NPC_Normal_Citizen7")]
    NPC_Normal_Citizen7 = 79,
    [InspectorName("NPC/NPC_Normal_Citizen8")]
    NPC_Normal_Citizen8 = 80,
    [InspectorName("NPC/NPC_Store_1")]
    NPC_Store_1 = 81,
    [InspectorName("NPC/NPC_Store_2")]
    NPC_Store_2 = 82,
    [InspectorName("NPC/NPC_Store_3")]
    NPC_Store_3 = 83,
    [InspectorName("NPC/NPC_Store_4")]
    NPC_Store_4 = 84,
    [InspectorName("NPC/NPC_Knight1_Runner")]
    NPC_Knight1_Runner = 85,
    [InspectorName("NPC/NPC_Knight2_Intermediate")]
    NPC_Knight2_Intermediate = 86,
    [InspectorName("NPC/NPC_Knight3_GrandMaster")]
    NPC_Knight3_GrandMaster = 87,
    [InspectorName("ScareCrow/Scarecrow_NormalScarecrow1")]
    Scarecrow_NormalScarecrow1 = 88,
    [InspectorName("ScareCrow/Scarecrow_RockGolem1")]
    Scarecrow_RockGolem1 = 89,
    [InspectorName("ScareCrow/Scarecrow_Shooter")]
    Scarecrow_Shooter = 90,
    [InspectorName("ScareCrow/Scarecrow_StandingBoss")]
    Scarecrow_StandingBoss = 91,
    [InspectorName("Console/ConsoleCommandTask")]
    ConsoleCommandTask = 92,
    [InspectorName("Console/SearchableTextTask")]
    SearchableTextTask = 93,
    [InspectorName("Test/TestCamera_Environment")]
    TestCamera_Environment = 94,
    [InspectorName("Test/TestPlayerMove_Environment")]
    TestPlayerMove_Environment = 95,

}