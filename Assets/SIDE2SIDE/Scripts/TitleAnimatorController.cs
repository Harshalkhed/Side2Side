using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimatorController : MonoBehaviour
{

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CallTitleAppearAnim()
    {
        animator.Play("title-appear");
    }


    public void CallTitleDisappearAnim()
    {
        animator.Play("title-disappear");
    }


}
