using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.Global
{
    public class ObjectPool : MonoBehaviour
    {
        [Tooltip("The actual object pool")]
        public List<GameObject> objects;

        [Tooltip("Prefabs to randomly instantiate from when the entire pool is active")]
        public List<GameObject> prefabs;

        /// <summary>
        /// Get an object from the pool, and "spawn" it at the specified position and rotation. 
        /// If the pool is fully in use, Instantiate a random prefab.
        /// </summary>
        /// <param name="position">World Space Position of the object</param>
        /// <param name="rotation">World Space Rotation of the object</param>
        /// <returns></returns>
        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            return Spawn(position, rotation, transform);
        }

        /// <summary>
        ///Get an object from the pool, and "spawn" it at the specified position and rotation. 
        ///If the pool is fully in use, Instantiate a random prefab.
        /// </summary>
        /// <param name="position">World Space Position of the object</param>
        /// <param name="rotation">World Space Rotation of the object</param>
        /// <param name="parent">Parent of the object</param>
        /// <returns></returns>
        public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject obj;

            //Grab the first inactive object from the object pool, and set its position and rotation.
            for (int i = 0; i < objects.Count; i++)
            {
                obj = objects[i];
                if (obj.activeSelf) { continue; }

                obj.transform.parent = parent;
                //obj.GetComponent<RectTransform>().SetParent(parent, true);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }

            //If no prefab list is supplied, return null and throw a warning
            if(prefabs.Count <= 0) 
            {
                Debug.LogWarning("Attempted to instantiate an object without prefabs being provided. Returned null instead."); 
                return null; 
            }

            //If no inactive object is available, instantiate a new one randomly from the prefab list and add it to the object pool
            obj = Instantiate(Utility.Pick(prefabs), position, rotation, parent);
            objects.Add(obj);
            return obj;
        }

        /// <summary>
        ///Get a random object from the pool, and "spawn" it at the specified position and rotation. 
        ///If the pool is fully in use, Instantiate a random prefab.
        /// </summary>
        /// <param name="position">World Space Position of the object</param>
        /// <param name="rotation">World Space Rotation of the object</param>
        /// <returns></returns>
        public GameObject SpawnRandom(Vector3 position, Quaternion rotation)
        {
            return SpawnRandom(position, rotation, transform);
        }

        /// <summary>
        ///Get a random object from the pool, and "spawn" it at the specified position and rotation. 
        ///If the pool is fully in use, Instantiate a random prefab.
        /// </summary>
        /// <param name="position">World Space Position of the object</param>
        /// <param name="rotation">World Space Rotation of the object</param>
        /// <param name="parent">Parent of the object</param>
        /// <returns></returns>
        public GameObject SpawnRandom(Vector3 position, Quaternion rotation, Transform parent)
        {
            //Randomize the object pool first, then just Get as normal

            Utility.FisherYates(ref objects);
            return Spawn(position, rotation, parent);
        }

        /// <summary>
        /// "Destroys" a GameObject and returns it to the pool
        /// </summary>
        /// <param name="target">The object to be stored</param>
        public void Store(GameObject target)
        {
            target.SetActive(false);
        }
    }
}
