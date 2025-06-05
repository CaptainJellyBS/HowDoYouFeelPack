using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public class PlayerPositionBox : MonoBehaviour
    {
        public float xzSize = 10.0f;
        public float height = 10.0f;

        public Transform nw, ne, sw, se;

        // Start is called before the first frame update
        void Start()
        {
            nw.localPosition = Vector3.forward * (0.5f * xzSize);
            ne.localPosition = Vector3.right * (0.5f * xzSize);
            se.localPosition = Vector3.back * (0.5f * xzSize);
            sw.localPosition = Vector3.left * (0.5f * xzSize);
        }

        // Update is called once per frame
        void Update()
        {
            //Vector3 playerDir = Vector3.Scale(Player.Instance.transform.position - transform.position)
            if(Vector3.Dot(Player.Instance.transform.position - nw.position, nw.forward) > 0)
            {
                transform.position += transform.forward * xzSize;
            }
            if (Vector3.Dot(Player.Instance.transform.position - se.position, se.forward) < 0)
            {
                transform.position -= transform.forward * xzSize;
            }
            if(Vector3.Dot(Player.Instance.transform.position - ne.position, ne.right) > 0)
            {
                transform.position += transform.right * xzSize;
            }
            if (Vector3.Dot(Player.Instance.transform.position - sw.position, sw.right) < 0)
            {
                transform.position -= transform.right * xzSize;
            }

            float y = GameManager.Instance.currentPlayerFloor * 10.0f;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }
}
