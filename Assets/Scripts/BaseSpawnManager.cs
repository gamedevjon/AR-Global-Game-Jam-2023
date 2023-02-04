using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class BaseSpawnManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _raycastManager;
    List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    [SerializeField] GameObject _spawnableBasePrefab;

    Camera _arCam;
    GameObject _spawnedObject;

    private void Start()
    {
        _spawnedObject = null;
        _arCam = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        RaycastHit hit;
        Ray ray = _arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (_raycastManager.Raycast(Input.GetTouch(0).position, _hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && _spawnedObject == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag =="Spawnable")
                    {
                        _spawnedObject = hit.collider.gameObject;
                    }
                    else
                    {
                        SpawnPrefab(_hits[0].pose.position);
                    }
                }
            }
            else if (Input.GetTouch(0).phase ==  TouchPhase.Moved && _spawnedObject!= null)
                _spawnedObject.transform.position = _hits[0].pose.position;
            

            //if (Input.GetTouch(0).phase == TouchPhase.Ended)
            //    _spawnedObject = null;
        }
    }

    private void SpawnPrefab(Vector3 spawnPosistion)
    {
        _spawnedObject = Instantiate(_spawnableBasePrefab, spawnPosistion, Quaternion.identity);    
    }
}
