using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Telemetry 
{
    public class HintsBehabiour : MonoBehaviour
    {
        [SerializeField] private GameObject plane;

        [SerializeField] private float secondsToWait;
        private bool activeTime;

        // Start is called before the first frame update
        void Start()
        {
            plane.SetActive(true);
            StartCoroutine(Timer());
            activeTime = false;
        }


        public void ActivateHints()
        {
            plane.SetActive(true);
        }

        public void DesactivateHints()
        {
            plane.SetActive(false);
        }

        public void ChangeTimer(bool _newBool)
        {
            activeTime = _newBool;
        }
   
        IEnumerator Timer()
        {
            while (true)
            {
                if (activeTime)
                {
                    DesactivateHints();
                }
                yield return new WaitForSeconds(secondsToWait);
            }
        }
    }
}
