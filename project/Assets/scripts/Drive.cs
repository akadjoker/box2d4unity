using UnityEngine;
using System.Collections;

public class Drive : MonoBehaviour {


	public float xspeed = 0f;
	public float power = 30f;
	public float friction = 0.99f;
	public float maxSpeed=700f;
	bool right = false;
	bool left = false;

	public bool useFront=true;
	public bool useBack=true;

	public GUIText label;

	public GameObject whellL;
	public GameObject whellR;

    private WheelJoint2D whellJointL;
    private WheelJoint2D whellJointR;



	private JointMotor2D motorl;
	private JointMotor2D motorr;

	//public CircleCollider2D colider;

	//private JointMotor2D motor;

	// Use this for initialization
	void Start () {


		whellJointL =(WheelJoint2D) whellL.GetComponent ("WheelJoint2D");
		whellJointR =(WheelJoint2D) whellR.GetComponent ("WheelJoint2D");

	
	}
	void FixedUpdate()
	{
		if (right) {
						xspeed += power;
				} 
		if (left) 
		{
						xspeed -= power;
				} 
	


	}
	
	void Update () {
		
		if(Input.GetKeyDown("d")){
			right = true;
		}
		if(Input.GetKeyUp("d")){
			right = false;
		}
		if(Input.GetKeyDown("a")){
			left = true;
		}
		if(Input.GetKeyUp("a")){
			left = false;
		}
		if(Input.GetKeyDown("s"))
		{
			xspeed=0;
		}

		motorr = whellJointR.motor;
		motorl = whellJointL.motor;

		
	
		
		xspeed *= friction;
		if (xspeed >= maxSpeed) 
		{
			xspeed=maxSpeed;
		}else
			if (xspeed <= -maxSpeed) 
		{
			xspeed=-maxSpeed;
		}


		motorl.maxMotorTorque=maxSpeed;
		motorr.maxMotorTorque=maxSpeed;

	if (useFront)
	{
		whellJointR.useMotor=true;
		motorr.motorSpeed= xspeed;
	} else
		{
		 whellJointR.useMotor=false;
		}


	if (useBack) {
						whellJointL.useMotor = true;
						motorl.motorSpeed = xspeed;
				} else {
			   whellJointL.useMotor=false;
				}
		

  


		 whellJointR.motor=motorr;
	     whellJointL.motor=motorl;

		label.text="Speed:"+xspeed.ToString();

		
	}
}
