using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBaseFSM : StateMachineBehaviour
{
    public GameObject camera;
    public GameObject CamTracker;

    public Vector3 CamPosOffset;

    public float smoothTime = .5f;
    public Vector3 camVelocity;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CamTracker = GameObject.FindGameObjectWithTag("CamTrackPos");
        camera = animator.gameObject;

        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Gameplay"))
        //{
        //    CamPosOffset = new Vector3(-0.24f, 34.83f, -40.83f);
        //}
        //else if (animator.GetCurrentAnimatorStateInfo(0).IsName("MainMenu"))
        //{
        //    CamPosOffset = new Vector3(-8.63f, 2.753f, 9.48f);
        //}
        //else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Upgrade"))
        //{
        //    CamPosOffset = new Vector3(4.72f, 1.76f, -5.97f);
        //}
    }
}