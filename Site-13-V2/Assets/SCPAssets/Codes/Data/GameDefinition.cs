using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Data
{
    [Serializable]
    public class GameDefinition
    {
        public List<MissionCollection> MissionCollections=new List<MissionCollection>();
        public Dictionary<string,RefSprite> Sprites;
        public Dictionary<string,RefTexture2D> Textures;
    }
    [Serializable]
    public class RefSprite
    {
        public string Name;
        public string Path;
        public WorkMode WorkMode;
        public Sprite LoadedSprite;
    }
    [Serializable]
    public class RefTexture2D
    {
        public string Name;
        public string Path;
        public WorkMode WorkMode;
        public Texture2D LoadedTexture;
    }


}
