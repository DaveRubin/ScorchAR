using System;
using UnityEngine;
using Vuforia;

public class VuforiaWrapper : ITrackableEventHandler{

    public Action<bool> onStatusChange;

    public void Init() {
        TrackableBehaviour trackableBehaviour = GameObject.Find("ImageTarget").GetComponent<TrackableBehaviour>();
        trackableBehaviour.RegisterTrackableEventHandler(this);
        if (onStatusChange != null) {
            onStatusChange(false);
        }
    }


    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
        Debug.LogFormat("Going from {0} -> {1}",previousStatus,newStatus);
        if (onStatusChange != null) {
            onStatusChange(newStatus == TrackableBehaviour.Status.DETECTED ||newStatus == TrackableBehaviour.Status.TRACKED);
        }
    }
}
