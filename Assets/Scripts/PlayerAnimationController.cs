using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private static readonly int GOLF_IDLE_HASH = Animator.StringToHash("golfIdleAnimation");
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private IEnumerator Start()
    {
        _animator.Play(GOLF_IDLE_HASH, 0, 0f);
        yield return new WaitForSeconds(5);
     

    }


}
