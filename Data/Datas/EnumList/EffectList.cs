using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectList
{
	None = -1,
	
         [InspectorName("AttackSkill/SA_MultiSlash")]
         SA_MultiSlash = 0,
         [InspectorName("Slash/Combo1_Left_Circle")]
         Combo1_Left_Circle = 1,
         [InspectorName("Slash/Combo1_Left_Half")]
         Combo1_Left_Half = 2,
         [InspectorName("Slash/Combo1_Right_Circle")]
         Combo1_Right_Circle = 3,
         [InspectorName("Slash/Combo1_Right_Half")]
         Combo1_Right_Half = 4,
         [InspectorName("Slash/Combo2_Left")]
         Combo2_Left = 5,
         [InspectorName("Slash/Combo2_Right")]
         Combo2_Right = 6,
         [InspectorName("Slash/FireSlash_Left")]
         FireSlash_Left = 7,
         [InspectorName("Slash/FireSlash_Right")]
         FireSlash_Right = 8,
         [InspectorName("Slash/Eff1_Crack")]
         Eff1_Crack = 9,
         [InspectorName("Slash/Eff3_Crack")]
         Eff3_Crack = 10,
         [InspectorName("Slash/Eff9_Crack")]
         Eff9_Crack = 11,
         [InspectorName("Casting/BloodExplosion_HandCasting")]
         BloodExplosion_HandCasting = 12,
         [InspectorName("Collision/BloodExplosion_Collision")]
         BloodExplosion_Collision = 13,
         [InspectorName("Magic/DashEffect")]
         DashEffect = 14,
         [InspectorName("Magic/DemonImpact")]
         DemonImpact = 15,
         [InspectorName("Magic/FireBallCollision")]
         FireBallCollision = 16,
         [InspectorName("Magic/IceExplosion")]
         IceExplosion = 17,
         [InspectorName("Magic/Lightningstrike2")]
         Lightningstrike2 = 18,
         [InspectorName("Magic/Lightningstrike")]
         Lightningstrike = 19,
         [InspectorName("Projectile/MeteorRock")]
         MeteorRock = 20,
         [InspectorName("Collision/Soul_Collision")]
         Soul_Collision = 21,
         [InspectorName("Projectile/Soul_Projectile")]
         Soul_Projectile = 22,
         [InspectorName("Explosion/Temporaryexplosion")]
         Temporaryexplosion = 23,
         [InspectorName("Casting/Timecast")]
         Timecast = 24,
         [InspectorName("Explosion/CFXRElectricExplosion")]
         CFXRElectricExplosion = 25,
         [InspectorName("Explosion/CFXRExplosion1")]
         CFXRExplosion1 = 26,
         [InspectorName("Explosion/CFXRExplosion2Bigger")]
         CFXRExplosion2Bigger = 27,
         [InspectorName("Explosion/CFXRExplosion2")]
         CFXRExplosion2 = 28,
         [InspectorName("Explosion/CFXRExplosion3")]
         CFXRExplosion3 = 29,
         [InspectorName("Explosion/CFXRExplosion3Bigger")]
         CFXRExplosion3Bigger = 30,
         [InspectorName("Explosion/CFXRExplosionSmoke1")]
         CFXRExplosionSmoke1 = 31,
         [InspectorName("Hit/CFXRHitB_Blue")]
         CFXRHitB_Blue = 32,
         [InspectorName("Hit/CFXRHitC3D")]
         CFXRHitC3D = 33,
         [InspectorName("Hit/CFXRImpactContrast")]
         CFXRImpactContrast = 34,
         [InspectorName("Hit/CFXRImpactContrast_Blue")]
         CFXRImpactContrast_Blue = 35,
         [InspectorName("Hit/CFXRSlash_Blue")]
         CFXRSlash_Blue = 36,
         [InspectorName("Hit/CFXRHitB3D_Blue")]
         CFXRHitB3D_Blue = 37,
         [InspectorName("Hit/CFXRHitA_Red")]
         CFXRHitA_Red = 38,
         [InspectorName("Hit/CFXRBigImpactHDR")]
         CFXRBigImpactHDR = 39,
         [InspectorName("Hit/CFXRSparksImpact")]
         CFXRSparksImpact = 40,
         [InspectorName("Hit/CFXRImpactGlowingHDR_Blue")]
         CFXRImpactGlowingHDR_Blue = 41,
         [InspectorName("Hit/CFXRHitSparksHDR")]
         CFXRHitSparksHDR = 42,
         [InspectorName("Hit/CFXRHitD3D_Yellow")]
         CFXRHitD3D_Yellow = 43,
         [InspectorName("Hit/CFXRHitC")]
         CFXRHitC = 44,
         [InspectorName("Projectile/_1_Acid_Projectile")]
         _1_Acid_Projectile = 45,
         [InspectorName("Projectile/_2_Arrow_Projectile")]
         _2_Arrow_Projectile = 46,
         [InspectorName("Projectile/_3_BlackHole_Projectile")]
         _3_BlackHole_Projectile = 47,
         [InspectorName("Projectile/_4_Blue_Projectile")]
         _4_Blue_Projectile = 48,
         [InspectorName("Projectile/_5_Bullet_Projectile")]
         _5_Bullet_Projectile = 49,
         [InspectorName("Projectile/_6_Fire_Projectile")]
         _6_Fire_Projectile = 50,
         [InspectorName("Projectile/_7_Green_Projectile")]
         _7_Green_Projectile = 51,
         [InspectorName("Projectile/_8_Huricara_Projectile")]
         _8_Huricara_Projectile = 52,
         [InspectorName("Projectile/_9_Magic_Projectile")]
         _9_Magic_Projectile = 53,
         [InspectorName("Projectile/_10_Star1_Projectile")]
         _10_Star1_Projectile = 54,
         [InspectorName("Projectile/_11_Star2_Projectile")]
         _11_Star2_Projectile = 55,
         [InspectorName("Projectile/_12_Thunder_Projectile")]
         _12_Thunder_Projectile = 56,
         [InspectorName("Projectile/_13_Torpedo_Projectile")]
         _13_Torpedo_Projectile = 57,
         [InspectorName("Projectile/_14_Triangle_Projectile")]
         _14_Triangle_Projectile = 58,
         [InspectorName("Projectile/_15_Water_Projectile")]
         _15_Water_Projectile = 59,
         [InspectorName("Projectile/_16_BlackFire_Projectile")]
         _16_BlackFire_Projectile = 60,
         [InspectorName("Projectile/_17_BlueDiamond_Projectile")]
         _17_BlueDiamond_Projectile = 61,
         [InspectorName("Projectile/_18_BlueFire_Projectile")]
         _18_BlueFire_Projectile = 62,
         [InspectorName("Projectile/_19_BlueLaser_Projectile")]
         _19_BlueLaser_Projectile = 63,
         [InspectorName("Projectile/_20_BlueRapid_Projectile")]
         _20_BlueRapid_Projectile = 64,
         [InspectorName("Projectile/_21_CircleBomb_Projectile")]
         _21_CircleBomb_Projectile = 65,
         [InspectorName("Projectile/_22_Cube_Projectile")]
         _22_Cube_Projectile = 66,
         [InspectorName("Projectile/_23_CuteStar_Projectile")]
         _23_CuteStar_Projectile = 67,
         [InspectorName("Projectile/_24_Dagger_Projectile")]
         _24_Dagger_Projectile = 68,
         [InspectorName("Projectile/_25_Electro_Projectile")]
         _25_Electro_Projectile = 69,
         [InspectorName("Projectile/_26_Fire_Projectile")]
         _26_Fire_Projectile = 70,
         [InspectorName("Projectile/_27_GreenExplosion_Projectile")]
         _27_GreenExplosion_Projectile = 71,
         [InspectorName("Projectile/_28_Heart_Projectile")]
         _28_Heart_Projectile = 72,
         [InspectorName("Projectile/_29_NatureArrow_Projectile")]
         _29_NatureArrow_Projectile = 73,
         [InspectorName("Projectile/_30_NovaOrange_Projectile")]
         _30_NovaOrange_Projectile = 74,
         [InspectorName("Projectile/_31_NovaViolet_Projectile")]
         _31_NovaViolet_Projectile = 75,
         [InspectorName("Projectile/_32_OrangeArrow_Projectile")]
         _32_OrangeArrow_Projectile = 76,
         [InspectorName("Projectile/_33_OrangeExplosion_Projectile")]
         _33_OrangeExplosion_Projectile = 77,
         [InspectorName("Projectile/_34_Pink_Projectile")]
         _34_Pink_Projectile = 78,
         [InspectorName("Projectile/_35_PinkArrow_Projectile")]
         _35_PinkArrow_Projectile = 79,
         [InspectorName("Projectile/_36_PinkCrystal_Projectile")]
         _36_PinkCrystal_Projectile = 80,
         [InspectorName("Projectile/_37_Red_Projectile")]
         _37_Red_Projectile = 81,
         [InspectorName("Projectile/_38_RedArrow_Projectile")]
         _38_RedArrow_Projectile = 82,
         [InspectorName("Projectile/_39_RedLaser_Projectile")]
         _39_RedLaser_Projectile = 83,
         [InspectorName("Projectile/_40_Slime_Projectile")]
         _40_Slime_Projectile = 84,
         [InspectorName("Projectile/_41_Water_Projectile")]
         _41_Water_Projectile = 85,
         [InspectorName("Projectile/_42_YellowArrow_Projectile")]
         _42_YellowArrow_Projectile = 86,
         [InspectorName("Hit/_1_Acid_Hit")]
         _1_Acid_Hit = 87,
         [InspectorName("Hit/_2_Arrow_Hit")]
         _2_Arrow_Hit = 88,
         [InspectorName("Hit/_3_BlackHole_Hit")]
         _3_BlackHole_Hit = 89,
         [InspectorName("Hit/_4_Blue_Hit")]
         _4_Blue_Hit = 90,
         [InspectorName("Hit/_5_Bullet_Hit")]
         _5_Bullet_Hit = 91,
         [InspectorName("Hit/_6_Fire_Hit")]
         _6_Fire_Hit = 92,
         [InspectorName("Hit/_7_Green_Hit")]
         _7_Green_Hit = 93,
         [InspectorName("Hit/_8_Huricara_Hit")]
         _8_Huricara_Hit = 94,
         [InspectorName("Hit/_9_Magic_Hit")]
         _9_Magic_Hit = 95,
         [InspectorName("Hit/_10_Star1_Hit")]
         _10_Star1_Hit = 96,
         [InspectorName("Hit/_11_Star2_Hit")]
         _11_Star2_Hit = 97,
         [InspectorName("Hit/_12_Thunder_Hit")]
         _12_Thunder_Hit = 98,
         [InspectorName("Hit/_13_Torpedo_Hit")]
         _13_Torpedo_Hit = 99,
         [InspectorName("Hit/_14_Triangle_Hit")]
         _14_Triangle_Hit = 100,
         [InspectorName("Hit/_15_Water_Hit")]
         _15_Water_Hit = 101,
         [InspectorName("Hit/_16_BlackFire_Hit")]
         _16_BlackFire_Hit = 102,
         [InspectorName("Hit/_17_BlueDiamond_Hit")]
         _17_BlueDiamond_Hit = 103,
         [InspectorName("Hit/_18_BlueFire_Hit")]
         _18_BlueFire_Hit = 104,
         [InspectorName("Hit/_19_BlueLaser_Hit")]
         _19_BlueLaser_Hit = 105,
         [InspectorName("Hit/_20_BlueRapid_Hit")]
         _20_BlueRapid_Hit = 106,
         [InspectorName("Hit/_21_CircleBomb_Hit")]
         _21_CircleBomb_Hit = 107,
         [InspectorName("Hit/_22_Cube_Hit")]
         _22_Cube_Hit = 108,
         [InspectorName("Hit/_23_CuteStar_Hit")]
         _23_CuteStar_Hit = 109,
         [InspectorName("Hit/_24_Dagger_Hit")]
         _24_Dagger_Hit = 110,
         [InspectorName("Hit/_25_Electro_Hit")]
         _25_Electro_Hit = 111,
         [InspectorName("Hit/_26_Fire_Hit")]
         _26_Fire_Hit = 112,
         [InspectorName("Hit/_27_GreenExplosion_Hit")]
         _27_GreenExplosion_Hit = 113,
         [InspectorName("Hit/_28_Heart_Hit")]
         _28_Heart_Hit = 114,
         [InspectorName("Hit/_29_NatureArrow_Hit")]
         _29_NatureArrow_Hit = 115,
         [InspectorName("Hit/_30_NovaOrange_Hit")]
         _30_NovaOrange_Hit = 116,
         [InspectorName("Hit/_31_NovaViolet_Hit")]
         _31_NovaViolet_Hit = 117,
         [InspectorName("Hit/_32_OrangeArrow_Hit")]
         _32_OrangeArrow_Hit = 118,
         [InspectorName("Hit/_33_OrangeExplosion_Hit")]
         _33_OrangeExplosion_Hit = 119,
         [InspectorName("Hit/_34_Pink_Hit")]
         _34_Pink_Hit = 120,
         [InspectorName("Hit/_35_PinkArrow_Hit")]
         _35_PinkArrow_Hit = 121,
         [InspectorName("Hit/_36_PinkCrystal_Hit")]
         _36_PinkCrystal_Hit = 122,
         [InspectorName("Hit/_37_Red_Hit")]
         _37_Red_Hit = 123,
         [InspectorName("Hit/_38_RedArrow_Hit")]
         _38_RedArrow_Hit = 124,
         [InspectorName("Hit/_39_RedLaser_Hit")]
         _39_RedLaser_Hit = 125,
         [InspectorName("Hit/_40_Slime_Hit")]
         _40_Slime_Hit = 126,
         [InspectorName("Hit/_41_Water_Hit")]
         _41_Water_Hit = 127,
         [InspectorName("Hit/_42_YellowArrow_Hit")]
         _42_YellowArrow_Hit = 128,
         [InspectorName("Flash/_1_Acid_Flash")]
         _1_Acid_Flash = 129,
         [InspectorName("Flash/_2_Arrow_Flash")]
         _2_Arrow_Flash = 130,
         [InspectorName("Flash/_3_BlackHole_Flash")]
         _3_BlackHole_Flash = 131,
         [InspectorName("Flash/_4_Blue_Flash")]
         _4_Blue_Flash = 132,
         [InspectorName("Flash/_5_Bullet_Flash")]
         _5_Bullet_Flash = 133,
         [InspectorName("Flash/_6_Fire_Flash")]
         _6_Fire_Flash = 134,
         [InspectorName("Flash/_7_Green_Flash")]
         _7_Green_Flash = 135,
         [InspectorName("Flash/_8_Huricara_Flash")]
         _8_Huricara_Flash = 136,
         [InspectorName("Flash/_9_Magic_Flash")]
         _9_Magic_Flash = 137,
         [InspectorName("Flash/_10_Star1_Flash")]
         _10_Star1_Flash = 138,
         [InspectorName("Flash/_11_Star2_Flash")]
         _11_Star2_Flash = 139,
         [InspectorName("Flash/_12_Thunder_Flash")]
         _12_Thunder_Flash = 140,
         [InspectorName("Flash/_13_Torpedo_Flash")]
         _13_Torpedo_Flash = 141,
         [InspectorName("Flash/_14_Triangle_Flash")]
         _14_Triangle_Flash = 142,
         [InspectorName("Flash/_15_Water_Flash")]
         _15_Water_Flash = 143,
         [InspectorName("Flash/_16_BlackFire_Flash")]
         _16_BlackFire_Flash = 144,
         [InspectorName("Flash/_17_BlueDiamond_Flash")]
         _17_BlueDiamond_Flash = 145,
         [InspectorName("Flash/_18_BlueFire_Flash")]
         _18_BlueFire_Flash = 146,
         [InspectorName("Flash/_19_BlueLaser_Flash")]
         _19_BlueLaser_Flash = 147,
         [InspectorName("Flash/_20_BlueRapid_Flash")]
         _20_BlueRapid_Flash = 148,
         [InspectorName("Flash/_21_CircleBomb_Flash")]
         _21_CircleBomb_Flash = 149,
         [InspectorName("Flash/_22_Cube_Flash")]
         _22_Cube_Flash = 150,
         [InspectorName("Flash/_23_CuteStar_Flash")]
         _23_CuteStar_Flash = 151,
         [InspectorName("Flash/_25_Electro_Flash")]
         _25_Electro_Flash = 152,
         [InspectorName("Flash/_26_Fire_Flash")]
         _26_Fire_Flash = 153,
         [InspectorName("Flash/_27_GreenExplosion_Flash")]
         _27_GreenExplosion_Flash = 154,
         [InspectorName("Flash/_28_Heart_Flash")]
         _28_Heart_Flash = 155,
         [InspectorName("Flash/_29_NatureArrow_Flash")]
         _29_NatureArrow_Flash = 156,
         [InspectorName("Flash/_30_NovaOrange_Flash")]
         _30_NovaOrange_Flash = 157,
         [InspectorName("Flash/_31_NovaViolet_Flash")]
         _31_NovaViolet_Flash = 158,
         [InspectorName("Flash/_32_OrangeArrow_Flash")]
         _32_OrangeArrow_Flash = 159,
         [InspectorName("Flash/_33_OrangeExplosion_Flash")]
         _33_OrangeExplosion_Flash = 160,
         [InspectorName("Flash/_34_Pink_Flash")]
         _34_Pink_Flash = 161,
         [InspectorName("Flash/_36_PinkCrystal_Flash")]
         _36_PinkCrystal_Flash = 162,
         [InspectorName("Flash/_37_Red_Flash")]
         _37_Red_Flash = 163,
         [InspectorName("Flash/_39_RedLaser_Flash")]
         _39_RedLaser_Flash = 164,
         [InspectorName("Flash/_40_Slime_Flash")]
         _40_Slime_Flash = 165,
         [InspectorName("Flash/_41_Water_Flash")]
         _41_Water_Flash = 166,
         [InspectorName("Flash/_42_YellowArrow_Flash")]
         _42_YellowArrow_Flash = 167,
         [InspectorName("Etc/DashPortal")]
         DashPortal = 168,
         [InspectorName("Etc/FX_Blizzard_Snow_01")]
         FX_Blizzard_Snow_01 = 169,
         [InspectorName("Etc/FX_Dust_Big_01")]
         FX_Dust_Big_01 = 170,
         [InspectorName("Etc/FX_ExperienceGain_01")]
         FX_ExperienceGain_01 = 171,
         [InspectorName("Etc/FX_Fire_01")]
         FX_Fire_01 = 172,
         [InspectorName("Etc/FX_Fire_Big_01")]
         FX_Fire_Big_01 = 173,
         [InspectorName("Etc/FX_Fire_Big_02")]
         FX_Fire_Big_02 = 174,
         [InspectorName("Etc/FX_Fire_Big_03")]
         FX_Fire_Big_03 = 175,
         [InspectorName("Etc/FX_Fire_Small_01")]
         FX_Fire_Small_01 = 176,
         [InspectorName("Etc/FX_Fire_Small_02")]
         FX_Fire_Small_02 = 177,
         [InspectorName("Etc/FX_Fire_Small_03")]
         FX_Fire_Small_03 = 178,
         [InspectorName("Etc/FX_Fireball_01")]
         FX_Fireball_01 = 179,
         [InspectorName("Etc/FX_Fog_Big_01")]
         FX_Fog_Big_01 = 180,
         [InspectorName("Etc/FX_Fog_Small_01")]
         FX_Fog_Small_01 = 181,
         [InspectorName("Etc/FX_Heal_01")]
         FX_Heal_01 = 182,
         [InspectorName("Etc/FX_Heal_02")]
         FX_Heal_02 = 183,
         [InspectorName("Etc/FX_LevelUp_01")]
         FX_LevelUp_01 = 184,
         [InspectorName("Etc/FX_Pickup_Health_01")]
         FX_Pickup_Health_01 = 185,
         [InspectorName("Etc/FX_Portal_Round_01")]
         FX_Portal_Round_01 = 186,
         [InspectorName("Etc/FX_Portal_Sphere_01")]
         FX_Portal_Sphere_01 = 187,
         [InspectorName("Etc/FX_Portal_Thin_01")]
         FX_Portal_Thin_01 = 188,
         [InspectorName("Etc/FX_Rain_01")]
         FX_Rain_01 = 189,
         [InspectorName("Etc/FX_RainbowSparkle_01")]
         FX_RainbowSparkle_01 = 190,
         [InspectorName("Etc/FX_Ritual_Circle_01")]
         FX_Ritual_Circle_01 = 191,
         [InspectorName("Etc/FX_Snow_01")]
         FX_Snow_01 = 192,
         [InspectorName("Slash/CounterAttack1_Green")]
         CounterAttack1_Green = 193,
         [InspectorName("Slash/CounterAttack2_Red")]
         CounterAttack2_Red = 194,
         [InspectorName("Slash/SlashWave")]
         SlashWave = 195,
         [InspectorName("Collision/MeteorCollision")]
         MeteorCollision = 196,
         [InspectorName("Magic/FireRing")]
         FireRing = 197,
         [InspectorName("Magic/HolyRing")]
         HolyRing = 198,
         [InspectorName("Magic/PhantomCardShow")]
         PhantomCardShow = 199,
         [InspectorName("Projectile/_43_BlueMagic_Projectile")]
         _43_BlueMagic_Projectile = 200,
         [InspectorName("Hit/_43_BlueMagic_Hit")]
         _43_BlueMagic_Hit = 201,
         [InspectorName("Slash/Slash1_1")]
         Slash1_1 = 202,
         [InspectorName("Slash/Slash1_2")]
         Slash1_2 = 203,
         [InspectorName("Slash/SlashPrick1_3")]
         SlashPrick1_3 = 204,
         [InspectorName("Slash/Slash2_1")]
         Slash2_1 = 205,
         [InspectorName("Slash/SlashDown2_2")]
         SlashDown2_2 = 206,
         [InspectorName("Slash/SlashPrick2_3")]
         SlashPrick2_3 = 207,
         [InspectorName("Slash/SlashDown3_1")]
         SlashDown3_1 = 208,
         [InspectorName("Slash/Slash3_2")]
         Slash3_2 = 209,
         [InspectorName("Slash/SlashPrick3_3")]
         SlashPrick3_3 = 210,
         [InspectorName("Slash/Slash4")]
         Slash4 = 211,
         [InspectorName("Slash/Slash5_1")]
         Slash5_1 = 212,
         [InspectorName("Slash/SlashDown5_2")]
         SlashDown5_2 = 213,
         [InspectorName("Slash/Slash6_1")]
         Slash6_1 = 214,
         [InspectorName("Slash/SlashDown6_2")]
         SlashDown6_2 = 215,
         [InspectorName("Slash/Slash7")]
         Slash7 = 216,
         [InspectorName("Slash/Slash8")]
         Slash8 = 217,
         [InspectorName("Slash/SlashPrick_1")]
         SlashPrick_1 = 218,
         [InspectorName("Slash/SlashPrick_2")]
         SlashPrick_2 = 219,
         [InspectorName("Hit/Slash1_Hit")]
         Slash1_Hit = 220,
         [InspectorName("Hit/Slash2_Hit")]
         Slash2_Hit = 221,
         [InspectorName("Hit/Slash3_Hit")]
         Slash3_Hit = 222,
         [InspectorName("Hit/Slash4_Hit")]
         Slash4_Hit = 223,
         [InspectorName("Hit/Slash5_1_Hit")]
         Slash5_1_Hit = 224,
         [InspectorName("Hit/Slash6_Hit")]
         Slash6_Hit = 225,
         [InspectorName("Hit/Slash7_Hit")]
         Slash7_Hit = 226,
         [InspectorName("Magic/Spatialsection")]
         Spatialsection = 227,
         [InspectorName("Projectile/PurpleFire_Projectile")]
         PurpleFire_Projectile = 228,
         [InspectorName("Hit/PurpleFire_Hit")]
         PurpleFire_Hit = 229,
         [InspectorName("Collision/ExplosionFire_Collision")]
         ExplosionFire_Collision = 230,
         [InspectorName("Collision/ExplosionAcid")]
         ExplosionAcid = 231,
         [InspectorName("Slash/Normal_Slash_Black")]
         Normal_Slash_Black = 232,
         [InspectorName("Slash/Claws1_Blue")]
         Claws1_Blue = 233,
         [InspectorName("Slash/Claws2_Red")]
         Claws2_Red = 234,
         [InspectorName("Slash/ClawOne_White_Slash")]
         ClawOne_White_Slash = 235,
         [InspectorName("Slash/GroundImpact")]
         GroundImpact = 236,
         [InspectorName("Explosion/GroundExplosion")]
         GroundExplosion = 237,
         [InspectorName("Slash/Claws3_Green")]
         Claws3_Green = 238,
         [InspectorName("Explosion/GroundExplosion_Green")]
         GroundExplosion_Green = 239,
         [InspectorName("Hit/BlackArrow_Hit")]
         BlackArrow_Hit = 240,
         [InspectorName("Projectile/BlackArrow_Projectile")]
         BlackArrow_Projectile = 241,
         [InspectorName("Hit/Black_Hit")]
         Black_Hit = 242,
         [InspectorName("Etc/ExcuteBeforeSkill")]
         ExcuteBeforeSkill = 243,
         [InspectorName("Etc/ExcuteBeforeSkill_Type1")]
         ExcuteBeforeSkill_Type1 = 244,
         [InspectorName("Etc/ExcuteBeforeSkill_Type2")]
         ExcuteBeforeSkill_Type2 = 245,
         [InspectorName("Etc/ExcuteBeforeSkill_Type2_1")]
         ExcuteBeforeSkill_Type2_1 = 246,
         [InspectorName("Etc/ExcuteBeforeSkill_Red_Type2_1")]
         ExcuteBeforeSkill_Red_Type2_1 = 247,
         [InspectorName("Slash/ClawOne_Black_Slash")]
         ClawOne_Black_Slash = 248,
         [InspectorName("Slash/ClawOne_Blue_Slash")]
         ClawOne_Blue_Slash = 249,
         [InspectorName("Slash/ClawOne_BlueGreen_Slash")]
         ClawOne_BlueGreen_Slash = 250,
         [InspectorName("Slash/ClawOne_Brown_Slash")]
         ClawOne_Brown_Slash = 251,
         [InspectorName("Slash/ClawOne_Gray_Slash")]
         ClawOne_Gray_Slash = 252,
         [InspectorName("Slash/ClawOne_Green_Slash")]
         ClawOne_Green_Slash = 253,
         [InspectorName("Slash/ClawOne_LightGreen_Slash")]
         ClawOne_LightGreen_Slash = 254,
         [InspectorName("Slash/ClawOne_Orange_Slash")]
         ClawOne_Orange_Slash = 255,
         [InspectorName("Slash/ClawOne_Purple_Slash")]
         ClawOne_Purple_Slash = 256,
         [InspectorName("Slash/ClawOne_Red_Slash")]
         ClawOne_Red_Slash = 257,
         [InspectorName("Slash/ClawOne_Yellow_Slash")]
         ClawOne_Yellow_Slash = 258,
         [InspectorName("Explosion/GroundExplosion_Blue")]
         GroundExplosion_Blue = 259,
         [InspectorName("Etc/FX_Heal_Immediate")]
         FX_Heal_Immediate = 260,
         [InspectorName("Explosion/TwoSideRockExplosion")]
         TwoSideRockExplosion = 261,
         [InspectorName("Explosion/GroundExplosion_Brown")]
         GroundExplosion_Brown = 262,
         [InspectorName("Casting/FX_Ritual_Circle_02")]
         FX_Ritual_Circle_02 = 263,
         [InspectorName("Etc/CollectSpark2")]
         CollectSpark2 = 264,
         [InspectorName("Casting/SingleCastingType1_Black")]
         SingleCastingType1_Black = 265,
         [InspectorName("Casting/SingleCastingType1_White")]
         SingleCastingType1_White = 266,
         [InspectorName("Casting/SingleCastingType2_White")]
         SingleCastingType2_White = 267,
         [InspectorName("Casting/DoubleCastingType1_Black")]
         DoubleCastingType1_Black = 268,
         [InspectorName("Casting/DoubleCastingType1_Red")]
         DoubleCastingType1_Red = 269,
         [InspectorName("Casting/DoubleCastingType2_BlackBig")]
         DoubleCastingType2_BlackBig = 270,
         [InspectorName("Casting/TripleCastingType1")]
         TripleCastingType1 = 271,
         [InspectorName("Casting/TripleCastingType2_FastCollectSpark")]
         TripleCastingType2_FastCollectSpark = 272,
         [InspectorName("Casting/TripleCastingType2_NormalCollectSpark")]
         TripleCastingType2_NormalCollectSpark = 273,
         [InspectorName("Casting/CollectSpark")]
         CollectSpark = 274,
         [InspectorName("Casting/Darkness_Long")]
         Darkness_Long = 275,
         [InspectorName("Casting/Darkness_Normal")]
         Darkness_Normal = 276,
         [InspectorName("Casting/Darkness_Short")]
         Darkness_Short = 277,
         [InspectorName("Etc/SmokeDust_Little")]
         SmokeDust_Little = 278,
         [InspectorName("Etc/SmokeDust_Normal")]
         SmokeDust_Normal = 279,
         [InspectorName("Etc/SmokeDust_Many")]
         SmokeDust_Many = 280,
         [InspectorName("Etc/SmokeDust_SuperMany")]
         SmokeDust_SuperMany = 281,
         [InspectorName("Etc/Wind_Long")]
         Wind_Long = 282,
         [InspectorName("Etc/Wind_Normal")]
         Wind_Normal = 283,
         [InspectorName("Etc/Wind_Short")]
         Wind_Short = 284,
         [InspectorName("Casting/DoubleCastingType1_Red_Fast")]
         DoubleCastingType1_Red_Fast = 285,
         [InspectorName("Etc/SpeedFastEffect")]
         SpeedFastEffect = 286,
         [InspectorName("Etc/SpeedSlowEffect")]
         SpeedSlowEffect = 287,
         [InspectorName("Etc/FX_Poison_Green_01")]
         FX_Poison_Green_01 = 288,
         [InspectorName("Etc/FX_Poison_Purple_01")]
         FX_Poison_Purple_01 = 289,
         [InspectorName("Etc/Poison_Aura_Green")]
         Poison_Aura_Green = 290,
         [InspectorName("Etc/Poison_Aura_Purple")]
         Poison_Aura_Purple = 291,
         [InspectorName("Slash/SlashWave2")]
         SlashWave2 = 292,
         [InspectorName("Slash/SlashWave3")]
         SlashWave3 = 293,
         [InspectorName("Etc/EnemyAppearEffect_Red")]
         EnemyAppearEffect_Red = 294,
         [InspectorName("Etc/EnemyAppearEffect_Black")]
         EnemyAppearEffect_Black = 295,
         [InspectorName("Etc/EnemyAppearEffect_White")]
         EnemyAppearEffect_White = 296,
         [InspectorName("Etc/LevelUpEffectUI_Image")]
         LevelUpEffectUI_Image = 297,
         [InspectorName("Buff/Buff_ATK_Increase")]
         Buff_ATK_Increase = 298,
         [InspectorName("Buff/Buff_CriticalPercent_Increase")]
         Buff_CriticalPercent_Increase = 299,
         [InspectorName("Etc/ExcuteBeforeSkill_Type2_1_Yellow")]
         ExcuteBeforeSkill_Type2_1_Yellow = 300,
         [InspectorName("Etc/Electricity1")]
         Electricity1 = 301,
         [InspectorName("Etc/Electricity2")]
         Electricity2 = 302,
         [InspectorName("Etc/GlowSpot1")]
         GlowSpot1 = 303,
         [InspectorName("Etc/GlowSpot2")]
         GlowSpot2 = 304,
         [InspectorName("Etc/GroundBlast")]
         GroundBlast = 305,
         [InspectorName("Etc/LevelUp_2")]
         LevelUp_2 = 306,
         [InspectorName("Etc/GlowSpot2_Disable")]
         GlowSpot2_Disable = 307,
         [InspectorName("Etc/CounterAir")]
         CounterAir = 308,
         [InspectorName("Etc/CounterParticle")]
         CounterParticle = 309,
         [InspectorName("Etc/CounterParticle_Full")]
         CounterParticle_Full = 310,
         [InspectorName("Etc/CounterParryParticle_Full")]
         CounterParryParticle_Full = 311,
         [InspectorName("Etc/CounterParticle2_Full")]
         CounterParticle2_Full = 312,
         [InspectorName("Etc/CounterParryParticle2_Full")]
         CounterParryParticle2_Full = 313,
         [InspectorName("Etc/DashPortal_Light")]
         DashPortal_Light = 314,

}