using System.Collections;
using UnityEngine;

namespace SideScroller
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] Animator m_animator;
        [SerializeField] PlayerController m_playerController;
        [SerializeField] Transform m_hips;

        private RotationStates m_rotationState;

        private const string STATE_IDLE_TRIGGER = "Idle";
        private const string STATE_JUMP_TRIGGER = "Jump";
        private const string STATE_PUNCH_TRIGGER = "Punch";
        private const string BLEND_PARAM = "RunBlend";
        private const string IS_GROUNDED_PARAM = "isGrounded";

        private const float BLEND_TIME = 0.2f;

        private Coroutine m_blendCoroutine;

        void Start()
        {
            m_playerController.OnplayerStateChangedEvent += AnimationStateChange;
        }

        private void OnDestroy()
        {
            m_playerController.OnplayerStateChangedEvent -= AnimationStateChange;
        }

        public void SetRotation(RotationStates rotationStates)
        {
            if (m_rotationState == rotationStates)
            {
                return;
            }

            m_rotationState = rotationStates;
            Vector3 currentRotation = m_hips.localRotation.eulerAngles;
            currentRotation.y = rotationStates == RotationStates.Right ? 90 : -90;
            m_hips.localRotation = Quaternion.Euler(currentRotation);
        }

        public void SetRotation(Vector3 direction)
        {
            transform.rotation = Quaternion.LookRotation(direction);
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
            }

            if (state == PlayerStates.RunBackwards)
            {
                SetBlendParam(0f);
            }

            if (state == PlayerStates.Jump)
            {
                SetJump();
            }
        }

        public void SetIsGrounded(bool isGrounded)
        {
            m_animator.SetBool(IS_GROUNDED_PARAM, isGrounded);
            
            if (isGrounded)
            {
                m_animator.ResetTrigger(STATE_JUMP_TRIGGER);
            }
        }

        private void SetBlendParam(float blend)
        {
            if (m_blendCoroutine != null)
            {
                StopCoroutine(m_blendCoroutine);
                m_blendCoroutine = null;
            }

            m_blendCoroutine = StartCoroutine(SmoothAnimationBlend(blend));
        }


        private IEnumerator SmoothAnimationBlend(float targetValue)
        {
            float startValue = m_animator.GetFloat(BLEND_PARAM);
            float time = 0f;
            float lerpFactor = 0f;
            float blendValue = 0f;

            while (time < BLEND_TIME)
            {
                time += Time.deltaTime;
                lerpFactor = time / BLEND_TIME;
                blendValue = Mathf.Lerp(startValue, targetValue, lerpFactor);
                m_animator.SetFloat(BLEND_PARAM, blendValue);
                yield return null;
            }

            m_animator.SetFloat(BLEND_PARAM, targetValue);
        }

        public void SetIdle()
        {
            m_animator.ResetTrigger(STATE_JUMP_TRIGGER);
            m_animator.SetTrigger(STATE_IDLE_TRIGGER);
        }

        public void TrigerPunch()
        {
            m_animator.SetTrigger(STATE_PUNCH_TRIGGER);
        }

        private void SetJump()
        {
            m_animator.ResetTrigger(STATE_IDLE_TRIGGER);
            m_animator.SetTrigger(STATE_JUMP_TRIGGER);
        }
    }
}