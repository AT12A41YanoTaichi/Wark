using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�v���C���[�̈ړ��A��]�A�A�j���[�V��������

public class MovementPlayer : MonoBehaviour
{
	
	//�J������]�p
	float lsh;
	float lsv;
	//���s�ړ��p
	float rsh;
	float rsv;
	//�n��p�̈ړ����x
	Vector3 Speed = Vector3.zero;
	//�n��p�̉�]
	Vector3 rot = Vector3.zero;

	private Vector3 move;
	private Vector3 moveForward;

	[Header("�n��ł̈ړ����x")] 
	[SerializeField] private float PlayerSpeed;
	[Header("�n��ł̉�]���x")]
	[SerializeField] private float PlayerRotatinSpeed;
	[Header("�n��ł̃W�����v��")]
	[SerializeField] private float jumpPower;
	[Header("�󒆂ł̈ړ����x")]
	[SerializeField] private float FlySpeed;
	[Header("�󒆂ł̉�]���x")]
	[SerializeField] private float FlyRotate;
	[Header("FlyTimeUi�̎擾")]
	[SerializeField] private FlyTimeUi FlyUi;
	[Header("Fly�o����ő厞��")]
	[SerializeField] public float FlyMaxTime;
	[Header("�U�������蔻��I�u�W�F�N�g")]
	[SerializeField] private GameObject AttackObject;
	[Header("Animator�擾")]
	[SerializeField] Animator animator;
	AnimatorStateInfo animeInfo;
	private Collider AttackCollider;
	private Attack attack;
	private float FlyTime;
	public Transform Camera;
	private bool isJump;
	private Rigidbody rb;
	private float AttackTime;
	private bool StatAttack = false;
	private bool Attack2;
	private bool Attack3;
	public bool isAttack = false;
	public bool isBlock = false;
	//���Z�b�g�p�̉�]���ۑ�
	Vector3 worldAngle;
	[Header("���̋O�Ղ��擾")]
	[SerializeField] private TrailRenderer trailRenderer;

	[Header("SaveManager�̎擾")]
	[SerializeField] private GameObject savemanager;

	private bool hasProcessed = false;

	public enum PlayerState
	{
		Ground,
	}
	 public PlayerState platyerState;

	void Start()
	{
		
		trailRenderer.emitting = false;
		platyerState = PlayerState.Ground;
		rb = this.GetComponent<Rigidbody>();
		isJump = false;
		FlyTime = FlyMaxTime;
		attack = AttackObject.GetComponent<Attack>();
		AttackCollider = AttackObject.GetComponent<Collider>();
		OffColliderAttack();
	}

    [System.Obsolete]
    void Update()
	{
		if(!hasProcessed)
        {
			transform.position = savemanager.GetComponent<SaveManager>().GetPlayerPos();
			hasProcessed = true;
		}
		
		//���݂̃A�j���[�V������Ԃ��擾
		animeInfo = animator.GetCurrentAnimatorStateInfo(0);

		if (Input.GetKeyDown("joystick button 4"))
		{
			isBlock = true;
			animator.SetBool("Block", true);
		}

		//�K�[�h������
		if(!isBlock)
        {
			//�K�[�h������Ȃ�
			PlayerSpeed = 3.0f;
			GrandMove();
			GraundRotaition();
			GroundAttack();
		}
		else
        {
			//�K�[�h��
			PlayerSpeed = 1.5f;
			if (animeInfo.nameHash == Animator.StringToHash("StartBlock"))
            {
				animator.SetFloat("BlockTime", animeInfo.normalizedTime);
			}

			lsh = Input.GetAxis("L_Stick_H");
			lsv = Input.GetAxis("L_Stick_V");
			GrandMove();
			BlockRotation();
			if (Input.GetKeyUp("joystick button 4"))
			{
				isBlock = false;
				animator.SetBool("Block", false);
				animator.SetBool("Run", false);
			}
		}
		

		//A�{�^������
		if (Input.GetKeyDown("joystick button 0"))
		{
			//����A�j���[�V������������
			if (animator.GetBool("Run"))
			{
				//���[�����O�A�j���[�V�����Ɉڍs
				animator.SetBool("Roll", true);
			}
			else
			{
				//�o�b�N�X�e�b�v�A�j���[�V�����Ɉڍs
				animator.SetBool("BackStep", true);
			}

		}
		if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Roll"))
		{
			if (animeInfo.normalizedTime > 0.8f)
			{
				animator.SetBool("Roll", false);	
			}

		}
		else if (animeInfo.nameHash == Animator.StringToHash("Base Layer.BackStep"))
		{
			if (animeInfo.normalizedTime > 0.8f)
			{
				animator.SetBool("BackStep", false);
			}
		}
	}

    [System.Obsolete]
	//�ʒu�X�V
    void GrandMove()
    {
		if (!isAttack && !GetComponent<PlayerBase>().StopMove)
        {
			lsh = Input.GetAxis("L_Stick_H");
			lsv = Input.GetAxis("L_Stick_V");
			//�J�����̑O�������x�N�g���擾
			Vector3 cameraForward = Vector3.Scale(Camera.transform.forward,new Vector3(1,0,1)).normalized;
			if (lsh < 0.0f)
			{
				move.z = PlayerSpeed * Time.deltaTime;
				SetMoveAnime(lsh,false);
			}
			else if (lsh > 0.0f)
			{
				move.z = -PlayerSpeed * Time.deltaTime;
				SetMoveAnime(lsh, false);
			}
			if (lsh == 0.0f)
			{
				move.z = 0.0f;
				SetMoveAnime(0.0f, false);
			}
			if (lsv < 0.0f)
			{
				move.x = PlayerSpeed * Time.deltaTime;
				SetMoveAnime(lsv, true);
			}
			else if (lsv > 0.0f)
			{
				move.x = -PlayerSpeed * Time.deltaTime;
				SetMoveAnime(lsv, true);
			}
			if (lsv == 0.0f)
			{
				move.x = 0.0f;
				SetMoveAnime(0.0f, true);
			}
			if (lsv == 0.0f && lsh == 0.0f)
			{
				if(!isBlock)
                {
					animator.SetBool("Run", false);
				}
				else
                {
					animator.SetBool("BlockRight", false);
					animator.SetBool("BlockLeft", false);
					animator.SetBool("BlockBack", false);
					animator.SetBool("BlockUp", false);
				}

			}

			moveForward = cameraForward * move.z + Camera.transform.right * move.x;
			moveForward = moveForward.normalized;

			if (move.magnitude > 0) 
            {
				transform.position += moveForward * PlayerSpeed * move.magnitude + new Vector3(0, rb.velocity.y, 0) * Time.deltaTime;
            }
		}
		
	}

    [System.Obsolete]
	//�ړ��A�j���[�V�����̃Z�b�g
    void SetMoveAnime(float move,bool hv)
    {
		if(isBlock)
        {
			if (move != 0.0f) 
            {
				if(hv)
                {
					if (move > 0)
					{
						animator.SetBool("BlockLeft", true);
					}
					else
					{
						animator.SetBool("BlockRight", true);
					}
				}
				else
                {
					if (move > 0)
					{
						animator.SetBool("BlockBack", true);
					}
					else
					{
						animator.SetBool("BlockUp", true);
					}
				}
				
			}
			
			
		}
		else
        {
			animator.SetBool("Run", true);
			if (animator.GetBool("AttackRun"))
			{
				if (animeInfo.normalizedTime > 0.1f)
				{
					animator.SetBool("AttackRun", false);
					animator.SetBool("Attack", false);
				}
			}
		}
		

	}

	//�K�[�h���̃L�����N�^�[�̌����X�V
	private void BlockRotation()
    {
		Vector3 cameraForward = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1)).normalized;

		transform.forward = cameraForward;

	}

	//�ʏ펞�̃L�����N�^�[�̌����X�V
	private void GraundRotaition()
    {
		lsh = Input.GetAxis("L_Stick_H");
		lsv = Input.GetAxis("L_Stick_V");
		if (lsh < 0.0f)
		{
			move.z = PlayerRotatinSpeed * Time.deltaTime;
			animator.SetFloat("Speed", -1);
		}
		else if (lsh > 0.0f)
		{
			move.z = -PlayerRotatinSpeed * Time.deltaTime;
			animator.SetFloat("Speed", 1);
		}
		if (lsh == 0.0f)
		{
			move.z = 0.0f;
		}
		if (lsv < 0.0f)
		{
			move.x = PlayerRotatinSpeed * Time.deltaTime;;
		}
		else if (lsv > 0.0f)
		{
			move.x = -PlayerRotatinSpeed * Time.deltaTime;
		}
		if (lsv == 0.0f)
		{
			move.x = 0.0f;
		}
		Vector3 cameraForward = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
		moveForward = cameraForward * move.z + Camera.transform.right * move.x;
		moveForward = moveForward.normalized;
		if (move.magnitude > 0)
        {
			Quaternion targetRotation = Quaternion.LookRotation(moveForward);
			transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,Time.deltaTime* PlayerRotatinSpeed);
		}

	}
	
	//�U�������蔻���On
	private void OffColliderAttack()
	{
		AttackCollider.enabled = false;
		//AttackObject.GetComponent<MeshRenderer>().enabled = false;
	}
	//�U�������蔻���OFF
	private void OnColliderAttack()
	{
		AttackCollider.enabled = true;
		//AttackObject.GetComponent<MeshRenderer>().enabled = true;
	}

	//void OnCollisionEnter(Collision collision)
	//{
	//	if(animator.GetBool("Run"))
 //       {
	//		animator.SetBool("JumpRun", true);
	//	}

	//	animator.SetBool("Jump", false);
 //   }

    [System.Obsolete]
	//�L�[���͂ōU�����s��
    void GroundAttack()
    {
		if (Input.GetKeyDown("joystick button 5"))
		{
			if(!animator.GetBool("AttackRun"))
            {
				animator.SetBool("Attack", true);
				animator.SetBool("Attack2", false);
				animator.SetBool("Attack3", false);
				isAttack = true;
			}
			
		}
		if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Attack"))
		{
			
			rb.GetComponent<Rigidbody>();
			rb.AddForce(transform.forward * 0.4f, ForceMode.VelocityChange);

			if (!attack.GetHit())
			{
				OnColliderAttack();
			}

			if (animeInfo.normalizedTime > 0.3f)
			{
				if (Input.GetKeyDown("joystick button 5"))
					Attack2 = true;
			}
			if (animeInfo.normalizedTime > 0.45f && animeInfo.normalizedTime < 0.7f)
			{
				trailRenderer.emitting = true;
			}
			else
			{
				trailRenderer.emitting = false;
			}
			if (animeInfo.normalizedTime > 1.0f)
			{
				if (Attack2)
				{
					animator.SetBool("Attack2", true);
					isAttack = true;
					attack.SetHit(false);
				}
				else
				{
					if (animator.GetBool("Run"))
                    {
						animator.SetBool("AttackRun", true);
					}
					else
                    {
						animator.SetBool("Attack", false);
						animator.SetBool("Attack2", false);
						animator.SetBool("Attack3", false);
					}
					isAttack = false;
					OffColliderAttack();
				}

			}

		}
		if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Attack2"))
		{
			
			if (!attack.GetHit())
			{
				OnColliderAttack();
			}

			rb.GetComponent<Rigidbody>();
			rb.AddForce(transform.forward * 0.4f, ForceMode.VelocityChange);

			if (animeInfo.normalizedTime > 0.3f)
			{
				if (Input.GetKeyDown("joystick button 5"))
					Attack3 = true;
			}

			if (animeInfo.normalizedTime > 0.3f&& animeInfo.normalizedTime < 0.45f)
            {
				trailRenderer.emitting = true;

			}
			else
            {
				trailRenderer.emitting = false;
            }

			if (animeInfo.normalizedTime > 0.8f)
			{
				if (Attack3)
				{
					animator.SetBool("Attack3", true);
					attack.SetHit(false);
					isAttack = true;

				}
				else
				{
					if (animator.GetBool("Run"))
					{
						animator.SetBool("AttackRun", true);
						animator.SetBool("Attack2", false);
						animator.SetBool("Attack3", false);
					}
					else
					{
						animator.SetBool("Attack", false);
						animator.SetBool("Attack2", false);
						animator.SetBool("Attack3", false);
					}
					OffColliderAttack();
					Attack2 = false;
					isAttack = false;
				}

			}

		}
		if (animeInfo.nameHash == Animator.StringToHash("Base Layer.Attack3"))
		{
			
			if (!attack.GetHit())
			{
				OnColliderAttack();
			}

			if (animeInfo.normalizedTime > 0.0f&& animeInfo.normalizedTime < 0.4f)
            {
				rb.GetComponent<Rigidbody>();
				rb.AddForce(transform.forward * 0.5f, ForceMode.VelocityChange);

			}

			if (animeInfo.normalizedTime > 0.3f && animeInfo.normalizedTime < 0.45f)
			{
				trailRenderer.emitting = true;

			}
			else
			{
				trailRenderer.emitting = false;
			}

			if (animeInfo.normalizedTime > 0.8f)
			{
				if (animator.GetBool("Run"))
				{
					animator.SetBool("AttackRun", true);
					animator.SetBool("Attack3", false);
					animator.SetBool("Attack2", false);
				}
				else
				{
					animator.SetBool("Attack", false);
					animator.SetBool("Attack3", false);
					animator.SetBool("Attack2", false);
				}
				OffColliderAttack();
				Attack2 = false;
				Attack3 = false;
				attack.SetHit(false);
				isAttack = false;
			}
		}
	}

	//�K�[�h�����ǂ������擾
	public bool GetisGuard()
    {
		return isBlock;
    }

}
