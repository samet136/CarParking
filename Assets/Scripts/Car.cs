using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : MonoBehaviour
{
    public bool forward;
    bool StandPointStatus = false;
    public GameObject[] Tracks;
    float RiseValue;
    bool PlatformUpgrade;
    //

    public Transform parent;
    public GameManager manager;
    public GameObject EffectPoint;

    void Start()
    {
        //StandPoint
    }


    void Update()
    {
        if (!StandPointStatus)
        {
            transform.Translate(6f * transform.forward * Time.deltaTime);
        }
        if (forward)
        {
            transform.Translate(15f * transform.forward * Time.deltaTime);
        }
        if (PlatformUpgrade)
        {
            if (RiseValue>manager.Platform_1.transform.position.y)
            {
                manager.Platform_1.transform.position = Vector3.Lerp(manager.Platform_1.transform.position, new Vector3(manager.Platform_1.transform.position.x,
            manager.Platform_1.transform.position.y + 1.3f, manager.Platform_1.transform.position.z), .01f);
            }
            else
            {
                PlatformUpgrade = false;
            }        
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Parking"))
        {
            forward = false;
            Tracks[0].SetActive(false);
            Tracks[1].SetActive(false);
            transform.SetParent(parent);
            if (manager.MoveUp)
            {
                RiseValue = manager.Platform_1.transform.position.y + 1.3f;
                PlatformUpgrade = true;
            }

            GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;

            manager.NewCarBring();
        }

        else if (collision.gameObject.CompareTag("Car"))
        {
            manager.CollisionEffect.transform.position = EffectPoint.transform.position;
            manager.CollisionEffect.Play();
            forward = false;

            manager.Lost();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StandPoint"))
        {
            StandPointStatus = true;

        }
        else if (other.CompareTag("Diamond"))
        {
            other.gameObject.SetActive(false);
            manager.DiamondCount++;
            manager.Sounds[0].Play();
        }
        else if (other.CompareTag("CenterPost"))
        {
            manager.CollisionEffect.transform.position = EffectPoint.transform.position;
            manager.CollisionEffect.Play();
            forward = false;
            manager.Lost();
        }
    }
}
