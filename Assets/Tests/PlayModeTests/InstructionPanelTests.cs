using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;

namespace Tests
{
    public class InstructionPanelTests
    { 

        [UnityTest]
        public IEnumerator ShowInstructionsOnFirstVisit()
        {
            Assert.AreEqual(true, OptionsData.SeenInstructions);
            yield return null;
        }

        [UnityTest]
        public IEnumerator DontShowInstructionsOnSecondVisit()
        {
            loadScene script = new loadScene();

            script.LoadAScene("Level_select");
            yield return new WaitForSeconds(0.1f);
            GameObject InstructionBox = GameObject.Find("InstructionManager");
            InstructionBox.GetComponent<InstructionHandler>().SetTransition(false);
            Assert.AreEqual(false, OptionsData.SeenInstructions);
            OptionsData.SeenInstructions = true;
            yield return null;
        }

        [UnityTest]
        public IEnumerator ExitInstructionsButton()//The Exit button is Changing the Options.Data showing that the Instructions have been shown.
        {
            loadScene script = new loadScene();

            script.LoadAScene("Level_select");
            yield return new WaitForSeconds(0.1f);

      
            GameObject quitInstructionsButton = GameObject.Find("QuitInstructionButtons");
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(quitInstructionsButton.gameObject, pointer, ExecuteEvents.pointerClickHandler);
            Assert.AreEqual(false, OptionsData.SeenInstructions);
            OptionsData.SeenInstructions = true;

            yield return null;
        }

        [UnityTest]
        public IEnumerator EnterInstructionsButton()//The Entrance button is Changing the Options.Data showing that the Instructions have been shown.
        {
            loadScene script = new loadScene();

            script.LoadAScene("Level_select");
            yield return new WaitForSeconds(0.1f);

            GameObject quitInstructionsButton = GameObject.Find("HelpButton");
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(quitInstructionsButton.gameObject, pointer, ExecuteEvents.pointerClickHandler);
            Assert.AreEqual(false, OptionsData.SeenInstructions);


            yield return null;
        }


    }
}
