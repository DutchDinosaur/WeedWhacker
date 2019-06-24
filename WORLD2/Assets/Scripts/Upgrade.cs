using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : CameraBaseFSM
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CamPosOffset = new Vector3(8.72f, 1.76f, 9.97f);
        camera.GetComponent<manager>().SetCarDrivable(false);
        camera.GetComponent<manager>().SetUpgradeMenu(true);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 desiredPos = CamTracker.transform.position + CamPosOffset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(camera.transform.position, desiredPos, ref camVelocity, smoothTime);
        camera.transform.position = smoothedPosition;

        camera.transform.LookAt(CamTracker.transform);

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        camera.GetComponent<manager>().SetCarDrivable(true);
        camera.GetComponent<manager>().SetUpgradeMenu(false);
    }

}
