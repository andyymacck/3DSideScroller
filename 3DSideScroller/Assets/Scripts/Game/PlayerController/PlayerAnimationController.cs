using UnityEngine;

namespace SideScroller
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] Animator m_animator;
        [SerializeField] PlayerController m_playerController;

        private const string STATE_IDLE_TRIGGER = "Idle";
        private const string STATE_RUN_TRIGGER = "Run";
        private const string STATE_JUMP_TRIGGER = "Jump";

        private const string BLEND_PARAM = "Blend";

        void Start()
        {
            m_playerController.OnplayerStateChangedEvent += AnimationStateChange;
        }


        void Update()
        {

        }

        private void OnDestroy()
        {
            m_playerController.OnplayerStateChangedEvent -= AnimationStateChange;
        }

        private void AnimationStateChange(PlayerStates state)
        {
            if (state == PlayerStates.Idle)
            {
                SetBlendParam(0.5f);
                SetIdle();
            }

            if (state == PlayerStates.RunForward)
            {
                SetBlendParam(1f);
                SetIdle();
            }

            if (state == PlayerStates.RunBackwards)
            {
                SetBlendParam(0f);
                SetIdle();
            }

            if (state == PlayerStates.Jump)
            {
                SetJump();
            }
        }

        private void SetBlendParam(float blend)
        {
            m_animator.SetFloat(BLEND_PARAM, blend);
        }

        private void SetIdle()
        {
            m_animator.SetTrigger(STATE_IDLE_TRIGGER);
        }

        private void SetRun()
        {
            m_animator.SetTrigger(STATE_RUN_TRIGGER);
        }

        private void SetJump()
        {
            m_animator.SetTrigger(STATE_JUMP_TRIGGER);
        }
    }
}
