using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class CharacterData : ScriptableObject
    {
        [Header("CharacterStats")]
        public int DefaultIndex;
        public string DefaultName;
        public int DefaultBaseHeatlh;
        public int DefaultBaseMovespeed;
        public Sprite defaultSprite;
        public GameObject DefaultPrefab;
        public float DefaultBaseAcceleration;
        public float DefaultBaseMaxVelocity;
    }
}
