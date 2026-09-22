using UnityEngine;
using UnityEngine.Animations.Rigging;


public class FootIKController : MonoBehaviour
{
    public Animator animator;
    public FootIKSettings settings;

    [SerializeField] private Transform LeftFootRayOrigin;
    [SerializeField] private Transform RightFootRayOrigin;

    public Transform LeftFootTarget;
    public Transform RightFootTarget;

    public Transform LeftFootGroundCheck;
    public Transform RightFootGroundCheck;

    public TwoBoneIKConstraint LeftFootConstraint;
    public TwoBoneIKConstraint RightFootConstraint;

    private FootIKSolver leftFoot;
    private FootIKSolver rightFoot;

    [Header("Hip Adjustment")]
    [SerializeField]
    private Transform HipTarget;
    private HipIKSolver HipSolver;

    private bool wasMoving;
    private bool wasIdle;
    

    private static readonly int WalkRunHash = Animator.StringToHash("Base Layer.WalkRun");
    private void Awake()
    {



        leftFoot = new FootIKSolver(
            LeftFootRayOrigin,
            LeftFootTarget,
            settings,
            LeftFootConstraint,
            LeftFootGroundCheck);

        rightFoot = new FootIKSolver(
            RightFootRayOrigin,
            RightFootTarget,
            settings,
            RightFootConstraint,
            RightFootGroundCheck);

        HipSolver = new HipIKSolver(settings,transform,HipTarget,leftFoot,rightFoot);


    }

    private void LateUpdate()
    {
        bool isMoving = Mathf.Abs(animator.GetFloat("Speed")) > 0.05f;
        bool shouldForcePlant =
            Mathf.Abs(animator.GetFloat("Speed")) < 0.05f &&
            animator.GetBool("IsTurning") &&
            animator.GetBool("IsGrounded");

        if (shouldForcePlant)
        {
            leftFoot.Plant();
            rightFoot.Plant();
            Debug.Log("Both feet planted");
        }

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        


        if (state.shortNameHash == WalkRunHash && !wasIdle)
        {
            leftFoot.Plant();
            rightFoot.Plant();
        }

        wasMoving = isMoving;
        wasIdle = state.shortNameHash == WalkRunHash;
        HipSolver.UpdateHipHeight();

        leftFoot.UpdateFoot(transform);
        rightFoot.UpdateFoot(transform);

        
    } 




    public void LeftFootPlant()
    {
        leftFoot.Plant();
        Debug.Log("Left foot planted");
    }


    public void LeftFootLift()
    {
        leftFoot.Lift();
    }


    public void RightFootPlant()
    {
        rightFoot.Plant();
        Debug.Log("Right foot planted");
    }


    public void RightFootLift()
    {
        rightFoot.Lift();
    }

    
}