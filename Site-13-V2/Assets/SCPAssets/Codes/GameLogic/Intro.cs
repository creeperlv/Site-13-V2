using Site13Kernel.GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Site13Kernel
{
    public class Intro : MonoBehaviour
    {
        public float Length;
        public int IntroScene;
        public int FirstScreen = 2;
        public int MainScene = 2;
        void Start()
        {

            StartCoroutine(__p());
        }
        IEnumerator __p()
        {
            yield return new WaitForSeconds(Length);
            //foreach (var item in SceneToShow)
            {
                SceneLoader.Instance.LoadScene(FirstScreen,true,false,false);

            }
            //SceneLoader.Instance.Unload(IntroScene);
            //yield return null;
            //SceneLoader.Instance.SetActive(MainScene);
        }
    }

}