using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Site13Kernel.Data;
using System.Threading.Tasks;
using Site13Kernel.Core;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace Site13Kernel.UEFI
{
    public class DefinitionLoader : UEFIBase
    {
        public List<MissionCollection> missionCollections= new List<MissionCollection>();
        public List<RefSprite> sprites= new List<RefSprite>();
        public List<RefTexture2D> textures= new List<RefTexture2D>();
        public override void Init()
        {
            StartCoroutine(LoadResources());
        }
        IEnumerator LoadResources()
        {
            Dictionary<string,RefSprite> _sprites=new Dictionary<string, RefSprite>();
            Dictionary<string,RefTexture2D> _textures=new Dictionary<string, RefTexture2D>();
            foreach (var item in sprites)
            {
                switch (item.WorkMode)
                {
                    case WorkMode.Internal:
                        {
                            if (item.LoadedSprite == null)
                            {
                                item.LoadedSprite = Resources.Load<Sprite>(item.Path);
                                Debug.Log("Loaded");
                            }
                        }
                        break;
                    case WorkMode.ExternalFile:
                        {
                            var uri="file://"+Path.Combine(Application.persistentDataPath,item.Path);
                            yield return LoadSprite(uri, item);
                        }
                        break;
                    case WorkMode.ExternalServer:
                        {
                            yield return LoadSprite(item.Path, item);
                        }
                        break;
                    default:
                        break;
                }
                _sprites.Add(item.Name, item);
            }
            foreach (var item in textures)
            {
                switch (item.WorkMode)
                {
                    case WorkMode.Internal:
                        {
                            if (item.LoadedTexture == null)
                            {
                                item.LoadedTexture = Resources.Load<Texture2D>(item.Path);
                                Debug.Log("Loaded");
                            }
                        }
                        break;
                    case WorkMode.ExternalFile:
                        {
                            var uri="file://"+Path.Combine(Application.persistentDataPath,item.Path);
                            yield return LoadTexture(uri, item);
                        }
                        break;
                    case WorkMode.ExternalServer:
                        {
                            yield return LoadTexture(item.Path, item);
                        }
                        break;
                    default:
                        break;
                }
                _textures.Add(item.Name, item);
            }
            GameRuntime.CurrentGlobals.CurrentGameDef.Sprites = _sprites;

            GameRuntime.CurrentGlobals.CurrentGameDef.Textures = _textures;
        }
        IEnumerator LoadSprite(string uri, RefSprite refSprite)
        {
            using (UnityWebRequest __UWR = UnityWebRequestTexture.GetTexture(uri))
            {
                yield return __UWR.SendWebRequest();

                if (__UWR.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(__UWR.error);
                }
                else
                {
                    var texture = DownloadHandlerTexture.GetContent(__UWR);
                    refSprite.LoadedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
                }
            }
        }
        IEnumerator LoadTexture(string uri, RefTexture2D refTexture)
        {
            using (UnityWebRequest __UWR = UnityWebRequestTexture.GetTexture(uri))
            {
                yield return __UWR.SendWebRequest();

                if (__UWR.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(__UWR.error);
                }
                else
                {
                    var texture = DownloadHandlerTexture.GetContent(__UWR);
                    refTexture.LoadedTexture = texture;
                }
            }
        }
        public override async Task Run()
        {
            await Task.Run(() =>
            {
                GameRuntime.CurrentGlobals.CurrentGameDef.MissionCollections = missionCollections;
            });
        }
    }
}
