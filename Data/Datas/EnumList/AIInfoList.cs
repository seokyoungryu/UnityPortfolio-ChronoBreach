using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AIInfoList
{
	None = -1,
	
    [InspectorName("Common/TestSkeleton")]
    TestSkeleton = 0,
    [InspectorName("Common/TestAI")]
    TestAI = 1,
    [InspectorName("Common/Test_Ver1")]
    Test_Ver1 = 2,
    [InspectorName("Common/RescuerTest1")]
    RescuerTest1 = 3,
    [InspectorName("Boss/Orc_Boss")]
    Orc_Boss = 4,
    [InspectorName("Common/Test_Ver2")]
    Test_Ver2 = 5,
    [InspectorName("Common/StrongAI")]
    StrongAI = 6,
    [InspectorName("Common/StandingProtect")]
    StandingProtect = 7,
    [InspectorName("Boss/Orc_Boss1")]
    Orc_Boss1 = 8,
    [InspectorName("Common/StrongAI2")]
    StrongAI2 = 9,
    [InspectorName("Common/Enemy2_SkeletonWarrior")]
    Enemy2_SkeletonWarrior = 10,
    [InspectorName("Boss/StandTower")]
    StandTower = 11,
    [InspectorName("Common/Enemy3_SkeletonWarrior")]
    Enemy3_SkeletonWarrior = 12,
    [InspectorName("Common/Enemy4_Wolf")]
    Enemy4_Wolf = 13,
    [InspectorName("Boss/Boss4_Valcanus")]
    Boss4_Valcanus = 14,
    [InspectorName("Common/Rescuer_Female1")]
    Rescuer_Female1 = 15,
    [InspectorName("Common/Rescuer_Female2")]
    Rescuer_Female2 = 16,
    [InspectorName("Common/Rescuer_Male1")]
    Rescuer_Male1 = 17,
    [InspectorName("Common/PlayableAI_Runner")]
    PlayableAI_Runner = 18,
    [InspectorName("Common/PlayableAI_Immediate")]
    PlayableAI_Immediate = 19,
    [InspectorName("Common/CommonZombieType1")]
    CommonZombieType1 = 20,
    [InspectorName("Common/CommonZombieType2")]
    CommonZombieType2 = 21,
    [InspectorName("Common/Zombie3")]
    Zombie3 = 22,
    [InspectorName("Common/Zombie4")]
    Zombie4 = 23,
    [InspectorName("Common/Zombie5")]
    Zombie5 = 24,
    [InspectorName("Common/Zombie6")]
    Zombie6 = 25,
    [InspectorName("Common/Zombie7")]
    Zombie7 = 26,
    [InspectorName("Common/Zombie8")]
    Zombie8 = 27,
    [InspectorName("Common/Zombie9")]
    Zombie9 = 28,
    [InspectorName("Common/Zombie10")]
    Zombie10 = 29,
    [InspectorName("Boss/Boss5_ZombieStrong")]
    Boss5_ZombieStrong = 30,
    [InspectorName("Common/CommonZombie")]
    CommonZombie = 31,
    [InspectorName("Boss/Boss6_SpeedZombie")]
    Boss6_SpeedZombie = 32,
    [InspectorName("Common/Rome_ParadinType1")]
    Rome_ParadinType1 = 33,
    [InspectorName("Common/Rome_ParadinType2")]
    Rome_ParadinType2 = 34,
    [InspectorName("Common/Rome_ParadinType3")]
    Rome_ParadinType3 = 35,
    [InspectorName("Elite/Empire_ParadinHero")]
    Empire_ParadinHero = 36,
    [InspectorName("Common/Rome_Archor")]
    Rome_Archor = 37,
    [InspectorName("Boss/Empire_Hero")]
    Empire_Hero = 38,
    [InspectorName("Common/Rome_CitizenFemale")]
    Rome_CitizenFemale = 39,
    [InspectorName("Common/Rome_CitizenMale")]
    Rome_CitizenMale = 40,
    [InspectorName("Common/Enemy6_ForestMan2")]
    Enemy6_ForestMan2 = 41,
    [InspectorName("Common/ProtectAI_1")]
    ProtectAI_1 = 42,
    [InspectorName("Common/Enemy6_ForestMan1")]
    Enemy6_ForestMan1 = 43,
    [InspectorName("Common/Enemy6_ForestMan3_Archor")]
    Enemy6_ForestMan3_Archor = 44,
    [InspectorName("Common/PlayableAI6_Magician")]
    PlayableAI6_Magician = 45,
    [InspectorName("Common/PlayableAI_Expert")]
    PlayableAI_Expert = 46,
    [InspectorName("Common/PlayableAI_GrandMaster")]
    PlayableAI_GrandMaster = 47,
    [InspectorName("Boss/Boss7_Archor_Diana")]
    Boss7_Archor_Diana = 48,
    [InspectorName("Boss/TestBoss")]
    TestBoss = 49,
    [InspectorName("Common/Enemy6_RockGolem")]
    Enemy6_RockGolem = 50,
    [InspectorName("Common/ProtectAI_2")]
    ProtectAI_2 = 51,
    [InspectorName("Elite/Boss2_RockGolem")]
    Boss2_RockGolem = 52,
    [InspectorName("Boss/Boss3_Anubis")]
    Boss3_Anubis = 53,
    [InspectorName("Boss/Boss1_Belcross")]
    Boss1_Belcross = 54,
    [InspectorName("Common/PlayableAI_Master_Title_2_2")]
    PlayableAI_Master_Title_2_2 = 55,
    [InspectorName("Common/PracticeMode1_ScareCrow1")]
    PracticeMode1_ScareCrow1 = 56,
    [InspectorName("Common/PracticeMode1_EnemyAI1")]
    PracticeMode1_EnemyAI1 = 57,
    [InspectorName("Common/PracticeMode1_Boss1")]
    PracticeMode1_Boss1 = 58,
    [InspectorName("Boss/Boss5_ZombieStrong2")]
    Boss5_ZombieStrong2 = 59,
    [InspectorName("Common/CommonZombieType3")]
    CommonZombieType3 = 60,
    [InspectorName("Common/CommonZombieType4")]
    CommonZombieType4 = 61,

}