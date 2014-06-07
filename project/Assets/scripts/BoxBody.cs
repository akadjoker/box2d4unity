using UnityEngine;
using System.Collections;
using Box2D;

public class BoxBody : MonoBehaviour  {

    public BodyType type;
    public Vector2 linearVelocity;
    public float angularVelocity;
    public float linearDamping;
    public float angularDamping;
    public bool fixedRotation;

    public float rotation;

    /// The friction coefficient, usually in the range [0,1].
    public float friction=0.2f;
    public float elasticity = 0.0f;
    public float mass=1f;


    public Body body;
    public float angle;
    public float width;
    public float height;
    public float rad;
    public Vector3 position;
    private UWorld uworld;

    void Awake()
    {

        uworld = Camera.main.GetComponent<UWorld>();
        if (uworld == null)
        {
            print("box2d is null");

        }
             
        
    }

   

	// Use this for initialization
	void Start () 
    {
       

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr == null)
        {
            print("sprite render is null");
        }

        
    


        width = spr.sprite.rect.width   /100f * transform.localScale.x;
        height = spr.sprite.rect.height /100f * transform.localScale.y;

     
      

        print(width.ToString()+"x"+height.ToString());

    //    print("extedns:"+spr.bounds.extents.ToString());
    //    print("size:" + spr.bounds.size.ToString());
    //    print("center:" + spr.bounds.center.ToString());


        if (uworld != null)
        {
            Vector3 ps = transform.position;
            BodyDef my_body = new BodyDef();
          // my_body.position.Set(ps.x, ps.y);
            my_body.type = type;

            PolygonShape shape = new PolygonShape();
            shape.SetAsBox(width / 2, height / 2);
        //    shape.SetAsBox(width / 2, height / 2, new Vector2(0, 0),  MathUtils.DegreeToRadian(rotation));//transform.rotation.eulerAngles.z));// MathUtils.RadianToDegree(48));//transform.rotation.eulerAngles.z-90));// transform.rotation.eulerAngles.z/Mathf.Deg2Rad);


      
            FixtureDef fd = new FixtureDef();

            fd.friction=friction;
            fd.restitution=elasticity ;
            fd.density= mass;

            fd.shape = shape;

            body = uworld.world.CreateBody(my_body);
            body.SetUserData(this);
            body.CreateFixture(fd);

            body.SetTransform(new Vector2(ps.x, ps.y), MathUtils.DegreeToRadian(transform.rotation.eulerAngles.z));

            setTrasform();
        }
       
	}

    void Update()
    {

          setTrasform();

    
    }

    void setTrasform()
    {
        if (body != null)
        {
            body.SetAngularVelocity(angularVelocity);
            body.SetLinearVelocity(linearVelocity);
            body.SetFixedRotation(fixedRotation);

            angle = MathUtils.RadianToDegree(body.GetAngle());// *Mathf.Rad2Deg;
            position.x = body.GetPosition().x;
            position.y = body.GetPosition().y;
            position.z = 0.0f;
        }


        transform.position = position;
       transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}
