using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanterBehavior : MonoBehaviour
{
    [SerializeField] GameObject _placablePrefab;
    [SerializeField] GameObject _ghostPrefab;

    Camera _camera;
    Ray _ray;
    RaycastHit _hit;
    GameObject _spawnedObject;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        RaycastHit hit;
        _ray = _camera.ScreenPointToRay(Input.GetTouch(0).position);

        if (Physics.Raycast(_ray, out _hit) && _hit.transform.CompareTag("PlayArea"))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && _spawnedObject == null)
            {
                if (Physics.Raycast(_ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable")
                    {
                        _spawnedObject = hit.collider.gameObject;
                    }
                    else
                    {
                        SpawnPrefab(_ghostPrefab, _hit.point);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && _spawnedObject != null)
            {
                _spawnedObject.transform.position = _hit.point;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Destroy(_spawnedObject.gameObject);
                SpawnPrefab(_placablePrefab, _hit.point);
                _spawnedObject = null;
                UIManager.Instance.DisplayPlanterButtons(true);
                this.gameObject.SetActive(false);
            }
        }
    }
    private void SpawnPrefab(GameObject prefab, Vector3 spawnPosistion)
    {
        _spawnedObject = Instantiate(prefab, spawnPosistion, Quaternion.identity);
    }
}
