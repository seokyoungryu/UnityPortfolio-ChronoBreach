using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagAndLayerDefine
{

    public class Tags
    {
        public const string Player = "Player";
        public const string Enemy = "Enemy";
        public const string Ground = "Ground";
    }

    public class LayersIndex
    {
        public const int Default = 1 << 0;
        public const int TransparentFX = 1 << 1;
        public const int IgnoreRayCast = 1 << 2;
        public const int Water = 1 << 4;
        public const int UI = 1 << 5;
        public const int Player = 1 << 6;
        public const int Enemy = 1 << 7;
        public const int Ground = 1 << 8;
        public const int StaticObject = 1 << 9;
        public const int ProtectAI = 1 << 10;
        public const int PlayableAI = 1 << 11;


    }

    public class LayersString
    {
        public const string Default = "Default";
        public const string TransparentFX = "TransparentFX";
        public const string IgnoreRayCast = "Ignore RayCast";
        public const string Water = "Water";
        public const string UI = "UI";
        public const string Player = "Player";
        public const string Enemy = "Enemy";
        public const string Ground = "Ground";
        public const string StaticObject = "StaticObject";
        public const string ProtectAI = "ProtectAI";
        public const string PlayableAI = "PlayableAI";

    }



}
