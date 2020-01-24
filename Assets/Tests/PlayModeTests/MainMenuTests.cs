using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class MainMenuTests
    {
        [UnityTest]
        public IEnumerator PlayButtonLoadsLevelSelect()
        {
            loadScene script = new loadScene();

            script.LoadAScene("mainMenu");
            yield return new WaitForSeconds(0.1f);

            GameObject PlayButton = GameObject.Find("PlayButton");
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(PlayButton.gameObject, pointer, ExecuteEvents.pointerClickHandler);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Level_select");
            yield return null;
        }

        [UnityTest]
        public IEnumerator BackgroundPresent()
        {
            loadScene script = new loadScene();

            script.LoadAScene("mainMenu");
            yield return new WaitForSeconds(0.1f);

            GameObject menuBackground = GameObject.Find("Menu_Background");
            Assert.AreNotEqual(menuBackground, null);
            yield return null;
        }

        [UnityTest]
        public IEnumerator MenuTransitionsToOptions()
        {
            loadScene script = new loadScene();

            script.LoadAScene("mainMenu");
            yield return new WaitForSeconds(0.1f);

            GameObject OptionsButton = GameObject.Find("OptionButton");
            float optionsButtonInitialPositionY = OptionsButton.transform.position.y;
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(OptionsButton.gameObject, pointer, ExecuteEvents.pointerClickHandler);
            yield return new WaitForSeconds(0.1f);
            Assert.IsTrue(OptionsButton.transform.position.y < optionsButtonInitialPositionY);
            yield return null;
        }

        [UnityTest]
        public IEnumerator BackgroundAnimationsBird()
        {
            loadScene script = new loadScene();

            script.LoadAScene("mainMenu");
            yield return new WaitForSeconds(0.1f);

            GameObject birdShadow = GameObject.Find("BirdShadow");
            float birdShadowY = birdShadow.transform.position.y;
            yield return new WaitForSeconds(0.1f);
            Assert.IsTrue(birdShadow.transform.position.y < birdShadowY);//The Bird is moving down and not in the same place
            yield return null;
        }

        [UnityTest]
        public IEnumerator BackgroundAnimationsTitle()
        {
            loadScene script = new loadScene();

            script.LoadAScene("mainMenu");
            yield return new WaitForSeconds(0.1f);

            GameObject titleHolder = GameObject.Find("TitleHolder");
            float titleHolderY = titleHolder.transform.position.y;
            yield return new WaitForSeconds(0.1f);
            Assert.IsTrue(titleHolder.transform.position.y > titleHolderY);//The Title is moving up and not in the same place.
            yield return null;
        }
    }
}
