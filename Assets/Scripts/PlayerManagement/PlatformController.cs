using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(CharacterController))]
public class PlatformController : MonoBehaviour
{
	[Header("Controls")]
	public string XAxis = "Horizontal";
	public string YAxis = "Vertical";
	public string JumpButton = "Jump";

	[Header("Moving")]
	public float walkSpeed = 1.5f;
	public float runSpeed = 7f;
	public float gravityScale = 6.6f;

	[Header("Jumping")]
	public float jumpSpeed = 25;
	public float jumpDuration = 0.5f;
	public float jumpInterruptFactor = 100;
	public float forceCrouchVelocity = 25;
	public float forceCrouchDuration = 0.5f;

	[Header("Graphics")]
	public SkeletonAnimation skeletonAnimation;

	[Header("Animation")]
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string walkName = "Walk";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string runName = "Run";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string idleName = "Idle";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string jumpName = "Jump";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string fallName = "Fall";
	[SpineAnimation(dataField: "skeletonAnimation")]
	public string crouchName = "Crouch";

	[Header("Effects")]
	public AudioSource jumpAudioSource;
	public AudioSource footstepAudioSource;
	//public ParticleSystem landParticles;
	[SpineEvent]
	public string footstepEventName = "Stepped";
	CharacterController controller;
	Vector3 velocity = default(Vector3);
	float jumpEndTime = 0;
	bool jumpInterrupt = false;
	Vector2 input;
	bool wasGrounded = false;

	void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	void Start()
	{
		skeletonAnimation.AnimationState.Event += HandleEvent;
        skeletonAnimation.skeleton.SetAttachment("Mouth-Worried", null);
        skeletonAnimation.skeleton.SetAttachment("Mouth-Mad", null);
	}

	void HandleEvent(Spine.TrackEntry trackEntry, Spine.Event e)
	{
		if (e.Data.Name == footstepEventName)
		{
			footstepAudioSource.Stop();
			footstepAudioSource.pitch = GetRandomPitch(0.2f);
			footstepAudioSource.Play();
		}
	}

	static float GetRandomPitch(float maxOffset)
	{
		return 1f + Random.Range(-maxOffset, maxOffset);
	}

	void Update()
	{
		input.x = Input.GetAxis(XAxis);
		input.y = Input.GetAxis(YAxis);
		bool crouching = (controller.isGrounded && input.y < -0.5f);
		velocity.x = 0;
		float dt = Time.deltaTime;

		if (!crouching)
		{
			if (Input.GetButtonDown(JumpButton) && controller.isGrounded)
			{
				jumpAudioSource.Stop();
				jumpAudioSource.Play();
				velocity.y = jumpSpeed;
				jumpEndTime = Time.time + jumpDuration;
			}
			else
			{
				jumpInterrupt |= Time.time < jumpEndTime && Input.GetButtonUp(JumpButton);
			}

			if (input.x != 0)
			{
				velocity.x = Mathf.Abs(input.x) > 0.6f ? runSpeed : walkSpeed;
				velocity.x *= Mathf.Sign(input.x);
			}

			if (jumpInterrupt)
			{
				if (velocity.y > 0)
				{
					velocity.y = Mathf.MoveTowards(velocity.y, 0, dt * jumpInterruptFactor);
				}
				else
				{
					jumpInterrupt = false;
				}
			}
		}

		var gravityDeltaVelocity = Physics.gravity * gravityScale * dt;

		if (controller.isGrounded)
		{
			jumpInterrupt = false;
		}
		else
		{
			if (wasGrounded)
			{
				if (velocity.y < 0)
					velocity.y = 0;
			}
			else
			{
				velocity += gravityDeltaVelocity;
			}
		}

		wasGrounded = controller.isGrounded;

		controller.Move(velocity * dt);

		if (!wasGrounded && controller.isGrounded)
		{
			footstepAudioSource.Play();
			//landParticles.Emit((int)(velocity.y / -9f) + 2);
		}

		if (controller.isGrounded)
		{
			if (crouching)
			{
                skeletonAnimation.loop = false;
                skeletonAnimation.timeScale = 1.75f;
				skeletonAnimation.AnimationName = crouchName;
			}
			else
			{
                if (input.x == 0)
                {
                    skeletonAnimation.loop = true;
                    skeletonAnimation.timeScale = 1.0f;
                    skeletonAnimation.AnimationName = idleName;
                }
				else
                {
					skeletonAnimation.loop = true;
                    skeletonAnimation.timeScale = 1.0f;
					skeletonAnimation.AnimationName = Mathf.Abs(input.x) > 0.6f ? runName : walkName;
                }
			}
		}
		else
		{
            skeletonAnimation.loop = true;
            skeletonAnimation.timeScale = 0.5f;
			skeletonAnimation.AnimationName = velocity.y > 0 ? jumpName : fallName;
		}

		if (input.x != 0)
			skeletonAnimation.Skeleton.FlipX = input.x < 0;

	}
}