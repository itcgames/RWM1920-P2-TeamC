using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class PausePanelTests : MonoBehaviour
    {
        private GameObject gameObject;
        private GameObject manager;
        [SetUp]
        public void Setup()
        {
            gameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Canvas"));
            manager = GameObject.Find("MenuManager");
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator TestPausePanelCanTurnOn()
        {
            PauseHandler script = manager.GetComponent<PauseHandler>();
            bool previous = GameObject.Find("Pause");
            script.SwitchPausePanel();

            yield return null;
            bool result = GameObject.Find("Pause");
            Time.timeScale = 1;

            Assert.AreNotEqual(previous, result);
        }

        [UnityTest]
        public IEnumerator TestThatSceneSwitches()
        {
            loadScene script = new loadScene();

            script.LoadAScene("mainMenu");
            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(SceneManager.GetActiveScene().name, "mainMenu");
        }
    }
}