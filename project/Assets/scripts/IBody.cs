using UnityEngine;
using System.Collections;
using Box2D;

public class IBody : MonoBehaviour {

    public BodyType type = BodyType.Dynamic;

    public float angularVelocity=0;
    public bool fixedRotation=false;

    public float friction=0.2f ;
    public float elasticity=0.1f ;
    public float mass=1.0f ;

    public Body body;
    private float angle;
    private Vector3 position;

    public float width;
    public float height;
    public float rad;

   // protected UWorld uworld;
    protected SpriteRenderer spr;
    protected World world;

    protected bool initialized = false;

    public virtual void createShape()
    {
    }
    //public  void Awake()
    public virtual void Start()
    {
        world = UWorld.PhysicsWorld;
        if (world==null) return;
        if (initialized)            return;
        initialized = true;
        

        if (world == null)
        {
            print("box2d is null");

        }

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr == null)
        {
            print("sprite render is null");

        }


        width = spr.sprite.rect.width / 100f * transform.localScale.x;
        height = spr.sprite.rect.height / 100f * transform.localScale.y;
        rad = (width / 2 + height / 2) / 2;

        BodyDef my_body = new BodyDef();
        my_body.type = type;

        body = world.CreateBody(my_body);
        body.SetUserData(this);
        //   body.SetTransform(new Vector2(ps.x, ps.y), MathUtils.DegreeToRadian(transform.rotation.eulerAngles.z));
        body.Position = new Vector2(transform.position.x, transform.position.y);
        body.Rotation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        print("body init");

        createShape();
    }



    public void setTrasform()
    {

        if (body != null)
        {
            if (elasticity <= 0.0f)
            {
                elasticity = 0.0f;
            } else
                if (elasticity >= 1.0f)
                {
                    elasticity = 1.0f;
                }
            if (elasticity <= 0.0f)
            {
                elasticity = 0.0f;
            }
            else
                if (elasticity >= 1.0f)
                {
                    elasticity = 1.0f;
                }


            body.SetAngularVelocity(angularVelocity);
            body.SetFixedRotation(fixedRotation);

            angle = MathUtils.RadianToDegree(body.GetAngle());// *Mathf.Rad2Deg;
            position.x = body.GetPosition().x;
            position.y = body.GetPosition().y;
            position.z = 0.0f;
        }


        transform.position = position;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


    }



   void Update()
   {
       setTrasform();
    
   }

   public Body PhysicsBody
   {
       get
       {
           if (!initialized)
               Start();
           return body;
       }
   }
  void OnDestroy()
   {
      if(world!=null)
        world.DestroyBody(body);
   }
}
