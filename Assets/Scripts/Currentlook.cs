using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currentlook : MonoBehaviour
{
    public Camera Maincamera;
    public float rayDistance = 10000f;
    public Barchart barchart; 

   
    void Start()
    {
        if (Maincamera == null)
        {
            Debug.LogError("Maincamera is not assigned. Please assign a Camera object to the Maincamera field.");
            return;
        }

        if (barchart == null)
        {
            Debug.LogError("Barchart component is missing. Please assign a Barchart object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Maincamera == null || barchart == null) return;

        Ray ray = new Ray(Maincamera.transform.position, Maincamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            PlayerPoint playerPoint = hitObject.GetComponent<PlayerPoint>();

            if (playerPoint != null)
            {
                barchart.SetPlayer(hitObject);
                barchart.SetActive(true);
            }
            else
            {
                barchart.SetActive(false);
            }
        }
        else
        {
            barchart.SetActive(false);
        }
    }
}