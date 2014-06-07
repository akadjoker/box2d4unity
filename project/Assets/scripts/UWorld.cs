using UnityEngine;
using System.Collections;
using Box2D;

public class UWorld : MonoBehaviour {


    private static DebugDraw _debugDraw = new DebugDraw();
    public  World world;
    public  float hz=60.0f;
    public float step=0.09f;//1f/60f;
    public  int velocityIterations=8;
    public  int positionIterations=3;
    public  int world_scale = 30;
    public  int k_maxContactPoints = 2048;
    public Vector2 gravity=new Vector2(0.0f, -10.0f);
    public  bool ready = false;
    public GUIText mousexoord;
    private Vector2 mouse;
    public GameObject box;
    public GameObject roda;

    void Awake()
    {



        mouse = new Vector2(0, 0);


        world = new World(gravity, true);

        world.Step(1000, velocityIterations, positionIterations);
        world.ClearForces();

        //    _destructionListener.test = this;
        //   _world.DestructionListener = _destructionListener;
        //    _world.ContactListener = this;
        world.DebugDraw = _debugDraw;




        uint flags = 0;
        flags += (uint)DebugDrawFlags.Shape;
        flags += (uint)DebugDrawFlags.Joint;
        //flags +=  (uint)DebugDrawFlags.AABB;
        flags += (uint)DebugDrawFlags.Pair;

        flags += (uint)DebugDrawFlags.CenterOfMass;
        _debugDraw.Flags = (DebugDrawFlags)flags;


        create_box(436 / 2, 20, 430, 10, false);
        create_box(436 / 2, 290, 430, 10, false);

        create_box(20, 327 / 2, 10, 330, false);
        create_box(400, 327 / 2, 10, 330, false);
        ready = true;

       
    }

	// Use this for initialization
	void Start () {

  
        
	}


  

    public  Body create_box(float px, float py, float w, float h, bool d)
    {
        BodyDef my_body = new BodyDef();
        my_body.position.Set(px / world_scale, py / world_scale);
        if (d)            my_body.type = BodyType.Dynamic;

        PolygonShape box = new PolygonShape();
        box.SetAsBox(w / 2 / world_scale, h / 2 / world_scale);
        FixtureDef fd = new FixtureDef();
        fd.shape = box;

        Body world_body = world.CreateBody(my_body);
        world_body.CreateFixture(fd);
        return world_body;
    }

 
 
    public  Body create_circle(float px, float py, float r, bool d)
    {
        BodyDef my_body = new BodyDef();
        my_body.position.Set(px / world_scale, py / world_scale);
        if (d)            my_body.type = BodyType.Dynamic;

        CircleShape shape = new CircleShape();
        shape._radius = r / world_scale;

        FixtureDef fd = new FixtureDef();
        fd.shape = shape;


        Body world_body = world.CreateBody(my_body);
        world_body.CreateFixture(fd);
        return world_body;

    }








    void FixedUpdate()
    {

        world.Step(step, velocityIterations, positionIterations);
        world.ClearForces();


    }
    void Update()
    {

      //  Vector3 p = Input.mousePosition;
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.x = p.x;
        mouse.y = p.y;
        p.z=0.0f;


        if (Input.GetMouseButtonUp(1))
        {

            Instantiate(box, p, Quaternion.identity);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Instantiate(roda, p, Quaternion.identity);

        }

        mousexoord.text = mouse.ToString();



    }

 

}
