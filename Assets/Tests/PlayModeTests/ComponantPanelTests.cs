using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Tests
{
    public class ComponantPanelTests : MonoBehaviour
    {
        [UnityTest]
        public IEnumerator EnsureThatAnObjectIsSpawned()
        {
            GameObject gameObject = Instantiate(Resources.Load<GameObject>("Prefabs/GameController"));
            GameObject cam = Instantiate(Resources.Load<GameObject>("Prefabs/aOutlineCamera"));
            yield return null;
            GameObject componant1 = GameObject.Find("Component1");
            ComponentPanel script = componant1.GetComponent<ComponentPanel>();
            Vector2 vec = new Vector2(componant1.transform.position.x, componant1.transform.position.y);

            PointerEventData ped = new PointerEventData(EventSystem.current);
            ped.position = vec;

            script.OnPointerDown(new PointerEventData(EventSystem.current));
            yield return null;
            bool result = GameObject.Find("Balloon Variant(Clone)");
            Destroy(gameObject);
            Destroy(cam);
            Destroy(GameObject.Find("Balloon Variant(Clone)"));


            Assert.IsTrue(result);
        }
    }
}