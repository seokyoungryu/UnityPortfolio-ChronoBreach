using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SoundList
{
	None = -1,
	
       [InspectorName("Effect/Attack_Swing/SwordSmall1")]
       SwordSmall1 = 0,
       [InspectorName("Effect/Attack_Swing/SwordSmall2")]
       SwordSmall2 = 1,
       [InspectorName("Effect/Attack_Swing/SwordSmall3")]
       SwordSmall3 = 2,
       [InspectorName("Effect/Attack_Swing/SwordSmall4")]
       SwordSmall4 = 3,
       [InspectorName("Effect/Attack_Swing/SwordSmall5")]
       SwordSmall5 = 4,
       [InspectorName("Effect/Attack_Swing/SwordSmall6")]
       SwordSmall6 = 5,
       [InspectorName("Effect/Attack_Swing/SwordSmall7")]
       SwordSmall7 = 6,
       [InspectorName("Effect/Attack_Swing/SwordSmall8")]
       SwordSmall8 = 7,
       [InspectorName("Effect/Attack_Swing/SwordSmall9")]
       SwordSmall9 = 8,
       [InspectorName("Effect/Attack_Swing/SwordSmall10")]
       SwordSmall10 = 9,
       [InspectorName("Effect/Attack_Swing/SwordSmall11")]
       SwordSmall11 = 10,
       [InspectorName("Effect/Attack_Swing/SwordSmall12")]
       SwordSmall12 = 11,
       [InspectorName("Effect/Attack_Swing/SwordSmall13")]
       SwordSmall13 = 12,
       [InspectorName("Effect/Attack_Swing/SwordSmall14")]
       SwordSmall14 = 13,
       [InspectorName("Effect/Attack_Swing/SwordSmall15")]
       SwordSmall15 = 14,
       [InspectorName("Effect/Attack_Swing/SwordSmall16")]
       SwordSmall16 = 15,
       [InspectorName("Effect/Attack_Swing/SwordSmall17")]
       SwordSmall17 = 16,
       [InspectorName("Effect/Attack_Swing/SwordSmall18")]
       SwordSmall18 = 17,
       [InspectorName("Effect/Attack_Swing/SwordSmall19")]
       SwordSmall19 = 18,
       [InspectorName("Effect/Attack_Swing/SwordSmall20")]
       SwordSmall20 = 19,
       [InspectorName("Effect/Attack_Swing/JianSwoosh01")]
       JianSwoosh01 = 20,
       [InspectorName("Effect/Attack_Swing/JianSwoosh02")]
       JianSwoosh02 = 21,
       [InspectorName("Effect/Attack_Swing/JianSwoosh03")]
       JianSwoosh03 = 22,
       [InspectorName("Effect/Attack_Swing/JianSwoosh04")]
       JianSwoosh04 = 23,
       [InspectorName("Effect/Attack_Swing/KatanaSwoosh01")]
       KatanaSwoosh01 = 24,
       [InspectorName("Effect/Attack_Swing/KatanaSwoosh02")]
       KatanaSwoosh02 = 25,
       [InspectorName("Effect/Attack_Swing/KatanaSwoosh03")]
       KatanaSwoosh03 = 26,
       [InspectorName("Effect/Attack_Swing/KatanaSwoosh04")]
       KatanaSwoosh04 = 27,
       [InspectorName("Effect/Attack_Swing/TantoSwoosh01")]
       TantoSwoosh01 = 28,
       [InspectorName("Effect/Attack_Swing/TantoSwoosh02")]
       TantoSwoosh02 = 29,
       [InspectorName("Effect/Attack_Swing/TantoSwoosh03")]
       TantoSwoosh03 = 30,
       [InspectorName("Effect/Attack_Swing/TantoSwoosh04")]
       TantoSwoosh04 = 31,
       [InspectorName("Effect/Attack_Swing/SwordNormal01")]
       SwordNormal01 = 32,
       [InspectorName("Effect/Attack_Swing/SwordNormal02")]
       SwordNormal02 = 33,
       [InspectorName("Effect/Attack_Swing/SwordNormal03")]
       SwordNormal03 = 34,
       [InspectorName("Effect/Attack_Swing/SwordNormal04")]
       SwordNormal04 = 35,
       [InspectorName("Effect/Attack_Swing/SwordNormal05")]
       SwordNormal05 = 36,
       [InspectorName("Effect/Attack_Swing/SwordNormal06")]
       SwordNormal06 = 37,
       [InspectorName("Effect/Attack_Swing/SwordNormal07")]
       SwordNormal07 = 38,
       [InspectorName("Effect/Attack_Swing/SwordNormal08")]
       SwordNormal08 = 39,
       [InspectorName("Effect/Attack_Swing/SwordNormal09")]
       SwordNormal09 = 40,
       [InspectorName("Effect/Attack_Swing/SwordNormal010")]
       SwordNormal010 = 41,
       [InspectorName("Effect/Attack_Swing/SwordNormal011")]
       SwordNormal011 = 42,
       [InspectorName("Effect/Attack_Swing/SwordNormal012")]
       SwordNormal012 = 43,
       [InspectorName("Effect/Attack_Swing/Sword2Normal01")]
       Sword2Normal01 = 44,
       [InspectorName("Effect/Attack_Swing/Sword2Normal02")]
       Sword2Normal02 = 45,
       [InspectorName("Effect/Attack_Swing/Sword2Normal03")]
       Sword2Normal03 = 46,
       [InspectorName("Effect/Attack_Swing/Sword2Normal04")]
       Sword2Normal04 = 47,
       [InspectorName("Effect/Attack_Swing/Sword2Normal05")]
       Sword2Normal05 = 48,
       [InspectorName("Effect/Attack_Swing/Sword2Normal06")]
       Sword2Normal06 = 49,
       [InspectorName("Effect/Attack_Swing/Sword2Normal07")]
       Sword2Normal07 = 50,
       [InspectorName("Effect/Attack_Swing/Sword3Normal01")]
       Sword3Normal01 = 51,
       [InspectorName("Effect/Attack_Swing/Sword3Normal02")]
       Sword3Normal02 = 52,
       [InspectorName("Effect/Attack_Swing/Sword3Normal03")]
       Sword3Normal03 = 53,
       [InspectorName("Effect/Attack_Swing/Sword3Normal04")]
       Sword3Normal04 = 54,
       [InspectorName("Effect/Attack_Swing/Dagger1")]
       Dagger1 = 55,
       [InspectorName("Effect/Attack_Swing/Dagger2")]
       Dagger2 = 56,
       [InspectorName("Effect/Attack_Swing/Dagger3")]
       Dagger3 = 57,
       [InspectorName("Effect/Attack_Swing/KnifeSwing1")]
       KnifeSwing1 = 58,
       [InspectorName("Effect/Attack_Swing/KnifeSwing2")]
       KnifeSwing2 = 59,
       [InspectorName("Effect/Attack_Swing/KnifeSwing3")]
       KnifeSwing3 = 60,
       [InspectorName("Effect/Attack_Swing/LargeSwordSwing1")]
       LargeSwordSwing1 = 61,
       [InspectorName("Effect/Attack_Swing/LargeSwordSwing2")]
       LargeSwordSwing2 = 62,
       [InspectorName("Effect/Attack_Swing/LargeSwordSwing3")]
       LargeSwordSwing3 = 63,
       [InspectorName("Effect/Attack_Swing/LargeSwordSwing4")]
       LargeSwordSwing4 = 64,
       [InspectorName("Effect/Attack_Swing/LargeSwordSwing5")]
       LargeSwordSwing5 = 65,
       [InspectorName("Effect/Attack_Swing/LargeSwordSwing6")]
       LargeSwordSwing6 = 66,
       [InspectorName("Effect/Attack_Swing/Punch1")]
       Punch1 = 67,
       [InspectorName("Effect/Attack_Swing/Punch2")]
       Punch2 = 68,
       [InspectorName("Effect/Attack_Swing/Punch3")]
       Punch3 = 69,
       [InspectorName("Effect/Attack_Swing/Punch4")]
       Punch4 = 70,
       [InspectorName("Effect/Attack_Swing/Punch5")]
       Punch5 = 71,
       [InspectorName("Effect/Attack_Swing/Stick1")]
       Stick1 = 72,
       [InspectorName("Effect/Attack_Swing/Stick2")]
       Stick2 = 73,
       [InspectorName("Effect/Attack_Swing/Sword1")]
       Sword1 = 74,
       [InspectorName("Effect/Attack_Swing/Sword2")]
       Sword2 = 75,
       [InspectorName("Effect/Attack_Swing/Sword3")]
       Sword3 = 76,
       [InspectorName("Effect/Attack_Swing/SwordSwing1")]
       SwordSwing1 = 77,
       [InspectorName("Effect/Attack_Swing/SwordSwing2")]
       SwordSwing2 = 78,
       [InspectorName("Effect/Attack_Swing/SwordSwing3")]
       SwordSwing3 = 79,
       [InspectorName("Effect/Attack_Swing/SwordSwing4")]
       SwordSwing4 = 80,
       [InspectorName("Effect/Attack_Swing/SwordSwing5")]
       SwordSwing5 = 81,
       [InspectorName("Effect/Attack_Swing/SwordSwing6")]
       SwordSwing6 = 82,
       [InspectorName("Effect/Attack_Swing/WEAPSwrd_Sword_Whoosh_01")]
       WEAPSwrd_Sword_Whoosh_01 = 83,
       [InspectorName("Effect/Attack_Swing/WEAPSwrd_Sword_Whoosh_02")]
       WEAPSwrd_Sword_Whoosh_02 = 84,
       [InspectorName("Effect/Attack_Swing/WEAPSwrd_Sword_Whoosh_03")]
       WEAPSwrd_Sword_Whoosh_03 = 85,
       [InspectorName("Effect/Attack_Swing/WEAPSwrd_Sword_Whoosh_04")]
       WEAPSwrd_Sword_Whoosh_04 = 86,
       [InspectorName("Effect/Attack_Swing/WEAPSwrd_Sword_Whoosh_05")]
       WEAPSwrd_Sword_Whoosh_05 = 87,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_EarthAttack_01")]
       Elemental_Sword_EarthAttack_01 = 88,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_EarthAttack_02")]
       Elemental_Sword_EarthAttack_02 = 89,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_EarthAttack_03")]
       Elemental_Sword_EarthAttack_03 = 90,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_FireAttack_01")]
       Elemental_Sword_FireAttack_01 = 91,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_FireAttack_02")]
       Elemental_Sword_FireAttack_02 = 92,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_FireAttack_03")]
       Elemental_Sword_FireAttack_03 = 93,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_IceAttackV1")]
       Elemental_Sword_IceAttackV1 = 94,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_IceAttackV2")]
       Elemental_Sword_IceAttackV2 = 95,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_IceAttackV3")]
       Elemental_Sword_IceAttackV3 = 96,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_PoisonAttack_01")]
       Elemental_Sword_PoisonAttack_01 = 97,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_PoisonAttack_02")]
       Elemental_Sword_PoisonAttack_02 = 98,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_PoisonAttack_03")]
       Elemental_Sword_PoisonAttack_03 = 99,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_ThunderAttack_01")]
       Elemental_Sword_ThunderAttack_01 = 100,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_ThunderAttack_02")]
       Elemental_Sword_ThunderAttack_02 = 101,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_ThunderAttack_03")]
       Elemental_Sword_ThunderAttack_03 = 102,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_WaterAttack_01")]
       Elemental_Sword_WaterAttack_01 = 103,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_WaterAttack_02")]
       Elemental_Sword_WaterAttack_02 = 104,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_WaterAttack_03")]
       Elemental_Sword_WaterAttack_03 = 105,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_WindAttack_01")]
       Elemental_Sword_WindAttack_01 = 106,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_WindAttack_02")]
       Elemental_Sword_WindAttack_02 = 107,
       [InspectorName("Effect/Attack_MagicSlash/Elemental_Sword_WindAttack_03")]
       Elemental_Sword_WindAttack_03 = 108,
       [InspectorName("Effect/Attack_MagicSlash/Magic_Sword_Attack_01")]
       Magic_Sword_Attack_01 = 109,
       [InspectorName("Effect/Attack_MagicSlash/Magic_Sword_Attack_02")]
       Magic_Sword_Attack_02 = 110,
       [InspectorName("Effect/Attack_MagicSlash/Magic_Sword_Attack_03")]
       Magic_Sword_Attack_03 = 111,
       [InspectorName("Effect/Attack_Bow/BowAim01")]
       BowAim01 = 112,
       [InspectorName("Effect/Attack_Bow/BowAim1")]
       BowAim1 = 113,
       [InspectorName("Effect/Attack_Bow/BowAim02")]
       BowAim02 = 114,
       [InspectorName("Effect/Attack_Bow/BowAim2")]
       BowAim2 = 115,
       [InspectorName("Effect/Attack_Bow/BowAim03")]
       BowAim03 = 116,
       [InspectorName("Effect/Attack_Bow/BowAim04")]
       BowAim04 = 117,
       [InspectorName("Effect/Attack_Bow/BowHandling01")]
       BowHandling01 = 118,
       [InspectorName("Effect/Attack_Bow/BowHandling02")]
       BowHandling02 = 119,
       [InspectorName("Effect/Attack_Bow/BowHandling03")]
       BowHandling03 = 120,
       [InspectorName("Effect/Attack_Bow/BowShotWhistle01")]
       BowShotWhistle01 = 121,
       [InspectorName("Effect/Attack_Bow/BowShotWhistle02")]
       BowShotWhistle02 = 122,
       [InspectorName("Effect/Attack_Bow/BowWhistleShot1")]
       BowWhistleShot1 = 123,
       [InspectorName("Effect/Attack_Bow/BowWhistleShot2")]
       BowWhistleShot2 = 124,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_EarthAttack_01")]
       Elemental_Bow_EarthAttack_01 = 125,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_EarthAttack_02")]
       Elemental_Bow_EarthAttack_02 = 126,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_EarthAttack_03")]
       Elemental_Bow_EarthAttack_03 = 127,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_FireAttack_01")]
       Elemental_Bow_FireAttack_01 = 128,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_FireAttack_02")]
       Elemental_Bow_FireAttack_02 = 129,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_FireAttack_03")]
       Elemental_Bow_FireAttack_03 = 130,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_IceAttack_01")]
       Elemental_Bow_IceAttack_01 = 131,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_IceAttack_02")]
       Elemental_Bow_IceAttack_02 = 132,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_IceAttack_03")]
       Elemental_Bow_IceAttack_03 = 133,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_PoisonAttack_01")]
       Elemental_Bow_PoisonAttack_01 = 134,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_PoisonAttack_02")]
       Elemental_Bow_PoisonAttack_02 = 135,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_PoisonAttack_03")]
       Elemental_Bow_PoisonAttack_03 = 136,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_ThunderAttack_01")]
       Elemental_Bow_ThunderAttack_01 = 137,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_ThunderAttack_02")]
       Elemental_Bow_ThunderAttack_02 = 138,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_ThunderAttack_03")]
       Elemental_Bow_ThunderAttack_03 = 139,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_WaterAttack_01")]
       Elemental_Bow_WaterAttack_01 = 140,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_WaterAttack_02")]
       Elemental_Bow_WaterAttack_02 = 141,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_WaterAttack_03")]
       Elemental_Bow_WaterAttack_03 = 142,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_WindAttack_01")]
       Elemental_Bow_WindAttack_01 = 143,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_WindAttack_02")]
       Elemental_Bow_WindAttack_02 = 144,
       [InspectorName("Effect/Attack_Bow/Elemental_Bow_WindAttack_03")]
       Elemental_Bow_WindAttack_03 = 145,
       [InspectorName("Effect/Attack_Bow/Fire_Bow_Whoosh_01")]
       Fire_Bow_Whoosh_01 = 146,
       [InspectorName("Effect/Attack_Bow/Fire_Bow_Whoosh_02")]
       Fire_Bow_Whoosh_02 = 147,
       [InspectorName("Effect/Attack_Bow/Ice_Arrow_Whoosh_01")]
       Ice_Arrow_Whoosh_01 = 148,
       [InspectorName("Effect/Attack_Bow/Ice_Arrow_Whoosh_02")]
       Ice_Arrow_Whoosh_02 = 149,
       [InspectorName("Effect/Attack_Bow/Poison_Arrow_Whoosh_01")]
       Poison_Arrow_Whoosh_01 = 150,
       [InspectorName("Effect/Attack_Bow/Poison_Arrow_Whoosh_02")]
       Poison_Arrow_Whoosh_02 = 151,
       [InspectorName("Effect/Attack_Bow/Poison_Arrow_Whoosh_03")]
       Poison_Arrow_Whoosh_03 = 152,
       [InspectorName("Effect/Attack_Bow/Poison_Sword_Whoosh_01")]
       Poison_Sword_Whoosh_01 = 153,
       [InspectorName("Effect/Attack_Bow/Poison_Sword_Whoosh_02")]
       Poison_Sword_Whoosh_02 = 154,
       [InspectorName("Effect/Attack_Bow/Poison_Sword_Whoosh_03")]
       Poison_Sword_Whoosh_03 = 155,
       [InspectorName("Effect/Attack_Bow/Shooting_Arrow_Ice_Reverb_01")]
       Shooting_Arrow_Ice_Reverb_01 = 156,
       [InspectorName("Effect/Attack_Bow/Shooting_Arrow_Ice_Reverb_02")]
       Shooting_Arrow_Ice_Reverb_02 = 157,
       [InspectorName("Effect/Attack_Bow/Shooting_Arrow_Ice_Reverb_03")]
       Shooting_Arrow_Ice_Reverb_03 = 158,
       [InspectorName("Effect/Attack_Bow/Shooting_fire_Arrow_Reverb_01")]
       Shooting_fire_Arrow_Reverb_01 = 159,
       [InspectorName("Effect/Attack_Bow/Shooting_fire_Arrow_Reverb_02")]
       Shooting_fire_Arrow_Reverb_02 = 160,
       [InspectorName("Effect/Attack_Bow/Shooting_Poison_Arrow_NoReverb_01")]
       Shooting_Poison_Arrow_NoReverb_01 = 161,
       [InspectorName("Effect/Attack_Bow/Shooting_Thunder_Arrow_NoReverb_01")]
       Shooting_Thunder_Arrow_NoReverb_01 = 162,
       [InspectorName("Effect/Attack_Bow/Shooting_Water_Arrow_NoReverb_01")]
       Shooting_Water_Arrow_NoReverb_01 = 163,
       [InspectorName("Effect/Attack_Bow/Shooting_Water_Arrow_NoReverb_02")]
       Shooting_Water_Arrow_NoReverb_02 = 164,
       [InspectorName("Effect/Attack_Bow/Shooting_Water_Arrow_NoReverb_03")]
       Shooting_Water_Arrow_NoReverb_03 = 165,
       [InspectorName("Effect/Attack_Bow/Shooting_Wind_Arrow_NoReverb_01")]
       Shooting_Wind_Arrow_NoReverb_01 = 166,
       [InspectorName("Effect/Attack_Bow/Shooting_Wind_Arrow_NoReverb_02")]
       Shooting_Wind_Arrow_NoReverb_02 = 167,
       [InspectorName("Effect/Attack_Bow/Shooting_Wind_Arrow_NoReverb_03")]
       Shooting_Wind_Arrow_NoReverb_03 = 168,
       [InspectorName("Effect/Attack_Bow/Water_Sword_Whoosh_01")]
       Water_Sword_Whoosh_01 = 169,
       [InspectorName("Effect/Attack_Bow/Water_Sword_Whoosh_02")]
       Water_Sword_Whoosh_02 = 170,
       [InspectorName("Effect/Attack_Bow/Water_Sword_Whoosh_03")]
       Water_Sword_Whoosh_03 = 171,
       [InspectorName("Effect/Attack_Bow/Wind_Sword_Whoosh_01")]
       Wind_Sword_Whoosh_01 = 172,
       [InspectorName("Effect/Attack_Bow/Wind_Sword_Whoosh_02")]
       Wind_Sword_Whoosh_02 = 173,
       [InspectorName("Effect/Attack_Bow/Wind_Sword_Whoosh_03")]
       Wind_Sword_Whoosh_03 = 174,
       [InspectorName("Effect/Monster_AttackVoice/Danger_02")]
       Danger_02 = 175,
       [InspectorName("Effect/Monster_AttackVoice/Danger_03")]
       Danger_03 = 176,
       [InspectorName("Effect/Monster_AttackVoice/Danger_With_Caw_02")]
       Danger_With_Caw_02 = 177,
       [InspectorName("Effect/Monster_AttackVoice/Growl_01")]
       Growl_01 = 178,
       [InspectorName("Effect/Monster_AttackVoice/Growl_02")]
       Growl_02 = 179,
       [InspectorName("Effect/Monster_AttackVoice/Growl_03")]
       Growl_03 = 180,
       [InspectorName("Effect/Monster_AttackVoice/Growl_04")]
       Growl_04 = 181,
       [InspectorName("Effect/Monster_AttackVoice/Growl_05")]
       Growl_05 = 182,
       [InspectorName("Effect/Monster_AttackVoice/Wolf_voice_creature_01_wav")]
       Wolf_voice_creature_01_wav = 183,
       [InspectorName("Effect/Monster_AttackVoice/Wolf_voice_creature_02_wav")]
       Wolf_voice_creature_02_wav = 184,
       [InspectorName("Effect/Monster_AttackVoice/Wolf_voice_creature_03_wav")]
       Wolf_voice_creature_03_wav = 185,
       [InspectorName("Effect/Monster_AttackVoice/Wolf_voice_creature_04_wav")]
       Wolf_voice_creature_04_wav = 186,
       [InspectorName("Effect/Monster_AttackVoice/Wolf_voice_creature_05_wav")]
       Wolf_voice_creature_05_wav = 187,
       [InspectorName("Effect/Monster_AttackVoice/Wolf_voice_creature_06_wav")]
       Wolf_voice_creature_06_wav = 188,
       [InspectorName("Effect/Monster_AttackVoice/Zombie_attack_one_shot_06")]
       Zombie_attack_one_shot_06 = 189,
       [InspectorName("Effect/Monster_AttackVoice/Zombie_attack_one_shot_08")]
       Zombie_attack_one_shot_08 = 190,
       [InspectorName("Effect/Monster_AttackVoice/Zombie_attack_one_shot_16")]
       Zombie_attack_one_shot_16 = 191,
       [InspectorName("Effect/Monster_AttackVoice/Zombie_attack_one_shot_20")]
       Zombie_attack_one_shot_20 = 192,
       [InspectorName("Effect/Monster_AttackVoice/Zombie_attack_one_shot_22")]
       Zombie_attack_one_shot_22 = 193,
       [InspectorName("Effect/Monster_AttackVoice/Zombie_attack_one_shot_24")]
       Zombie_attack_one_shot_24 = 194,
       [InspectorName("Effect/Monster_Roar/AgressiveShout_01_WithEcho")]
       AgressiveShout_01_WithEcho = 195,
       [InspectorName("Effect/Monster_Roar/AgressiveShout_02_WithEcho")]
       AgressiveShout_02_WithEcho = 196,
       [InspectorName("Effect/Monster_Roar/AgressiveShout_05_WithEcho")]
       AgressiveShout_05_WithEcho = 197,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v1_wav")]
       Monster_Loud_v1_wav = 198,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v2_wav")]
       Monster_Loud_v2_wav = 199,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v3_wav")]
       Monster_Loud_v3_wav = 200,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v4_wav")]
       Monster_Loud_v4_wav = 201,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v5_wav")]
       Monster_Loud_v5_wav = 202,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v6_wav")]
       Monster_Loud_v6_wav = 203,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v7_wav")]
       Monster_Loud_v7_wav = 204,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v8_wav")]
       Monster_Loud_v8_wav = 205,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v9_wav")]
       Monster_Loud_v9_wav = 206,
       [InspectorName("Effect/Monster_Roar/Monster_Loud_v10_wav")]
       Monster_Loud_v10_wav = 207,
       [InspectorName("Effect/Monster_Roar/MonsterPack2_Monster01_Death02")]
       MonsterPack2_Monster01_Death02 = 208,
       [InspectorName("Effect/Monster_Roar/MonsterPack2_Monster01_Death03")]
       MonsterPack2_Monster01_Death03 = 209,
       [InspectorName("Effect/Monster_Roar/MonsterPack2_Monster01_Death11")]
       MonsterPack2_Monster01_Death11 = 210,
       [InspectorName("Effect/Monster_Roar/Slow_Breathing")]
       Slow_Breathing = 211,
       [InspectorName("Effect/Monster_Roar/Zombie_as_attack1_one_shot_01")]
       Zombie_as_attack1_one_shot_01 = 212,
       [InspectorName("Effect/Monster_Roar/Zombie_as_attack2_one_shot_07")]
       Zombie_as_attack2_one_shot_07 = 213,
       [InspectorName("Effect/Monster_Roar/Zombie_as_attack2_one_shot_08")]
       Zombie_as_attack2_one_shot_08 = 214,
       [InspectorName("Effect/Monster_Roar/Zombie_as_scream2_one_shot_09")]
       Zombie_as_scream2_one_shot_09 = 215,
       [InspectorName("Effect/Monster_Roar/Zombie_as_scream3_one_shot_03")]
       Zombie_as_scream3_one_shot_03 = 216,
       [InspectorName("Effect/Monster_Die/CREAHmn_Skeleton_Death_01")]
       CREAHmn_Skeleton_Death_01 = 217,
       [InspectorName("Effect/Monster_Die/CREAHmn_Skeleton_Death_02")]
       CREAHmn_Skeleton_Death_02 = 218,
       [InspectorName("Effect/Monster_Die/CREAHmn_Skeleton_Death_03")]
       CREAHmn_Skeleton_Death_03 = 219,
       [InspectorName("Effect/Monster_Die/CREAHmn_Skeleton_Death_04")]
       CREAHmn_Skeleton_Death_04 = 220,
       [InspectorName("Effect/Monster_Die/CREAHmn_Skeleton_Death_05")]
       CREAHmn_Skeleton_Death_05 = 221,
       [InspectorName("Effect/Monster_Die/CREAHmn_Skeleton_Death_06")]
       CREAHmn_Skeleton_Death_06 = 222,
       [InspectorName("Effect/Monster_Die/CREAHmn_Skeleton_Death_07")]
       CREAHmn_Skeleton_Death_07 = 223,
       [InspectorName("Effect/Monster_Die/Monster_Dying_v1_wav")]
       Monster_Dying_v1_wav = 224,
       [InspectorName("Effect/Monster_Die/Monster_Dying_v2_wav")]
       Monster_Dying_v2_wav = 225,
       [InspectorName("Effect/Monster_Die/Monster_Dying_v3_wav")]
       Monster_Dying_v3_wav = 226,
       [InspectorName("Effect/Monster_Die/Monster_Dying_v4_Wav")]
       Monster_Dying_v4_Wav = 227,
       [InspectorName("Effect/Monster_Die/Monster_Dying_v5_wav")]
       Monster_Dying_v5_wav = 228,
       [InspectorName("Effect/Monster_Die/Monster_Dying_v6_wav")]
       Monster_Dying_v6_wav = 229,
       [InspectorName("Effect/Monster_Die/Monster_Dying_v7_wav")]
       Monster_Dying_v7_wav = 230,
       [InspectorName("Effect/Monster_Die/Monster_Say_Die_v5_wav")]
       Monster_Say_Die_v5_wav = 231,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v1_variation_01_wav")]
       Zombie_Die_v1_variation_01_wav = 232,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v1_variation_02_wav")]
       Zombie_Die_v1_variation_02_wav = 233,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v1_wav")]
       Zombie_Die_v1_wav = 234,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v2_variation_01_wav")]
       Zombie_Die_v2_variation_01_wav = 235,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v2_variation_02_wav")]
       Zombie_Die_v2_variation_02_wav = 236,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v2_wav")]
       Zombie_Die_v2_wav = 237,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v3_variation_01_wav")]
       Zombie_Die_v3_variation_01_wav = 238,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v3_variation_02_wav")]
       Zombie_Die_v3_variation_02_wav = 239,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v3_wav")]
       Zombie_Die_v3_wav = 240,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v4_variation_01_wav")]
       Zombie_Die_v4_variation_01_wav = 241,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v4_variation_02_wav")]
       Zombie_Die_v4_variation_02_wav = 242,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v4_wav")]
       Zombie_Die_v4_wav = 243,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v5_variation_01_wav")]
       Zombie_Die_v5_variation_01_wav = 244,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v5_variation_02_wav")]
       Zombie_Die_v5_variation_02_wav = 245,
       [InspectorName("Effect/Monster_Die/Zombie_Die_v5_wav")]
       Zombie_Die_v5_wav = 246,
       [InspectorName("Effect/Monster_Hurt/CREAHmn_Skeleton_Hurt_01")]
       CREAHmn_Skeleton_Hurt_01 = 247,
       [InspectorName("Effect/Monster_Hurt/CREAHmn_Skeleton_Hurt_02")]
       CREAHmn_Skeleton_Hurt_02 = 248,
       [InspectorName("Effect/Monster_Hurt/CREAHmn_Skeleton_Hurt_03")]
       CREAHmn_Skeleton_Hurt_03 = 249,
       [InspectorName("Effect/Monster_Hurt/CREAHmn_Skeleton_Hurt_04")]
       CREAHmn_Skeleton_Hurt_04 = 250,
       [InspectorName("Effect/Monster_Hurt/CREAHmn_Skeleton_Hurt_05")]
       CREAHmn_Skeleton_Hurt_05 = 251,
       [InspectorName("Effect/Monster_Hurt/CREAHmn_Skeleton_Hurt_06")]
       CREAHmn_Skeleton_Hurt_06 = 252,
       [InspectorName("Effect/Monster_Hurt/CREAHmn_Skeleton_Hurt_07")]
       CREAHmn_Skeleton_Hurt_07 = 253,
       [InspectorName("Effect/Monster_Hurt/Zombie_damage_one_shot_04")]
       Zombie_damage_one_shot_04 = 254,
       [InspectorName("Effect/Monster_Hurt/Zombie_damage_one_shot_06")]
       Zombie_damage_one_shot_06 = 255,
       [InspectorName("Effect/Monster_Hurt/Zombie_damage_one_shot_15")]
       Zombie_damage_one_shot_15 = 256,
       [InspectorName("Effect/Monster_Hurt/Zombie_damage_one_shot_31")]
       Zombie_damage_one_shot_31 = 257,
       [InspectorName("Effect/Monster_Hurt/Zombie_Get_Hurt_v5_variation_01_wav")]
       Zombie_Get_Hurt_v5_variation_01_wav = 258,
       [InspectorName("Effect/Monster_Hurt/Zombie_Get_Hurt_v5_variation_02_wav")]
       Zombie_Get_Hurt_v5_variation_02_wav = 259,
       [InspectorName("Effect/Monster_Hurt/Zombie_Get_Hurt_v5_wav")]
       Zombie_Get_Hurt_v5_wav = 260,
       [InspectorName("Effect/Monster_FindNear/Scream_01")]
       Scream_01 = 261,
       [InspectorName("Effect/Monster_FindNear/Scream_02")]
       Scream_02 = 262,
       [InspectorName("Effect/Monster_FindNear/Scream_03")]
       Scream_03 = 263,
       [InspectorName("Effect/Monster_FindNear/Scream_04")]
       Scream_04 = 264,
       [InspectorName("Effect/Monster_FindNear/Scream_With_Caw_01")]
       Scream_With_Caw_01 = 265,
       [InspectorName("Effect/Monster_FindNear/Scream_With_Caw_02")]
       Scream_With_Caw_02 = 266,
       [InspectorName("Effect/Monster_FindNear/Scream_With_Caw_03")]
       Scream_With_Caw_03 = 267,
       [InspectorName("Effect/Monster_FindNear/Scream_With_Caw_04")]
       Scream_With_Caw_04 = 268,
       [InspectorName("Effect/Buff/Cast_Heal_01")]
       Cast_Heal_01 = 269,
       [InspectorName("Effect/Buff/Cast_Heal_02")]
       Cast_Heal_02 = 270,
       [InspectorName("Effect/Buff/Cast_Heal_03")]
       Cast_Heal_03 = 271,
       [InspectorName("Effect/Buff/Cast_Heal_04")]
       Cast_Heal_04 = 272,
       [InspectorName("Effect/Buff/Cast_Heal_05")]
       Cast_Heal_05 = 273,
       [InspectorName("Effect/Buff/Cast_Heal_06")]
       Cast_Heal_06 = 274,
       [InspectorName("Effect/Buff/Cast_Heal_07")]
       Cast_Heal_07 = 275,
       [InspectorName("Effect/Buff/Cast_Heal_08")]
       Cast_Heal_08 = 276,
       [InspectorName("Effect/Buff/Cast_Heal_09")]
       Cast_Heal_09 = 277,
       [InspectorName("Effect/Buff/Cast_Heal_10")]
       Cast_Heal_10 = 278,
       [InspectorName("Effect/Buff/Cast_Heal_11")]
       Cast_Heal_11 = 279,
       [InspectorName("Effect/Buff/Cast_Heal_12")]
       Cast_Heal_12 = 280,
       [InspectorName("Effect/Buff/Cast_Heal_13")]
       Cast_Heal_13 = 281,
       [InspectorName("Effect/Buff/Cast_Heal_14")]
       Cast_Heal_14 = 282,
       [InspectorName("Effect/Buff/Cast_Heal_15")]
       Cast_Heal_15 = 283,
       [InspectorName("Effect/Buff/Cast_Heal_16")]
       Cast_Heal_16 = 284,
       [InspectorName("Effect/Buff/Cast_Heal_17")]
       Cast_Heal_17 = 285,
       [InspectorName("Effect/Buff/Cast_Heal_18")]
       Cast_Heal_18 = 286,
       [InspectorName("Effect/Buff/Cast_Heal_19")]
       Cast_Heal_19 = 287,
       [InspectorName("Effect/Buff/Cast_Heal_20")]
       Cast_Heal_20 = 288,
       [InspectorName("Effect/Buff/Large_Heal_01")]
       Large_Heal_01 = 289,
       [InspectorName("Effect/Buff/Large_Heal_02")]
       Large_Heal_02 = 290,
       [InspectorName("Effect/Buff/Large_Heal_03")]
       Large_Heal_03 = 291,
       [InspectorName("Effect/Buff/Large_Heal_04")]
       Large_Heal_04 = 292,
       [InspectorName("Effect/Buff/Large_Heal_05")]
       Large_Heal_05 = 293,
       [InspectorName("Effect/Buff/Large_Heal_06")]
       Large_Heal_06 = 294,
       [InspectorName("Effect/Buff/Large_Heal_07")]
       Large_Heal_07 = 295,
       [InspectorName("Effect/Buff/Large_Heal_08")]
       Large_Heal_08 = 296,
       [InspectorName("Effect/Buff/Large_Heal_09")]
       Large_Heal_09 = 297,
       [InspectorName("Effect/Buff/Large_Heal_10")]
       Large_Heal_10 = 298,
       [InspectorName("Effect/Buff/Small_Heal_01")]
       Small_Heal_01 = 299,
       [InspectorName("Effect/Buff/Small_Heal_02")]
       Small_Heal_02 = 300,
       [InspectorName("Effect/Buff/Small_Heal_03")]
       Small_Heal_03 = 301,
       [InspectorName("Effect/Buff/Small_Heal_04")]
       Small_Heal_04 = 302,
       [InspectorName("Effect/Buff/Small_Heal_05")]
       Small_Heal_05 = 303,
       [InspectorName("Effect/Buff/Small_Heal_06")]
       Small_Heal_06 = 304,
       [InspectorName("Effect/Buff/Small_Heal_07")]
       Small_Heal_07 = 305,
       [InspectorName("Effect/Buff/Small_Heal_08")]
       Small_Heal_08 = 306,
       [InspectorName("Effect/Buff/Small_Heal_09")]
       Small_Heal_09 = 307,
       [InspectorName("Effect/Buff/Small_Heal_10")]
       Small_Heal_10 = 308,
       [InspectorName("Effect/Counter/BombExplosion02")]
       BombExplosion02 = 309,
       [InspectorName("Effect/Counter/JianClash01")]
       JianClash01 = 310,
       [InspectorName("Effect/Counter/JianClash02")]
       JianClash02 = 311,
       [InspectorName("Effect/Counter/JianClash03")]
       JianClash03 = 312,
       [InspectorName("Effect/Counter/JianClash04")]
       JianClash04 = 313,
       [InspectorName("Effect/Counter/PistolShot01")]
       PistolShot01 = 314,
       [InspectorName("Effect/Counter/PistolShot02")]
       PistolShot02 = 315,
       [InspectorName("Effect/Counter/PistolShotOld01")]
       PistolShotOld01 = 316,
       [InspectorName("Effect/Counter/PistolShotOld02")]
       PistolShotOld02 = 317,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR1_mono")]
       SWORD_Hit_Armor_Hard_RR1_mono = 318,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR2_mono")]
       SWORD_Hit_Armor_Hard_RR2_mono = 319,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR3_mono")]
       SWORD_Hit_Armor_Hard_RR3_mono = 320,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR4_mono")]
       SWORD_Hit_Armor_Hard_RR4_mono = 321,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR5_mono")]
       SWORD_Hit_Armor_Hard_RR5_mono = 322,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR6_mono")]
       SWORD_Hit_Armor_Hard_RR6_mono = 323,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR7_mono")]
       SWORD_Hit_Armor_Hard_RR7_mono = 324,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR8_mono")]
       SWORD_Hit_Armor_Hard_RR8_mono = 325,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR9_mono")]
       SWORD_Hit_Armor_Hard_RR9_mono = 326,
       [InspectorName("Effect/Defense/SWORD_Hit_Armor_Hard_RR10_mono")]
       SWORD_Hit_Armor_Hard_RR10_mono = 327,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR1_mono")]
       SWORD_Hit_Sword_Hard_RR1_mono = 328,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR2_mono")]
       SWORD_Hit_Sword_Hard_RR2_mono = 329,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR3_mono")]
       SWORD_Hit_Sword_Hard_RR3_mono = 330,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR4_mono")]
       SWORD_Hit_Sword_Hard_RR4_mono = 331,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR5_mono")]
       SWORD_Hit_Sword_Hard_RR5_mono = 332,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR6_mono")]
       SWORD_Hit_Sword_Hard_RR6_mono = 333,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR7_mono")]
       SWORD_Hit_Sword_Hard_RR7_mono = 334,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR8_mono")]
       SWORD_Hit_Sword_Hard_RR8_mono = 335,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR9_mono")]
       SWORD_Hit_Sword_Hard_RR9_mono = 336,
       [InspectorName("Effect/Defense/SWORD_Hit_Sword_Hard_RR10_mono")]
       SWORD_Hit_Sword_Hard_RR10_mono = 337,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Armor_Hard_RR2_mono")]
       SWORD_Whoosh_Hit_Armor_Hard_RR2_mono = 338,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Armor_Hard_RR5_mono")]
       SWORD_Whoosh_Hit_Armor_Hard_RR5_mono = 339,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR1_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR1_mono = 340,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR2_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR2_mono = 341,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR3_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR3_mono = 342,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR4_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR4_mono = 343,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR5_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR5_mono = 344,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR6_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR6_mono = 345,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR7_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR7_mono = 346,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR8_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR8_mono = 347,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR9_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR9_mono = 348,
       [InspectorName("Effect/Defense/SWORD_Whoosh_Hit_Sword_Hard_RR10_mono")]
       SWORD_Whoosh_Hit_Sword_Hard_RR10_mono = 349,
       [InspectorName("Effect/Doors/Door_Reverb_v1_wav")]
       Door_Reverb_v1_wav = 350,
       [InspectorName("Effect/Doors/Door_Reverb_v2_wav")]
       Door_Reverb_v2_wav = 351,
       [InspectorName("Effect/Doors/Door_Reverb_v3_wav")]
       Door_Reverb_v3_wav = 352,
       [InspectorName("Effect/Doors/Door_Reverb_v4_wav")]
       Door_Reverb_v4_wav = 353,
       [InspectorName("Effect/Doors/Door_Reverb_v5_wav")]
       Door_Reverb_v5_wav = 354,
       [InspectorName("Effect/Doors/Door_Reverb_v6_wav")]
       Door_Reverb_v6_wav = 355,
       [InspectorName("Effect/Doors/Metal_Slide_Door_v1_wav")]
       Metal_Slide_Door_v1_wav = 356,
       [InspectorName("Effect/Doors/Metal_Slide_Door_v2_wav")]
       Metal_Slide_Door_v2_wav = 357,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_01")]
       FGHTImpt_Punch_Breaking_Bones_01 = 358,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_02")]
       FGHTImpt_Punch_Breaking_Bones_02 = 359,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_03")]
       FGHTImpt_Punch_Breaking_Bones_03 = 360,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_04")]
       FGHTImpt_Punch_Breaking_Bones_04 = 361,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_05")]
       FGHTImpt_Punch_Breaking_Bones_05 = 362,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_06")]
       FGHTImpt_Punch_Breaking_Bones_06 = 363,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_07")]
       FGHTImpt_Punch_Breaking_Bones_07 = 364,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_08")]
       FGHTImpt_Punch_Breaking_Bones_08 = 365,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_09")]
       FGHTImpt_Punch_Breaking_Bones_09 = 366,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_10")]
       FGHTImpt_Punch_Breaking_Bones_10 = 367,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_11")]
       FGHTImpt_Punch_Breaking_Bones_11 = 368,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_12")]
       FGHTImpt_Punch_Breaking_Bones_12 = 369,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_13")]
       FGHTImpt_Punch_Breaking_Bones_13 = 370,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_14")]
       FGHTImpt_Punch_Breaking_Bones_14 = 371,
       [InspectorName("Effect/Hit/FGHTImpt_Punch_Breaking_Bones_15")]
       FGHTImpt_Punch_Breaking_Bones_15 = 372,
       [InspectorName("Effect/Hit/GOREBlood_Blood_01")]
       GOREBlood_Blood_01 = 373,
       [InspectorName("Effect/Hit/GOREBlood_Blood_02")]
       GOREBlood_Blood_02 = 374,
       [InspectorName("Effect/Hit/GOREBlood_Blood_03")]
       GOREBlood_Blood_03 = 375,
       [InspectorName("Effect/Hit/GOREBlood_Blood_04")]
       GOREBlood_Blood_04 = 376,
       [InspectorName("Effect/Hit/GOREBlood_Blood_05")]
       GOREBlood_Blood_05 = 377,
       [InspectorName("Effect/Projectile/Arcane_Buff_01")]
       Arcane_Buff_01 = 378,
       [InspectorName("Effect/Projectile/Arcane_Buff_02")]
       Arcane_Buff_02 = 379,
       [InspectorName("Effect/Projectile/Arcane_Buff_03")]
       Arcane_Buff_03 = 380,
       [InspectorName("Effect/Projectile/Arcane_Buff_04")]
       Arcane_Buff_04 = 381,
       [InspectorName("Effect/Projectile/Arcane_Buff_05")]
       Arcane_Buff_05 = 382,
       [InspectorName("Effect/Projectile/Arcane_Spell_01")]
       Arcane_Spell_01 = 383,
       [InspectorName("Effect/Projectile/Arcane_Spell_02")]
       Arcane_Spell_02 = 384,
       [InspectorName("Effect/Projectile/Arcane_Spell_03")]
       Arcane_Spell_03 = 385,
       [InspectorName("Effect/Projectile/Arcane_Spell_04")]
       Arcane_Spell_04 = 386,
       [InspectorName("Effect/Projectile/Arcane_Spell_05")]
       Arcane_Spell_05 = 387,
       [InspectorName("Effect/Projectile/Arcane_Spell_06")]
       Arcane_Spell_06 = 388,
       [InspectorName("Effect/Projectile/Arcane_Spell_07")]
       Arcane_Spell_07 = 389,
       [InspectorName("Effect/Projectile/Arcane_Spell_08")]
       Arcane_Spell_08 = 390,
       [InspectorName("Effect/Projectile/Arcane_Spell_09")]
       Arcane_Spell_09 = 391,
       [InspectorName("Effect/Projectile/Arcane_Spell_10")]
       Arcane_Spell_10 = 392,
       [InspectorName("Effect/Projectile/Cast_01")]
       Cast_01 = 393,
       [InspectorName("Effect/Projectile/Cast_02")]
       Cast_02 = 394,
       [InspectorName("Effect/Projectile/Cast_03")]
       Cast_03 = 395,
       [InspectorName("Effect/Projectile/Cast_04")]
       Cast_04 = 396,
       [InspectorName("Effect/Projectile/Cast_05")]
       Cast_05 = 397,
       [InspectorName("Effect/Projectile/Cast_06")]
       Cast_06 = 398,
       [InspectorName("Effect/Projectile/Cast_07")]
       Cast_07 = 399,
       [InspectorName("Effect/Projectile/Dark_Aura")]
       Dark_Aura = 400,
       [InspectorName("Effect/Projectile/Dark_Hit_01")]
       Dark_Hit_01 = 401,
       [InspectorName("Effect/Projectile/Dark_Hit_02")]
       Dark_Hit_02 = 402,
       [InspectorName("Effect/Projectile/Dark_Hit_03")]
       Dark_Hit_03 = 403,
       [InspectorName("Effect/Projectile/Dark_Magic_Buff_01")]
       Dark_Magic_Buff_01 = 404,
       [InspectorName("Effect/Projectile/Dark_Magic_Buff_02")]
       Dark_Magic_Buff_02 = 405,
       [InspectorName("Effect/Projectile/Dark_Magic_Buff_03")]
       Dark_Magic_Buff_03 = 406,
       [InspectorName("Effect/Projectile/Dark_Magic_Whoosh_01")]
       Dark_Magic_Whoosh_01 = 407,
       [InspectorName("Effect/Projectile/Dark_Magic_Whoosh_02")]
       Dark_Magic_Whoosh_02 = 408,
       [InspectorName("Effect/Projectile/Dark_Resurrection")]
       Dark_Resurrection = 409,
       [InspectorName("Effect/Projectile/Dark_Spell_01")]
       Dark_Spell_01 = 410,
       [InspectorName("Effect/Projectile/Dark_Spell_02")]
       Dark_Spell_02 = 411,
       [InspectorName("Effect/Projectile/Dark_Spell_03")]
       Dark_Spell_03 = 412,
       [InspectorName("Effect/Projectile/Dark_Spell_04")]
       Dark_Spell_04 = 413,
       [InspectorName("Effect/Projectile/Dragon_Fireball")]
       Dragon_Fireball = 414,
       [InspectorName("Effect/Projectile/Earth_Aura_Loop")]
       Earth_Aura_Loop = 415,
       [InspectorName("Effect/Projectile/Earth_Hit_01")]
       Earth_Hit_01 = 416,
       [InspectorName("Effect/Projectile/Earth_Hit_02")]
       Earth_Hit_02 = 417,
       [InspectorName("Effect/Projectile/Earth_Hit_03")]
       Earth_Hit_03 = 418,
       [InspectorName("Effect/Projectile/Earth_Magic_Whoosh_01")]
       Earth_Magic_Whoosh_01 = 419,
       [InspectorName("Effect/Projectile/Earth_Magic_Whoosh_02")]
       Earth_Magic_Whoosh_02 = 420,
       [InspectorName("Effect/Projectile/Fire_Buff_01")]
       Fire_Buff_01 = 421,
       [InspectorName("Effect/Projectile/Fire_Buff_02")]
       Fire_Buff_02 = 422,
       [InspectorName("Effect/Projectile/Fire_Buff_03")]
       Fire_Buff_03 = 423,
       [InspectorName("Effect/Projectile/Fire_Buff_04")]
       Fire_Buff_04 = 424,
       [InspectorName("Effect/Projectile/Fire_Buff_05")]
       Fire_Buff_05 = 425,
       [InspectorName("Effect/Projectile/Fire_Earthquake_With_Lava")]
       Fire_Earthquake_With_Lava = 426,
       [InspectorName("Effect/Projectile/Fire_Hit_01")]
       Fire_Hit_01 = 427,
       [InspectorName("Effect/Projectile/Fire_Hit_02")]
       Fire_Hit_02 = 428,
       [InspectorName("Effect/Projectile/Fire_Hit_03")]
       Fire_Hit_03 = 429,
       [InspectorName("Effect/Projectile/Fire_magic_buff_01")]
       Fire_magic_buff_01 = 430,
       [InspectorName("Effect/Projectile/Fire_magic_buff_02")]
       Fire_magic_buff_02 = 431,
       [InspectorName("Effect/Projectile/Fire_magic_buff_03")]
       Fire_magic_buff_03 = 432,
       [InspectorName("Effect/Projectile/Fire_Magic_Dragon")]
       Fire_Magic_Dragon = 433,
       [InspectorName("Effect/Projectile/Fire_Spell_01")]
       Fire_Spell_01 = 434,
       [InspectorName("Effect/Projectile/Fire_Spell_02")]
       Fire_Spell_02 = 435,
       [InspectorName("Effect/Projectile/Fire_Spell_03")]
       Fire_Spell_03 = 436,
       [InspectorName("Effect/Projectile/Fire_Spell_04")]
       Fire_Spell_04 = 437,
       [InspectorName("Effect/Projectile/Fire_Spell_05")]
       Fire_Spell_05 = 438,
       [InspectorName("Effect/Projectile/Fire_Spell_06")]
       Fire_Spell_06 = 439,
       [InspectorName("Effect/Projectile/Fire_Spell_07")]
       Fire_Spell_07 = 440,
       [InspectorName("Effect/Projectile/Fire_Spell_08")]
       Fire_Spell_08 = 441,
       [InspectorName("Effect/Projectile/Fire_Spell_09")]
       Fire_Spell_09 = 442,
       [InspectorName("Effect/Projectile/Fire_Spell_10")]
       Fire_Spell_10 = 443,
       [InspectorName("Effect/Projectile/Fire_Spell_011")]
       Fire_Spell_011 = 444,
       [InspectorName("Effect/Projectile/Fire_Spell_022")]
       Fire_Spell_022 = 445,
       [InspectorName("Effect/Projectile/Fire_Spell_033")]
       Fire_Spell_033 = 446,
       [InspectorName("Effect/Projectile/Heal_01")]
       Heal_01 = 447,
       [InspectorName("Effect/Projectile/Heal_02")]
       Heal_02 = 448,
       [InspectorName("Effect/Projectile/Heal_03")]
       Heal_03 = 449,
       [InspectorName("Effect/Projectile/Heal_04")]
       Heal_04 = 450,
       [InspectorName("Effect/Projectile/Heal_05")]
       Heal_05 = 451,
       [InspectorName("Effect/Projectile/Heal_06")]
       Heal_06 = 452,
       [InspectorName("Effect/Projectile/Heal_07")]
       Heal_07 = 453,
       [InspectorName("Effect/Projectile/Heal_08")]
       Heal_08 = 454,
       [InspectorName("Effect/Projectile/Heal_09")]
       Heal_09 = 455,
       [InspectorName("Effect/Projectile/Heal_10")]
       Heal_10 = 456,
       [InspectorName("Effect/Projectile/Ice_Magic_whoosh_01")]
       Ice_Magic_whoosh_01 = 457,
       [InspectorName("Effect/Projectile/Ice_Magic_whoosh_02")]
       Ice_Magic_whoosh_02 = 458,
       [InspectorName("Effect/Projectile/Ice_Buff_01")]
       Ice_Buff_01 = 459,
       [InspectorName("Effect/Projectile/Ice_Buff_02")]
       Ice_Buff_02 = 460,
       [InspectorName("Effect/Projectile/Ice_Buff_03")]
       Ice_Buff_03 = 461,
       [InspectorName("Effect/Projectile/Ice_Buff_04")]
       Ice_Buff_04 = 462,
       [InspectorName("Effect/Projectile/Ice_Buff_05")]
       Ice_Buff_05 = 463,
       [InspectorName("Effect/Projectile/Ice_conjure")]
       Ice_conjure = 464,
       [InspectorName("Effect/Projectile/Ice_hit_01")]
       Ice_hit_01 = 465,
       [InspectorName("Effect/Projectile/Ice_hit_02")]
       Ice_hit_02 = 466,
       [InspectorName("Effect/Projectile/Ice_hit_03")]
       Ice_hit_03 = 467,
       [InspectorName("Effect/Projectile/Ice_Magic_Buff_01")]
       Ice_Magic_Buff_01 = 468,
       [InspectorName("Effect/Projectile/Ice_Magic_Buff_02")]
       Ice_Magic_Buff_02 = 469,
       [InspectorName("Effect/Projectile/Ice_Magic_Buff_03")]
       Ice_Magic_Buff_03 = 470,
       [InspectorName("Effect/Projectile/Ice_spell_01")]
       Ice_spell_01 = 471,
       [InspectorName("Effect/Projectile/Ice_spell_02")]
       Ice_spell_02 = 472,
       [InspectorName("Effect/Projectile/Ice_spell_03")]
       Ice_spell_03 = 473,
       [InspectorName("Effect/Projectile/Ice_Spell_04")]
       Ice_Spell_04 = 474,
       [InspectorName("Effect/Projectile/Ice_Spell_05")]
       Ice_Spell_05 = 475,
       [InspectorName("Effect/Projectile/Ice_Spell_06")]
       Ice_Spell_06 = 476,
       [InspectorName("Effect/Projectile/Ice_Spell_07")]
       Ice_Spell_07 = 477,
       [InspectorName("Effect/Projectile/Ice_Spell_08")]
       Ice_Spell_08 = 478,
       [InspectorName("Effect/Projectile/Ice_Spell_09")]
       Ice_Spell_09 = 479,
       [InspectorName("Effect/Projectile/Ice_Spell_10")]
       Ice_Spell_10 = 480,
       [InspectorName("Effect/Projectile/Ice_Spell_011")]
       Ice_Spell_011 = 481,
       [InspectorName("Effect/Projectile/Ice_Spell_022")]
       Ice_Spell_022 = 482,
       [InspectorName("Effect/Projectile/Ice_Spell_033")]
       Ice_Spell_033 = 483,
       [InspectorName("Effect/Projectile/Lighting_Aura_loop")]
       Lighting_Aura_loop = 484,
       [InspectorName("Effect/Projectile/Lighting_Hit_01")]
       Lighting_Hit_01 = 485,
       [InspectorName("Effect/Projectile/Lighting_Hit_02")]
       Lighting_Hit_02 = 486,
       [InspectorName("Effect/Projectile/Lighting_Hit_03")]
       Lighting_Hit_03 = 487,
       [InspectorName("Effect/Projectile/Lighting_Magic_Buff_01")]
       Lighting_Magic_Buff_01 = 488,
       [InspectorName("Effect/Projectile/Lighting_Magic_Buff_02")]
       Lighting_Magic_Buff_02 = 489,
       [InspectorName("Effect/Projectile/Lighting_Magic_Buff_03")]
       Lighting_Magic_Buff_03 = 490,
       [InspectorName("Effect/Projectile/Lighting_Magic_Whoosh_01")]
       Lighting_Magic_Whoosh_01 = 491,
       [InspectorName("Effect/Projectile/Lighting_Magic_Whoosh_02")]
       Lighting_Magic_Whoosh_02 = 492,
       [InspectorName("Effect/Projectile/Lighting_Spell_01")]
       Lighting_Spell_01 = 493,
       [InspectorName("Effect/Projectile/Lighting_Spell_02")]
       Lighting_Spell_02 = 494,
       [InspectorName("Effect/Projectile/Lighting_Spell_03")]
       Lighting_Spell_03 = 495,
       [InspectorName("Effect/Projectile/Negative_Aura_01")]
       Negative_Aura_01 = 496,
       [InspectorName("Effect/Projectile/Negative_Aura_02")]
       Negative_Aura_02 = 497,
       [InspectorName("Effect/Projectile/Negative_Aura_03")]
       Negative_Aura_03 = 498,
       [InspectorName("Effect/Projectile/Negative_Aura_04")]
       Negative_Aura_04 = 499,
       [InspectorName("Effect/Projectile/Negative_Aura_05")]
       Negative_Aura_05 = 500,
       [InspectorName("Effect/Projectile/Negative_Aura_06")]
       Negative_Aura_06 = 501,
       [InspectorName("Effect/Projectile/Negative_Aura_07")]
       Negative_Aura_07 = 502,
       [InspectorName("Effect/Projectile/Negative_Aura_08")]
       Negative_Aura_08 = 503,
       [InspectorName("Effect/Projectile/Negative_Aura_09")]
       Negative_Aura_09 = 504,
       [InspectorName("Effect/Projectile/Negative_Aura_10")]
       Negative_Aura_10 = 505,
       [InspectorName("Effect/Projectile/Poison_aura")]
       Poison_aura = 506,
       [InspectorName("Effect/Projectile/Poison_Hit_01")]
       Poison_Hit_01 = 507,
       [InspectorName("Effect/Projectile/Poison_Hit_02")]
       Poison_Hit_02 = 508,
       [InspectorName("Effect/Projectile/Poison_Hit_03")]
       Poison_Hit_03 = 509,
       [InspectorName("Effect/Projectile/Poison_Hit_04")]
       Poison_Hit_04 = 510,
       [InspectorName("Effect/Projectile/Poison_Magic_Buff_01")]
       Poison_Magic_Buff_01 = 511,
       [InspectorName("Effect/Projectile/Poison_Magic_Buff_02")]
       Poison_Magic_Buff_02 = 512,
       [InspectorName("Effect/Projectile/Poison_Magic_Buff_03")]
       Poison_Magic_Buff_03 = 513,
       [InspectorName("Effect/Projectile/Poison_Magic_Whoosh_01")]
       Poison_Magic_Whoosh_01 = 514,
       [InspectorName("Effect/Projectile/Poison_Magic_Whoosh_02")]
       Poison_Magic_Whoosh_02 = 515,
       [InspectorName("Effect/Projectile/Poison_Spell_01")]
       Poison_Spell_01 = 516,
       [InspectorName("Effect/Projectile/Poison_Spell_02")]
       Poison_Spell_02 = 517,
       [InspectorName("Effect/Projectile/Poison_Spell_03")]
       Poison_Spell_03 = 518,
       [InspectorName("Effect/Projectile/Rocks_Magic_Buff_01")]
       Rocks_Magic_Buff_01 = 519,
       [InspectorName("Effect/Projectile/Rocks_Magic_Buff_02")]
       Rocks_Magic_Buff_02 = 520,
       [InspectorName("Effect/Projectile/Rocks_Magic_Buff_03")]
       Rocks_Magic_Buff_03 = 521,
       [InspectorName("Effect/Projectile/Rocks_Spell_01")]
       Rocks_Spell_01 = 522,
       [InspectorName("Effect/Projectile/Rocks_Spell_02")]
       Rocks_Spell_02 = 523,
       [InspectorName("Effect/Projectile/Rocks_Spell_03")]
       Rocks_Spell_03 = 524,
       [InspectorName("Effect/Projectile/Shadow_Buff_01")]
       Shadow_Buff_01 = 525,
       [InspectorName("Effect/Projectile/Shadow_Buff_02")]
       Shadow_Buff_02 = 526,
       [InspectorName("Effect/Projectile/Shadow_Buff_03")]
       Shadow_Buff_03 = 527,
       [InspectorName("Effect/Projectile/Shadow_Buff_04")]
       Shadow_Buff_04 = 528,
       [InspectorName("Effect/Projectile/Shadow_Buff_05")]
       Shadow_Buff_05 = 529,
       [InspectorName("Effect/Projectile/Shadow_Spell_01")]
       Shadow_Spell_01 = 530,
       [InspectorName("Effect/Projectile/Shadow_Spell_02")]
       Shadow_Spell_02 = 531,
       [InspectorName("Effect/Projectile/Shadow_Spell_03")]
       Shadow_Spell_03 = 532,
       [InspectorName("Effect/Projectile/Shadow_Spell_04")]
       Shadow_Spell_04 = 533,
       [InspectorName("Effect/Projectile/Shadow_Spell_05")]
       Shadow_Spell_05 = 534,
       [InspectorName("Effect/Projectile/Shadow_Spell_06")]
       Shadow_Spell_06 = 535,
       [InspectorName("Effect/Projectile/Shadow_Spell_07")]
       Shadow_Spell_07 = 536,
       [InspectorName("Effect/Projectile/Shadow_Spell_08")]
       Shadow_Spell_08 = 537,
       [InspectorName("Effect/Projectile/Shadow_Spell_09")]
       Shadow_Spell_09 = 538,
       [InspectorName("Effect/Projectile/Shadow_Spell_10")]
       Shadow_Spell_10 = 539,
       [InspectorName("Effect/Projectile/Water_Buff_01")]
       Water_Buff_01 = 540,
       [InspectorName("Effect/Projectile/Water_Buff_02")]
       Water_Buff_02 = 541,
       [InspectorName("Effect/Projectile/Water_Buff_03")]
       Water_Buff_03 = 542,
       [InspectorName("Effect/Projectile/Water_Buff_04")]
       Water_Buff_04 = 543,
       [InspectorName("Effect/Projectile/Water_Buff_05")]
       Water_Buff_05 = 544,
       [InspectorName("Effect/Projectile/Water_Hit_01")]
       Water_Hit_01 = 545,
       [InspectorName("Effect/Projectile/Water_Hit_02")]
       Water_Hit_02 = 546,
       [InspectorName("Effect/Projectile/Water_Hit_03")]
       Water_Hit_03 = 547,
       [InspectorName("Effect/Projectile/Water_Magic_Buff_01")]
       Water_Magic_Buff_01 = 548,
       [InspectorName("Effect/Projectile/Water_Magic_Buff_02")]
       Water_Magic_Buff_02 = 549,
       [InspectorName("Effect/Projectile/Water_Magic_Buff_03")]
       Water_Magic_Buff_03 = 550,
       [InspectorName("Effect/Projectile/Water_Magic_whoosh_01")]
       Water_Magic_whoosh_01 = 551,
       [InspectorName("Effect/Projectile/Water_Magic_whoosh_02")]
       Water_Magic_whoosh_02 = 552,
       [InspectorName("Effect/Projectile/Water_Shield")]
       Water_Shield = 553,
       [InspectorName("Effect/Projectile/Water_Spell_01")]
       Water_Spell_01 = 554,
       [InspectorName("Effect/Projectile/Water_Spell_02")]
       Water_Spell_02 = 555,
       [InspectorName("Effect/Projectile/Water_Spell_03")]
       Water_Spell_03 = 556,
       [InspectorName("Effect/Projectile/Water_Spell_04")]
       Water_Spell_04 = 557,
       [InspectorName("Effect/Projectile/Water_Spell_05")]
       Water_Spell_05 = 558,
       [InspectorName("Effect/Projectile/Water_Spell_06")]
       Water_Spell_06 = 559,
       [InspectorName("Effect/Projectile/Water_Spell_07")]
       Water_Spell_07 = 560,
       [InspectorName("Effect/Projectile/Water_Spell_08")]
       Water_Spell_08 = 561,
       [InspectorName("Effect/Projectile/Water_Spell_09")]
       Water_Spell_09 = 562,
       [InspectorName("Effect/Projectile/Water_Spell_10")]
       Water_Spell_10 = 563,
       [InspectorName("Effect/Projectile/Water_Spell_011")]
       Water_Spell_011 = 564,
       [InspectorName("Effect/Projectile/Water_Spell_022")]
       Water_Spell_022 = 565,
       [InspectorName("Effect/Projectile/Water_Spell_033")]
       Water_Spell_033 = 566,
       [InspectorName("Effect/Projectile/Wind_Aura_Loop")]
       Wind_Aura_Loop = 567,
       [InspectorName("Effect/Projectile/Wind_Buff_01")]
       Wind_Buff_01 = 568,
       [InspectorName("Effect/Projectile/Wind_Buff_02")]
       Wind_Buff_02 = 569,
       [InspectorName("Effect/Projectile/Wind_Buff_03")]
       Wind_Buff_03 = 570,
       [InspectorName("Effect/Projectile/Wind_Buff_04")]
       Wind_Buff_04 = 571,
       [InspectorName("Effect/Projectile/Wind_Buff_05")]
       Wind_Buff_05 = 572,
       [InspectorName("Effect/Projectile/Wind_Hit_01")]
       Wind_Hit_01 = 573,
       [InspectorName("Effect/Projectile/Wind_Hit_02")]
       Wind_Hit_02 = 574,
       [InspectorName("Effect/Projectile/Wind_Hit_03")]
       Wind_Hit_03 = 575,
       [InspectorName("Effect/Projectile/Wind_Magic_Buff_01")]
       Wind_Magic_Buff_01 = 576,
       [InspectorName("Effect/Projectile/Wind_Magic_Buff_02")]
       Wind_Magic_Buff_02 = 577,
       [InspectorName("Effect/Projectile/Wind_Magic_Buff_03")]
       Wind_Magic_Buff_03 = 578,
       [InspectorName("Effect/Projectile/Wind_Spell_01")]
       Wind_Spell_01 = 579,
       [InspectorName("Effect/Projectile/Wind_Spell_02")]
       Wind_Spell_02 = 580,
       [InspectorName("Effect/Projectile/Wind_Spell_03")]
       Wind_Spell_03 = 581,
       [InspectorName("Effect/Projectile/Wind_Spell_04")]
       Wind_Spell_04 = 582,
       [InspectorName("Effect/Projectile/Wind_Spell_05")]
       Wind_Spell_05 = 583,
       [InspectorName("Effect/Projectile/Wind_Spell_06")]
       Wind_Spell_06 = 584,
       [InspectorName("Effect/Projectile/Wind_Spell_07")]
       Wind_Spell_07 = 585,
       [InspectorName("Effect/Projectile/Wind_Spell_08")]
       Wind_Spell_08 = 586,
       [InspectorName("Effect/Projectile/Wind_Spell_09")]
       Wind_Spell_09 = 587,
       [InspectorName("Effect/Projectile/Wind_Spell_10")]
       Wind_Spell_10 = 588,
       [InspectorName("Effect/Projectile/Wind_Spell_011")]
       Wind_Spell_011 = 589,
       [InspectorName("Effect/Projectile/Wind_Spell_022")]
       Wind_Spell_022 = 590,
       [InspectorName("Effect/Projectile/Wind_Spell_033")]
       Wind_Spell_033 = 591,
       [InspectorName("Effect/Projectile/Wind_Whoosh_01")]
       Wind_Whoosh_01 = 592,
       [InspectorName("Effect/Projectile/Wind_Whoosh_02")]
       Wind_Whoosh_02 = 593,
       [InspectorName("BGM/Title/FantasyMusic7")]
       FantasyMusic7 = 594,
       [InspectorName("BGM/Battle/Action_Cinematic_Music_Pack_Volume_2_Track_6_Battle_Action")]
       Action_Cinematic_Music_Pack_Volume_2_Track_6_Battle_Action = 595,
       [InspectorName("BGM/Battle/Action_Cinematic_Music_Pack_Volume_3_Track_8_Battle_Action")]
       Action_Cinematic_Music_Pack_Volume_3_Track_8_Battle_Action = 596,
       [InspectorName("BGM/Battle/Classic_Orchestral_Music_Pack_Volume_1_Track_10")]
       Classic_Orchestral_Music_Pack_Volume_1_Track_10 = 597,
       [InspectorName("BGM/Battle/DarkFantasyStudio_Chainsaw")]
       DarkFantasyStudio_Chainsaw = 598,
       [InspectorName("BGM/Battle/Epic_Fantasy_Music_Pack_Volume_1_Track_6_Battle_Action")]
       Epic_Fantasy_Music_Pack_Volume_1_Track_6_Battle_Action = 599,
       [InspectorName("BGM/Battle/Epic_Fantasy_Music_Pack_Volume_1_Track_8_Battle_Action")]
       Epic_Fantasy_Music_Pack_Volume_1_Track_8_Battle_Action = 600,
       [InspectorName("BGM/Battle/Epic_Legend_Music_Pack_Volume_1_Track_7_Battle_Action")]
       Epic_Legend_Music_Pack_Volume_1_Track_7_Battle_Action = 601,
       [InspectorName("BGM/Battle/Epic_RPG_Music_Pack_Volume_1_Track_10_Battle_Action")]
       Epic_RPG_Music_Pack_Volume_1_Track_10_Battle_Action = 602,
       [InspectorName("BGM/Main/FantasyMusic1")]
       FantasyMusic1 = 603,
       [InspectorName("BGM/Main/ForestInstrumentalAmbDay")]
       ForestInstrumentalAmbDay = 604,
       [InspectorName("BGM/Main/WildernessInstrumentalAmb")]
       WildernessInstrumentalAmb = 605,
       [InspectorName("BGM/Dungeon/NatureAmbience")]
       NatureAmbience = 606,
       [InspectorName("BGM/Dungeon/DarkFantasyStudio_Tableturning")]
       DarkFantasyStudio_Tableturning = 607,
       [InspectorName("BGM/Dungeon/DarkFantasyStudio_Darkcircus")]
       DarkFantasyStudio_Darkcircus = 608,
       [InspectorName("BGM/Dungeon/DarkFantasyStudio_Thecursedquest")]
       DarkFantasyStudio_Thecursedquest = 609,
       [InspectorName("BGM/Dungeon/DarkFantasyStudioHiddenfromtheworld")]
       DarkFantasyStudioHiddenfromtheworld = 610,
       [InspectorName("BGM/Dungeon/Action_Cinematic_Music_Pack_Volume_3_Track_2_Introduction")]
       Action_Cinematic_Music_Pack_Volume_3_Track_2_Introduction = 611,
       [InspectorName("BGM/Dungeon/Classic_Orchestral_Music_Pack_Volume_1_Track_4")]
       Classic_Orchestral_Music_Pack_Volume_1_Track_4 = 612,
       [InspectorName("BGM/Dungeon/Epic_Documentary_Music_Pack_Volume_1_Track_4")]
       Epic_Documentary_Music_Pack_Volume_1_Track_4 = 613,
       [InspectorName("BGM/Dungeon/Epic_Fantasy_Music_Pack_Volume_1_Track_5_Gameplay_Ambience")]
       Epic_Fantasy_Music_Pack_Volume_1_Track_5_Gameplay_Ambience = 614,
       [InspectorName("BGM/Dungeon/Epic_Legend_Music_Pack_Volume_1_Track_4_Gameplay_Ambience")]
       Epic_Legend_Music_Pack_Volume_1_Track_4_Gameplay_Ambience = 615,
       [InspectorName("BGM/Dungeon/Epic_RPG_Music_Pack_Volume_1_Track_7_Gameplay_Ambience")]
       Epic_RPG_Music_Pack_Volume_1_Track_7_Gameplay_Ambience = 616,
       [InspectorName("BGM/Dungeon/Peplum_Music_Pack_Volume_1_Track_8_Battle_Action")]
       Peplum_Music_Pack_Volume_1_Track_8_Battle_Action = 617,
       [InspectorName("ETC/BackGround/Beach1Loop")]
       Beach1Loop = 618,
       [InspectorName("ETC/BackGround/Beach3Loop")]
       Beach3Loop = 619,
       [InspectorName("ETC/BackGround/Beach2Loop")]
       Beach2Loop = 620,
       [InspectorName("ETC/BackGround/Fire1Loop")]
       Fire1Loop = 621,
       [InspectorName("ETC/BackGround/Jungle1")]
       Jungle1 = 622,
       [InspectorName("ETC/BackGround/Jungle1Loop")]
       Jungle1Loop = 623,
       [InspectorName("ETC/BackGround/Jungle2Loop")]
       Jungle2Loop = 624,
       [InspectorName("ETC/BackGround/Jungle6")]
       Jungle6 = 625,
       [InspectorName("ETC/BackGround/Jungle3")]
       Jungle3 = 626,
       [InspectorName("ETC/BackGround/Jungle7")]
       Jungle7 = 627,
       [InspectorName("ETC/BackGround/Jungle6Loop")]
       Jungle6Loop = 628,
       [InspectorName("ETC/BackGround/Jungle4Loop")]
       Jungle4Loop = 629,
       [InspectorName("ETC/BackGround/jungle5")]
       jungle5 = 630,
       [InspectorName("ETC/BackGround/Rain1Loop")]
       Rain1Loop = 631,
       [InspectorName("ETC/BackGround/Rain2Loop")]
       Rain2Loop = 632,
       [InspectorName("ETC/BackGround/Water1Loop")]
       Water1Loop = 633,
       [InspectorName("ETC/BackGround/Wind1Loop")]
       Wind1Loop = 634,
       [InspectorName("Effect/Hit/Explosion2")]
       Explosion2 = 635,
       [InspectorName("Effect/Hit/FireExplosion3")]
       FireExplosion3 = 636,
       [InspectorName("Effect/Explosion/EXPLDsgn_Grenade_Explosion_01")]
       EXPLDsgn_Grenade_Explosion_01 = 637,
       [InspectorName("Effect/Explosion/EXPLDsgn_Grenade_Explosion_02")]
       EXPLDsgn_Grenade_Explosion_02 = 638,
       [InspectorName("Effect/Explosion/EXPLDsgn_Grenade_Explosion_03")]
       EXPLDsgn_Grenade_Explosion_03 = 639,
       [InspectorName("Effect/Explosion/EXPLOSION_Long_Impact_with_Long_Tail_stereo")]
       EXPLOSION_Long_Impact_with_Long_Tail_stereo = 640,
       [InspectorName("Effect/Explosion/EXPLOSION_Medium_Blast_Scatter_Debris_Long_Faint_Tail_stereo")]
       EXPLOSION_Medium_Blast_Scatter_Debris_Long_Faint_Tail_stereo = 641,
       [InspectorName("Effect/Explosion/EXPLOSION_Medium_Blast_Scatter_Debris_stereo")]
       EXPLOSION_Medium_Blast_Scatter_Debris_stereo = 642,
       [InspectorName("Effect/Explosion/EXPLOSION_Medium_Bright_Impact_Rumble_Quick_Tail_stereo")]
       EXPLOSION_Medium_Bright_Impact_Rumble_Quick_Tail_stereo = 643,
       [InspectorName("Effect/Explosion/EXPLOSION_Medium_Bright_Kickback_stereo")]
       EXPLOSION_Medium_Bright_Kickback_stereo = 644,
       [InspectorName("Effect/Explosion/EXPLOSION_Medium_Bright_Reverbed_Kickback_stereo")]
       EXPLOSION_Medium_Bright_Reverbed_Kickback_stereo = 645,
       [InspectorName("Effect/Explosion/EXPLOSION_Medium_Dirty_Crackle_stereo")]
       EXPLOSION_Medium_Dirty_Crackle_stereo = 646,
       [InspectorName("Effect/Explosion/EXPLOSION_Short_Bang_Reverb_stereo")]
       EXPLOSION_Short_Bang_Reverb_stereo = 647,
       [InspectorName("Effect/Explosion/EXPLOSION_Short_Distorted_stereo")]
       EXPLOSION_Short_Distorted_stereo = 648,
       [InspectorName("Effect/Projectile/BloodPostEffect")]
       BloodPostEffect = 649,
       [InspectorName("Effect/Projectile/BloodExplosion")]
       BloodExplosion = 650,
       [InspectorName("Effect/Buff/Fly")]
       Fly = 651,
       [InspectorName("Effect/Explosion/Lightning_1")]
       Lightning_1 = 652,
       [InspectorName("Effect/Explosion/Temporary_Explosion")]
       Temporary_Explosion = 653,
       [InspectorName("Effect/Monster_FindNear/Soldier_Hunter_Surprised_1")]
       Soldier_Hunter_Surprised_1 = 654,
       [InspectorName("Effect/Monster_FindNear/Soldier_Hunter_Surprised_2")]
       Soldier_Hunter_Surprised_2 = 655,
       [InspectorName("Effect/Monster_FindNear/Soldier_Hunter_Surprised_3")]
       Soldier_Hunter_Surprised_3 = 656,
       [InspectorName("Effect/Monster_FindNear/Soldier_Hunter_Surprised_4")]
       Soldier_Hunter_Surprised_4 = 657,
       [InspectorName("Effect/Monster_FindNear/Soldier_Hunter_Surprised_5")]
       Soldier_Hunter_Surprised_5 = 658,
       [InspectorName("Effect/Monster_FindNear/Soldier_Hunter_Surprised_6")]
       Soldier_Hunter_Surprised_6 = 659,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem01_KRST_NONE")]
       MAGMisc_ChooseRewardItem01_KRST_NONE = 660,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem02_KRST_NONE")]
       MAGMisc_ChooseRewardItem02_KRST_NONE = 661,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem03_KRST_NONE")]
       MAGMisc_ChooseRewardItem03_KRST_NONE = 662,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem04_KRST_NONE")]
       MAGMisc_ChooseRewardItem04_KRST_NONE = 663,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem05_KRST_NONE")]
       MAGMisc_ChooseRewardItem05_KRST_NONE = 664,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem06_KRST_NONE")]
       MAGMisc_ChooseRewardItem06_KRST_NONE = 665,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem07_KRST_NONE")]
       MAGMisc_ChooseRewardItem07_KRST_NONE = 666,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem08_KRST_NONE")]
       MAGMisc_ChooseRewardItem08_KRST_NONE = 667,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem09_KRST_NONE")]
       MAGMisc_ChooseRewardItem09_KRST_NONE = 668,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem10_KRST_NONE")]
       MAGMisc_ChooseRewardItem10_KRST_NONE = 669,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem11_KRST_NONE")]
       MAGMisc_ChooseRewardItem11_KRST_NONE = 670,
       [InspectorName("UI/Buttons/MAGMisc_ChooseRewardItem12_KRST_NONE")]
       MAGMisc_ChooseRewardItem12_KRST_NONE = 671,
       [InspectorName("UI/Buttons/MAGMisc_UpgradeButton01_KRST_NONE")]
       MAGMisc_UpgradeButton01_KRST_NONE = 672,
       [InspectorName("UI/Buttons/MAGMisc_UpgradeButton02_KRST_NONE")]
       MAGMisc_UpgradeButton02_KRST_NONE = 673,
       [InspectorName("UI/Buttons/MAGMisc_UpgradeButton03_KRST_NONE")]
       MAGMisc_UpgradeButton03_KRST_NONE = 674,
       [InspectorName("UI/Buttons/MAGMisc_UpgradeButton04_KRST_NONE")]
       MAGMisc_UpgradeButton04_KRST_NONE = 675,
       [InspectorName("UI/Buttons/MAGMisc_UpgradeButton05_KRST_NONE")]
       MAGMisc_UpgradeButton05_KRST_NONE = 676,
       [InspectorName("UI/Buttons/MAGMisc_UpgradeButton06_KRST_NONE")]
       MAGMisc_UpgradeButton06_KRST_NONE = 677,
       [InspectorName("UI/Buttons/MAGMisc_UpgradeButton07_KRST_NONE")]
       MAGMisc_UpgradeButton07_KRST_NONE = 678,
       [InspectorName("UI/Buttons/UI_Click_Aftertap_mono")]
       UI_Click_Aftertap_mono = 679,
       [InspectorName("UI/Buttons/UI_Click_Cut_mono")]
       UI_Click_Cut_mono = 680,
       [InspectorName("UI/Buttons/UI_Click_Deep_mono")]
       UI_Click_Deep_mono = 681,
       [InspectorName("UI/Buttons/UI_Click_Distinct_mono")]
       UI_Click_Distinct_mono = 682,
       [InspectorName("UI/Buttons/UI_Click_Distinct_Short_mono")]
       UI_Click_Distinct_Short_mono = 683,
       [InspectorName("UI/Buttons/UI_Click_Metallic_Bright_mono")]
       UI_Click_Metallic_Bright_mono = 684,
       [InspectorName("UI/Buttons/UI_Click_Metallic_mono")]
       UI_Click_Metallic_mono = 685,
       [InspectorName("UI/Buttons/UI_Click_Organic_mono")]
       UI_Click_Organic_mono = 686,
       [InspectorName("UI/Buttons/UI_Click_Smooth_mono")]
       UI_Click_Smooth_mono = 687,
       [InspectorName("UI/Buttons/UI_Click_Snappy_mono")]
       UI_Click_Snappy_mono = 688,
       [InspectorName("UI/Buttons/UI_Click_Subtle_mono")]
       UI_Click_Subtle_mono = 689,
       [InspectorName("UI/Buttons/UI_Click_Tap_Dirty_mono")]
       UI_Click_Tap_Dirty_mono = 690,
       [InspectorName("UI/Buttons/UI_Click_Tap_Hybrid_mono")]
       UI_Click_Tap_Hybrid_mono = 691,
       [InspectorName("UI/Buttons/UI_Click_Tap_Hybrid_Muffled_mono")]
       UI_Click_Tap_Hybrid_Muffled_mono = 692,
       [InspectorName("UI/Buttons/UI_Click_Tap_Hybrid_Smooth_mono")]
       UI_Click_Tap_Hybrid_Smooth_mono = 693,
       [InspectorName("UI/Buttons/UI_Click_Tap_Knock_Subtle_Dark_mono")]
       UI_Click_Tap_Knock_Subtle_Dark_mono = 694,
       [InspectorName("UI/Buttons/UI_Click_Tap_Knock_Subtle_Darker_mono")]
       UI_Click_Tap_Knock_Subtle_Darker_mono = 695,
       [InspectorName("UI/Buttons/UI_Click_Tap_Knock_Subtle_mono")]
       UI_Click_Tap_Knock_Subtle_mono = 696,
       [InspectorName("UI/Buttons/UI_Click_Tap_Noisy_Bright_mono")]
       UI_Click_Tap_Noisy_Bright_mono = 697,
       [InspectorName("UI/Buttons/UI_Click_Tap_Noisy_Long_mono")]
       UI_Click_Tap_Noisy_Long_mono = 698,
       [InspectorName("UI/Buttons/UI_Click_Tap_Noisy_mono")]
       UI_Click_Tap_Noisy_mono = 699,
       [InspectorName("UI/Buttons/UI_Click_Tap_Noisy_Subtle_mono")]
       UI_Click_Tap_Noisy_Subtle_mono = 700,
       [InspectorName("UI/Buttons/UI_Click_Tap_Short_mono")]
       UI_Click_Tap_Short_mono = 701,
       [InspectorName("UI/Buttons/UI_Click_Tap_Subtle_Alternative_mono")]
       UI_Click_Tap_Subtle_Alternative_mono = 702,
       [InspectorName("UI/Buttons/UI_Click_Tap_Subtle_mono")]
       UI_Click_Tap_Subtle_mono = 703,
       [InspectorName("UI/Buttons/UIClick_CloseButton01_KRST_NONE")]
       UIClick_CloseButton01_KRST_NONE = 704,
       [InspectorName("UI/Buttons/UIClick_CloseButton02_KRST_NONE")]
       UIClick_CloseButton02_KRST_NONE = 705,
       [InspectorName("UI/Buttons/UIClick_CloseButton03_KRST_NONE")]
       UIClick_CloseButton03_KRST_NONE = 706,
       [InspectorName("UI/Buttons/UIClick_CloseButton04_KRST_NONE")]
       UIClick_CloseButton04_KRST_NONE = 707,
       [InspectorName("UI/Buttons/UIClick_CloseButton05_KRST_NONE")]
       UIClick_CloseButton05_KRST_NONE = 708,
       [InspectorName("UI/Buttons/UIClick_CloseButton06_KRST_NONE")]
       UIClick_CloseButton06_KRST_NONE = 709,
       [InspectorName("UI/Buttons/UIClick_CloseButton07_KRST_NONE")]
       UIClick_CloseButton07_KRST_NONE = 710,
       [InspectorName("UI/Buttons/UIClick_CloseButton08_KRST_NONE")]
       UIClick_CloseButton08_KRST_NONE = 711,
       [InspectorName("UI/Buttons/UIClick_CloseButton09_KRST_NONE")]
       UIClick_CloseButton09_KRST_NONE = 712,
       [InspectorName("UI/Buttons/UIClick_CloseButton10_KRST_NONE")]
       UIClick_CloseButton10_KRST_NONE = 713,
       [InspectorName("UI/Buttons/UIClick_CloseButton11_KRST_NONE")]
       UIClick_CloseButton11_KRST_NONE = 714,
       [InspectorName("UI/Buttons/UIClick_CloseButton12_KRST_NONE")]
       UIClick_CloseButton12_KRST_NONE = 715,
       [InspectorName("UI/Buttons/UIClick_CloseButton13_KRST_NONE")]
       UIClick_CloseButton13_KRST_NONE = 716,
       [InspectorName("UI/Buttons/UIClick_CloseButton14_KRST_NONE")]
       UIClick_CloseButton14_KRST_NONE = 717,
       [InspectorName("UI/Buttons/UIClick_NeutralButton01_KRST_NONE")]
       UIClick_NeutralButton01_KRST_NONE = 718,
       [InspectorName("UI/Buttons/UIClick_NeutralButton02_KRST_NONE")]
       UIClick_NeutralButton02_KRST_NONE = 719,
       [InspectorName("UI/Buttons/UIClick_NeutralButton03_KRST_NONE")]
       UIClick_NeutralButton03_KRST_NONE = 720,
       [InspectorName("UI/Buttons/UIClick_NeutralButton04_KRST_NONE")]
       UIClick_NeutralButton04_KRST_NONE = 721,
       [InspectorName("UI/Buttons/UIClick_NeutralButton05_KRST_NONE")]
       UIClick_NeutralButton05_KRST_NONE = 722,
       [InspectorName("UI/Buttons/UIClick_NeutralButton06_KRST_NONE")]
       UIClick_NeutralButton06_KRST_NONE = 723,
       [InspectorName("UI/Buttons/UIClick_NeutralButton07_KRST_NONE")]
       UIClick_NeutralButton07_KRST_NONE = 724,
       [InspectorName("UI/Buttons/UIClick_NeutralButton08_KRST_NONE")]
       UIClick_NeutralButton08_KRST_NONE = 725,
       [InspectorName("UI/Buttons/UIClick_NeutralButton09_KRST_NONE")]
       UIClick_NeutralButton09_KRST_NONE = 726,
       [InspectorName("UI/Buttons/UIClick_NeutralButton10_KRST_NONE")]
       UIClick_NeutralButton10_KRST_NONE = 727,
       [InspectorName("UI/Buttons/UIClick_NeutralButton11_KRST_NONE")]
       UIClick_NeutralButton11_KRST_NONE = 728,
       [InspectorName("UI/Buttons/UIClick_NeutralButton12_KRST_NONE")]
       UIClick_NeutralButton12_KRST_NONE = 729,
       [InspectorName("UI/Buttons/UIClick_NeutralButton13_KRST_NONE")]
       UIClick_NeutralButton13_KRST_NONE = 730,
       [InspectorName("UI/Buttons/UIClick_NeutralButton14_KRST_NONE")]
       UIClick_NeutralButton14_KRST_NONE = 731,
       [InspectorName("UI/Buttons/UIClick_NeutralButton15_KRST_NONE")]
       UIClick_NeutralButton15_KRST_NONE = 732,
       [InspectorName("UI/Buttons/UIClick_NeutralButton16_KRST_NONE")]
       UIClick_NeutralButton16_KRST_NONE = 733,
       [InspectorName("UI/Buttons/UIClick_NeutralButton17_KRST_NONE")]
       UIClick_NeutralButton17_KRST_NONE = 734,
       [InspectorName("UI/Buttons/UIClick_NeutralButton18_KRST_NONE")]
       UIClick_NeutralButton18_KRST_NONE = 735,
       [InspectorName("UI/Buttons/UIClick_NeutralButton19_KRST_NONE")]
       UIClick_NeutralButton19_KRST_NONE = 736,
       [InspectorName("UI/Buttons/UIClick_NeutralButton20_KRST_NONE")]
       UIClick_NeutralButton20_KRST_NONE = 737,
       [InspectorName("UI/Buttons/UIClick_NeutralButton21_KRST_NONE")]
       UIClick_NeutralButton21_KRST_NONE = 738,
       [InspectorName("UI/Collects/coins_2")]
       coins_2 = 739,
       [InspectorName("UI/Collects/coins_4")]
       coins_4 = 740,
       [InspectorName("UI/Collects/coins_15")]
       coins_15 = 741,
       [InspectorName("UI/Collects/coins_63")]
       coins_63 = 742,
       [InspectorName("UI/Collects/Collective_Coins_01")]
       Collective_Coins_01 = 743,
       [InspectorName("UI/Collects/Collective_Coins_02")]
       Collective_Coins_02 = 744,
       [InspectorName("UI/Collects/Collective_Coins_03")]
       Collective_Coins_03 = 745,
       [InspectorName("UI/Collects/Collective_Coins_04")]
       Collective_Coins_04 = 746,
       [InspectorName("UI/HUD/MAGMisc_CharacterLevelUp01_KRST_NONE")]
       MAGMisc_CharacterLevelUp01_KRST_NONE = 747,
       [InspectorName("UI/HUD/MAGMisc_CharacterLevelUp02_KRST_NONE")]
       MAGMisc_CharacterLevelUp02_KRST_NONE = 748,
       [InspectorName("UI/HUD/MAGMisc_CharacterLevelUp03_KRST_NONE")]
       MAGMisc_CharacterLevelUp03_KRST_NONE = 749,
       [InspectorName("UI/HUD/MAGMisc_CharacterLevelUp04_KRST_NONE")]
       MAGMisc_CharacterLevelUp04_KRST_NONE = 750,
       [InspectorName("UI/HUD/MAGMisc_CharacterLevelUp05_KRST_NONE")]
       MAGMisc_CharacterLevelUp05_KRST_NONE = 751,
       [InspectorName("UI/HUD/MAGMisc_HubMenuAppears01_KRST_NONE")]
       MAGMisc_HubMenuAppears01_KRST_NONE = 752,
       [InspectorName("UI/HUD/MAGMisc_HubMenuAppears02_KRST_NONE")]
       MAGMisc_HubMenuAppears02_KRST_NONE = 753,
       [InspectorName("UI/HUD/MAGMisc_HubMenuAppears03_KRST_NONE")]
       MAGMisc_HubMenuAppears03_KRST_NONE = 754,
       [InspectorName("UI/HUD/UIClick_MouseoverTheSkills01_KRST_NONE")]
       UIClick_MouseoverTheSkills01_KRST_NONE = 755,
       [InspectorName("UI/HUD/UIClick_MouseoverTheSkills02_KRST_NONE")]
       UIClick_MouseoverTheSkills02_KRST_NONE = 756,
       [InspectorName("UI/HUD/UIClick_MouseoverTheSkills03_KRST_NONE")]
       UIClick_MouseoverTheSkills03_KRST_NONE = 757,
       [InspectorName("UI/HUD/UIClick_MouseoverTheSkills04_KRST_NONE")]
       UIClick_MouseoverTheSkills04_KRST_NONE = 758,
       [InspectorName("UI/HUD/UIClick_MouseoverTheSkills05_KRST_NONE")]
       UIClick_MouseoverTheSkills05_KRST_NONE = 759,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot01_KRST_NONE")]
       UIClick_OpenSkillQuickSlot01_KRST_NONE = 760,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot02_KRST_NONE")]
       UIClick_OpenSkillQuickSlot02_KRST_NONE = 761,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot03_KRST_NONE")]
       UIClick_OpenSkillQuickSlot03_KRST_NONE = 762,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot04_KRST_NONE")]
       UIClick_OpenSkillQuickSlot04_KRST_NONE = 763,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot05_KRST_NONE")]
       UIClick_OpenSkillQuickSlot05_KRST_NONE = 764,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot06_KRST_NONE")]
       UIClick_OpenSkillQuickSlot06_KRST_NONE = 765,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot07_KRST_NONE")]
       UIClick_OpenSkillQuickSlot07_KRST_NONE = 766,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot08_KRST_NONE")]
       UIClick_OpenSkillQuickSlot08_KRST_NONE = 767,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot09_KRST_NONE")]
       UIClick_OpenSkillQuickSlot09_KRST_NONE = 768,
       [InspectorName("UI/HUD/UIClick_OpenSkillQuickSlot10_KRST_NONE")]
       UIClick_OpenSkillQuickSlot10_KRST_NONE = 769,
       [InspectorName("UI/HUD/UIClick_SelectTypeOfMagic01_KRST_NONE")]
       UIClick_SelectTypeOfMagic01_KRST_NONE = 770,
       [InspectorName("UI/HUD/UIClick_SelectTypeOfMagic02_KRST_NONE")]
       UIClick_SelectTypeOfMagic02_KRST_NONE = 771,
       [InspectorName("UI/HUD/UIClick_SelectTypeOfMagic03_KRST_NONE")]
       UIClick_SelectTypeOfMagic03_KRST_NONE = 772,
       [InspectorName("UI/HUD/UIClick_SelectTypeOfMagic04_KRST_NONE")]
       UIClick_SelectTypeOfMagic04_KRST_NONE = 773,
       [InspectorName("UI/HUD/UIClick_SelectTypeOfMagic05_KRST_NONE")]
       UIClick_SelectTypeOfMagic05_KRST_NONE = 774,
       [InspectorName("UI/HUD/UIClick_SelectTypeOfMagic06_KRST_NONE")]
       UIClick_SelectTypeOfMagic06_KRST_NONE = 775,
       [InspectorName("UI/HUD/UIClick_SelectTypeOfMagic07_KRST_NONE")]
       UIClick_SelectTypeOfMagic07_KRST_NONE = 776,
       [InspectorName("UI/HUD/UIClick_SelectTypeOfMagic08_KRST_NONE")]
       UIClick_SelectTypeOfMagic08_KRST_NONE = 777,
       [InspectorName("UI/HUD/UIClick_SelectTypeOfMagic09_KRST_NONE")]
       UIClick_SelectTypeOfMagic09_KRST_NONE = 778,
       [InspectorName("UI/Characters/CLOTHMisc_WearBoots02_KRST_NONE")]
       CLOTHMisc_WearBoots02_KRST_NONE = 779,
       [InspectorName("UI/Characters/CLOTHMisc_WearBoots03_KRST_NONE")]
       CLOTHMisc_WearBoots03_KRST_NONE = 780,
       [InspectorName("UI/Characters/CLOTHMisc_WearBoots04_KRST_NONE")]
       CLOTHMisc_WearBoots04_KRST_NONE = 781,
       [InspectorName("UI/Characters/FOLYClth_WearAccessories01_KRST_NONE")]
       FOLYClth_WearAccessories01_KRST_NONE = 782,
       [InspectorName("UI/Characters/FOLYClth_WearAccessories02_KRST_NONE")]
       FOLYClth_WearAccessories02_KRST_NONE = 783,
       [InspectorName("UI/Characters/FOLYClth_WearAccessories03_KRST_NONE")]
       FOLYClth_WearAccessories03_KRST_NONE = 784,
       [InspectorName("UI/Characters/FOLYClth_WearAccessories04_KRST_NONE")]
       FOLYClth_WearAccessories04_KRST_NONE = 785,
       [InspectorName("UI/Characters/METLTonl_WearRing01_KRST_NONE")]
       METLTonl_WearRing01_KRST_NONE = 786,
       [InspectorName("UI/Characters/METLTonl_WearRing02_KRST_NONE")]
       METLTonl_WearRing02_KRST_NONE = 787,
       [InspectorName("UI/Characters/METLTonl_WearRing03_KRST_NONE")]
       METLTonl_WearRing03_KRST_NONE = 788,
       [InspectorName("UI/Characters/OBJJewl_SetJewelInHole01_KRST_NONE")]
       OBJJewl_SetJewelInHole01_KRST_NONE = 789,
       [InspectorName("UI/Characters/OBJJewl_SetJewelInHole02_KRST_NONE")]
       OBJJewl_SetJewelInHole02_KRST_NONE = 790,
       [InspectorName("UI/Characters/OBJJewl_SetJewelInHole03_KRST_NONE")]
       OBJJewl_SetJewelInHole03_KRST_NONE = 791,
       [InspectorName("UI/Characters/OBJJewl_SetJewelInHole04_KRST_NONE")]
       OBJJewl_SetJewelInHole04_KRST_NONE = 792,
       [InspectorName("UI/Characters/OBJJewl_WearAmulet01_KRST_NONE")]
       OBJJewl_WearAmulet01_KRST_NONE = 793,
       [InspectorName("UI/Characters/OBJJewl_WearAmulet02_KRST_NONE")]
       OBJJewl_WearAmulet02_KRST_NONE = 794,
       [InspectorName("UI/Characters/OBJJewl_WearAmulet03_KRST_NONE")]
       OBJJewl_WearAmulet03_KRST_NONE = 795,
       [InspectorName("UI/Characters/UIClick_AdjustStats01_KRST_NONE")]
       UIClick_AdjustStats01_KRST_NONE = 796,
       [InspectorName("UI/Characters/UIClick_AdjustStats02_KRST_NONE")]
       UIClick_AdjustStats02_KRST_NONE = 797,
       [InspectorName("UI/Characters/UIClick_AdjustStats03_KRST_NONE")]
       UIClick_AdjustStats03_KRST_NONE = 798,
       [InspectorName("UI/Characters/UIClick_AdjustStats04_KRST_NONE")]
       UIClick_AdjustStats04_KRST_NONE = 799,
       [InspectorName("UI/Characters/UIClick_AdjustStats05_KRST_NONE")]
       UIClick_AdjustStats05_KRST_NONE = 800,
       [InspectorName("UI/Characters/UIClick_AdjustStats06_KRST_NONE")]
       UIClick_AdjustStats06_KRST_NONE = 801,
       [InspectorName("UI/Characters/UIClick_AdjustStats07_KRST_NONE")]
       UIClick_AdjustStats07_KRST_NONE = 802,
       [InspectorName("UI/Characters/WEAPArmr_WearChestArmor01_KRST_NONE")]
       WEAPArmr_WearChestArmor01_KRST_NONE = 803,
       [InspectorName("UI/Characters/WEAPArmr_WearChestArmor02_KRST_NONE")]
       WEAPArmr_WearChestArmor02_KRST_NONE = 804,
       [InspectorName("UI/Characters/WEAPArmr_WearChestArmor03_KRST_NONE")]
       WEAPArmr_WearChestArmor03_KRST_NONE = 805,
       [InspectorName("UI/Characters/WEAPArmr_WearChestArmor04_KRST_NONE")]
       WEAPArmr_WearChestArmor04_KRST_NONE = 806,
       [InspectorName("UI/Characters/WEAPArmr_WearGloves01_KRST_NONE")]
       WEAPArmr_WearGloves01_KRST_NONE = 807,
       [InspectorName("UI/Characters/WEAPArmr_WearGloves02_KRST_NONE")]
       WEAPArmr_WearGloves02_KRST_NONE = 808,
       [InspectorName("UI/Characters/WEAPArmr_WearGloves03_KRST_NONE")]
       WEAPArmr_WearGloves03_KRST_NONE = 809,
       [InspectorName("UI/Characters/WEAPArmr_WearGloves04_KRST_NONE")]
       WEAPArmr_WearGloves04_KRST_NONE = 810,
       [InspectorName("UI/Characters/WEAPArmr_WearHandShield01_KRST_NONE")]
       WEAPArmr_WearHandShield01_KRST_NONE = 811,
       [InspectorName("UI/Characters/WEAPArmr_WearHandShield02_KRST_NONE")]
       WEAPArmr_WearHandShield02_KRST_NONE = 812,
       [InspectorName("UI/Characters/WEAPArmr_WearHandShield03_KRST_NONE")]
       WEAPArmr_WearHandShield03_KRST_NONE = 813,
       [InspectorName("UI/Characters/WEAPArmr_WearHandShield04_KRST_NONE")]
       WEAPArmr_WearHandShield04_KRST_NONE = 814,
       [InspectorName("UI/Characters/WEAPArmr_WearHandShield05_KRST_NONE")]
       WEAPArmr_WearHandShield05_KRST_NONE = 815,
       [InspectorName("UI/Characters/WEAPArmr_WearHandShield06_KRST_NONE")]
       WEAPArmr_WearHandShield06_KRST_NONE = 816,
       [InspectorName("UI/Characters/WEAPArmr_WearHelmet01_KRST_NONE")]
       WEAPArmr_WearHelmet01_KRST_NONE = 817,
       [InspectorName("UI/Characters/WEAPArmr_WearHelmet02_KRST_NONE")]
       WEAPArmr_WearHelmet02_KRST_NONE = 818,
       [InspectorName("UI/Characters/WEAPArmr_WearHelmet03_KRST_NONE")]
       WEAPArmr_WearHelmet03_KRST_NONE = 819,
       [InspectorName("UI/Characters/WEAPArmr_WearHelmet04_KRST_NONE")]
       WEAPArmr_WearHelmet04_KRST_NONE = 820,
       [InspectorName("UI/Characters/WEAPArmr_WearHelmet05_KRST_NONE")]
       WEAPArmr_WearHelmet05_KRST_NONE = 821,
       [InspectorName("UI/Characters/WEAPArmr_WearPants01_KRST_NONE")]
       WEAPArmr_WearPants01_KRST_NONE = 822,
       [InspectorName("UI/Characters/WEAPArmr_WearPants02_KRST_NONE")]
       WEAPArmr_WearPants02_KRST_NONE = 823,
       [InspectorName("UI/Characters/WEAPArmr_WearPants03_KRST_NONE")]
       WEAPArmr_WearPants03_KRST_NONE = 824,
       [InspectorName("UI/Characters/WEAPArmr_WearPants04_KRST_NONE")]
       WEAPArmr_WearPants04_KRST_NONE = 825,
       [InspectorName("UI/Characters/WEAPArmr_WearPants05_KRST_NONE")]
       WEAPArmr_WearPants05_KRST_NONE = 826,
       [InspectorName("UI/Characters/WEAPBow_WearBow01_KRST_NONE")]
       WEAPBow_WearBow01_KRST_NONE = 827,
       [InspectorName("UI/Characters/WEAPBow_WearBow02_KRST_NONE")]
       WEAPBow_WearBow02_KRST_NONE = 828,
       [InspectorName("UI/Characters/WEAPBow_WearCrossbow_KRST_NONE")]
       WEAPBow_WearCrossbow_KRST_NONE = 829,
       [InspectorName("UI/Characters/WEAPMisc_WearMetalWeapon01_KRST_NONE")]
       WEAPMisc_WearMetalWeapon01_KRST_NONE = 830,
       [InspectorName("UI/Characters/WEAPMisc_WearMetalWeapon02_KRST_NONE")]
       WEAPMisc_WearMetalWeapon02_KRST_NONE = 831,
       [InspectorName("UI/Characters/WEAPMisc_WearMetalWeapon03_KRST_NONE")]
       WEAPMisc_WearMetalWeapon03_KRST_NONE = 832,
       [InspectorName("UI/Characters/WEAPMisc_WearMetalWeapon04_KRST_NONE")]
       WEAPMisc_WearMetalWeapon04_KRST_NONE = 833,
       [InspectorName("UI/Characters/WEAPMisc_WearMetalWeapon05_KRST_NONE")]
       WEAPMisc_WearMetalWeapon05_KRST_NONE = 834,
       [InspectorName("UI/Characters/WEAPMisc_WearMetalWeapon06_KRST_NONE")]
       WEAPMisc_WearMetalWeapon06_KRST_NONE = 835,
       [InspectorName("UI/Characters/WEAPMisc_WearMetalWeapon07_KRST_NONE")]
       WEAPMisc_WearMetalWeapon07_KRST_NONE = 836,
       [InspectorName("UI/Characters/WEAPMisc_WearWoodWeapon01_KRST_NONE")]
       WEAPMisc_WearWoodWeapon01_KRST_NONE = 837,
       [InspectorName("UI/Characters/WEAPMisc_WearWoodWeapon02_KRST_NONE")]
       WEAPMisc_WearWoodWeapon02_KRST_NONE = 838,
       [InspectorName("UI/Characters/WEAPMisc_WearWoodWeapon03_KRST_NONE")]
       WEAPMisc_WearWoodWeapon03_KRST_NONE = 839,
       [InspectorName("UI/Characters/WEAPMisc_WearWoodWeapon04_KRST_NONE")]
       WEAPMisc_WearWoodWeapon04_KRST_NONE = 840,
       [InspectorName("UI/Characters/WEAPMisc_WearWoodWeapon05_KRST_NONE")]
       WEAPMisc_WearWoodWeapon05_KRST_NONE = 841,
       [InspectorName("UI/Characters/CLOTHMisc_WearBoots01_KRST_NONE")]
       CLOTHMisc_WearBoots01_KRST_NONE = 842,
       [InspectorName("UI/Collects/CollectItem222")]
       CollectItem222 = 843,
       [InspectorName("UI/Collects/PrizeWheelSpin2Reward")]
       PrizeWheelSpin2Reward = 844,
       [InspectorName("UI/Collects/Select_Item_05")]
       Select_Item_05 = 845,
       [InspectorName("UI/Collects/Select_Item_06")]
       Select_Item_06 = 846,
       [InspectorName("UI/HUD/SFX_UI_Appear_1")]
       SFX_UI_Appear_1 = 847,
       [InspectorName("UI/HUD/SFX_UI_Appear_2")]
       SFX_UI_Appear_2 = 848,
       [InspectorName("UI/HUD/SFX_UI_Bonus_1")]
       SFX_UI_Bonus_1 = 849,
       [InspectorName("UI/HUD/SFX_UI_Bonus_Rich_1")]
       SFX_UI_Bonus_Rich_1 = 850,
       [InspectorName("UI/HUD/SFX_UI_Button_Click_Generic_3")]
       SFX_UI_Button_Click_Generic_3 = 851,
       [InspectorName("UI/HUD/SFX_UI_Success_Achievement_Pop_1")]
       SFX_UI_Success_Achievement_Pop_1 = 852,
       [InspectorName("UI/HUD/SFX_UI_Success_Bright_Pop_1")]
       SFX_UI_Success_Bright_Pop_1 = 853,
       [InspectorName("UI/HUD/SFX_UI_Success_Bright_Rich_1")]
       SFX_UI_Success_Bright_Rich_1 = 854,
       [InspectorName("UI/HUD/Stars_complete_Level_02")]
       Stars_complete_Level_02 = 855,
       [InspectorName("UI/HUD/Stars_complete_Level_03")]
       Stars_complete_Level_03 = 856,
       [InspectorName("UI/HUD/Stars_complete_Level_04")]
       Stars_complete_Level_04 = 857,
       [InspectorName("UI/HUD/Stars_complete_Level_05")]
       Stars_complete_Level_05 = 858,
       [InspectorName("UI/HUD/STGR_Success_Calm_1")]
       STGR_Success_Calm_1 = 859,
       [InspectorName("UI/HUD/STGR_Success_Energetic_Happy")]
       STGR_Success_Energetic_Happy = 860,
       [InspectorName("UI/HUD/STGR_Success_Rich_1")]
       STGR_Success_Rich_1 = 861,
       [InspectorName("UI/HUD/Stretch_Complete_v1_wav")]
       Stretch_Complete_v1_wav = 862,
       [InspectorName("UI/HUD/Stretch_Complete_v2_wav")]
       Stretch_Complete_v2_wav = 863,
       [InspectorName("UI/HUD/Task_Complete_01")]
       Task_Complete_01 = 864,
       [InspectorName("UI/HUD/Task_Complete_02")]
       Task_Complete_02 = 865,
       [InspectorName("UI/HUD/Task_Complete_03")]
       Task_Complete_03 = 866,
       [InspectorName("UI/HUD/Task_Complete_04")]
       Task_Complete_04 = 867,
       [InspectorName("UI/HUD/Task_Complete_05")]
       Task_Complete_05 = 868,
       [InspectorName("UI/HUD/Heal_wav")]
       Heal_wav = 869,
       [InspectorName("UI/HUD/C01_wav")]
       C01_wav = 870,
       [InspectorName("UI/HUD/C011_wav")]
       C011_wav = 871,
       [InspectorName("UI/HUD/C201_wav")]
       C201_wav = 872,
       [InspectorName("UI/HUD/C0221_wav")]
       C0221_wav = 873,

}