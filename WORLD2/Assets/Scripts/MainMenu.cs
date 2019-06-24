using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : CameraBaseFSM
{


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        camera.GetComponent<manager>().SetMenu(true);
        camera.GetComponent<manager>().SetCarDrivable(false); 
        camera.GetComponent<manager>().SetScore(false);
        CamPosOffset = new Vector3(-8.63f, 2.753f, 9.48f);
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
        camera.GetComponent<manager>().SetMenu(false);
        camera.GetComponent<manager>().SetCarDrivable(true);
        camera.GetComponent<manager>().SetScore(true);
    }
}
