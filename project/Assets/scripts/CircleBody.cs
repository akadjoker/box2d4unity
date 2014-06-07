using UnityEngine;
using System.Collections;
using Box2D;

public class CircleBody : MonoBehaviour {


    public BodyType type;
    public Vector2 linearVelocity;
    public float angularVelocity ;
    public float linearDamping ;
    public float angularDamping;
    public bool fixedRotation ;

    /// The friction coefficient, usually in the range [0,1].
    public float friction = 0.2f;
    public float elasticity = 0.99f;
    public float mass = 1f;

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
    void Start()
    {

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr == null)
        {
            print("sprite render is null");
        }





        width = spr.sprite.rect.width / 100f * transform.localScale.x;
        height = spr.sprite.rect.height / 100f * transform.localScale.y;



        rad = (width / 2 + height / 2) / 2;

        if (uworld != null)
        {
            Vector3 ps = transform.position;
            BodyDef my_body = new BodyDef();
          //  my_body.position.Set(ps.x, ps.y);
            my_body.type = type;
            CircleShape shape = new CircleShape();
            shape._radius = rad;
            FixtureDef fd = new FixtureDef();
            fd.friction = friction;
            fd.restitution = elasticity;
            fd.density = mass;

            fd.shape = shape;
            body = uworld.world.CreateBody(my_body);
            body.SetUserData(this);
            body.CreateFixture(fd);
            body.SetTransform(new Vector2(ps.x, ps.y), MathUtils.DegreeToRadian(transform.rotation.eulerAngles.z));

        }

    }

    void Update()
    {



        if (body != null)
        {

            body.SetAngularVelocity(angularVelocity);
            body.SetLinearVelocity(linearVelocity);
            body.SetFixedRotation(fixedRotation);


            angle = body.GetAngle() * Mathf.Rad2Deg;
            position.x = body.GetPosition().x;
            position.y = body.GetPosition().y;
            position.z = 0.0f;
        }

        Vector3 p = Camera.main.ScreenToWorldPoint(position);
        p.z = 0.0f;
        transform.position = position;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
