using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ARPlaceHologram : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabToPlace;

    private ARRaycastManager _raycastManager;
    private ARAnchorManager _anchorManager;

    private static readonly List<ARRaycastHit> Hits = new();

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _anchorManager = GetComponent<ARAnchorManager>();

    }

    protected void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    protected void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        var activeTouches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
        if (activeTouches.Count == 0 || activeTouches[0].phase != TouchPhase.Began)
        {
            return;
        }
        if (_raycastManager.Raycast(activeTouches[0].screenPosition, Hits, TrackableType.AllTypes))
        {
            var hitPose = Hits[0].pose;
            // Instantiate the prefab at the given position
            // Note: the object is not anchored yet!
            // Instantiate(_prefabToPlace, hitPose.position, hitPose.rotation);
            CreateAnchor(hit);
            Debug.Log($"Instantiated on: {Hits[0].hitType}");
        }

    }

    ARAnchor CreateAnchor(in ARRaycastHit hitPose)
      
    {
        ARAnchor anchor;
        var instantiatedObject = Instantiate(_prefabToPlace, hit.pose.position, hit.pose.rotation);
        // Make sure the new GameObject has an ARAnchor component
        if (!instantiatedObject.TryGetComponent<ARAnchor>(out anchor))
        {
            anchor = instantiatedObject.AddComponent<ARAnchor>();
        }
        Debug.Log($"Created regular anchor (id: {anchor.nativePtr}).");
        return anchor;
    }
}
