using System.Collections.Generic;
using UnityEngine;

public class OverworldDirectionController : MonoBehaviour
{
    public PlayerVariables variables;

    public List<AnimationClip> animations;
    public List<Vector2> directionKey;

    private Dictionary<Vector2, AnimationClip> animationDict;

    public Animator animator;

    private AnimatorOverrideController overrideController;

    private Vector2 lastDirection;

    void Start()
    {
        // Init dictionary
        animationDict = new Dictionary<Vector2, AnimationClip>();
        for (int i = 0; i < directionKey.Count; i++)
        {
            animationDict[directionKey[i]] = animations[i];
        }

        // Create override controller based on current one
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }

    void Update()
    {
        Vector2 inputDir = variables.playerFacing;


            inputDir = new Vector2(
                Mathf.Round(inputDir.x),
                Mathf.Round(inputDir.y)
            );



        if (animationDict.TryGetValue(inputDir, out AnimationClip clip))
        {
            // "Run" MUST match the original clip name in the Animator
            Debug.Log(clip);
            
            overrideController["Run"] = clip;
        }
    }
}