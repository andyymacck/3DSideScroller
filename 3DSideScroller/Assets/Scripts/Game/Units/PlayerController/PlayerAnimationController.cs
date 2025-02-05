using System.Collections;
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
        private const string BLEND_PARAM = "RunBlend";

        private const float BLEND_TIME = 0.2f;

        private Coroutine m_blendCoroutine;

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

        private void SetBlendParam(float blend)
        {
            if (m_blendCoroutine != null)
            {
                StopCoroutine(m_blendCoroutine);
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

        private void SetIdle()
        {
            m_animator.SetTrigger(STATE_IDLE_TRIGGER);
        }

        private void SetJump()
        {
            m_animator.SetTrigger(STATE_JUMP_TRIGGER);
        }
    }
}