using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public string currentState;

    public void ChangeAnimationsState(Animator anim, string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        anim.Play(newState);

        currentState = newState;
    }
}
