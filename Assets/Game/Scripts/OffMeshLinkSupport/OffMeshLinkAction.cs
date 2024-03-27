using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(OffMeshLink))]
public class OffMeshLinkAction : MonoBehaviour
{
    public enum Action
    {
        Climb,
        JumpWithStyle,
        JumpWithAniamtion,
        ClimbDown
    }
    public Action action;
    public bool applyRotationBlend = true;
}
