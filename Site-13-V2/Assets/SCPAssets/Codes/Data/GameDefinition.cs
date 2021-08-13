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
        public List<RefSprite> Sprites;
        public List<RefTexture2D> Textures;
    }
    [Serializable]
    public class RefSprite
    {
        public string Path;
        public WorkMode WorkMode;
        public Sprite LoadedSprite;
    }
    [Serializable]
    public class RefTexture2D
    {
        public string Path;
        public WorkMode WorkMode;
        public Texture2D LoadedTexture;
    }


}
