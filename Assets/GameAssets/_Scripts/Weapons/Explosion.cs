using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator anim;
    private AnimationClip clip;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        clip = anim.GetCurrentAnimatorClipInfo(0)[0].clip;

        // clip.name;
    }
}
