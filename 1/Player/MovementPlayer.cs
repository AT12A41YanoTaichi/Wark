using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーの移動、回転、アニメーション処理

public class MovementPlayer : MonoBehaviour
{
	
	//カメラ回転用
	float lsh;
	float lsv;
	//平行移動用
	float rsh;
	float rsv;
	//地上用の移動速度
	Vector3 Speed = Vector3.zero;
	//地上用の回転
	Vector3 rot = Vector3.zero;

	private Vector3 move;
	private Vector3 moveForward;

	[Header("地上での移動速度")] 
	[SerializeField] private float PlayerSpeed;
	[Header("地上での回転速度")]
	[SerializeField] private float PlayerRotatinSpeed;
	[Header("地上でのジャンプ力")]
	[SerializeField] private float jumpPower;
	[Header("空中での移動速度")]
	[SerializeField] private float FlySpeed;
	[Header("空中での回転速度")]
	[SerializeField] private float FlyRotate;
	[Header("FlyTimeUiの取得")]
	[SerializeField] private FlyTimeUi FlyUi;
	[Header("Fly出来る最大時間")]
	[SerializeField] public float FlyMaxTime;
	[Header("攻撃当たり判定オブジェクト")]
	[SerializeField] private GameObject AttackObject;
	[Header("Animator取得")]
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
	//リセット用の回転軸保存
	Vector3 worldAngle;
	[Header("剣の軌跡を取得")]
	[SerializeField] private TrailRenderer trailRenderer;

	[Header("SaveManagerの取得")]
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
		
		//現在のアニメーション状態を取得
		animeInfo = animator.GetCurrentAnimatorStateInfo(0);

		if (Input.GetKeyDown("joystick button 4"))
		{
			isBlock = true;
			animator.SetBool("Block", true);
		}

		//ガード中判定
		if(!isBlock)
        {
			//ガード中じゃない
			PlayerSpeed = 3.0f;
			GrandMove();
			GraundRotaition();
			GroundAttack();
		}
		else
        {
			//ガード中
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
		

		//Aボタン入力
		if (Input.GetKeyDown("joystick button 0"))
		{
			//走りアニメーション中か判定
			if (animator.GetBool("Run"))
			{
				//ローリングアニメーションに移行
				animator.SetBool("Roll", true);
			}
			else
			{
				//バックステップアニメーションに移行
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
	//位置更新
    void GrandMove()
    {
		if (!isAttack && !GetComponent<PlayerBase>().StopMove)
        {
			lsh = Input.GetAxis("L_Stick_H");
			lsv = Input.GetAxis("L_Stick_V");
			//カメラの前方方向ベクトル取得
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
	//移動アニメーションのセット
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

	//ガード時のキャラクターの向き更新
	private void BlockRotation()
    {
		Vector3 cameraForward = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1)).normalized;

		transform.forward = cameraForward;

	}

	//通常時のキャラクターの向き更新
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
	
	//攻撃当たり判定をOn
	private void OffColliderAttack()
	{
		AttackCollider.enabled = false;
		//AttackObject.GetComponent<MeshRenderer>().enabled = false;
	}
	//攻撃当たり判定をOFF
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
	//キー入力で攻撃を行う
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

	//ガード中かどうかを取得
	public bool GetisGuard()
    {
		return isBlock;
    }

}
