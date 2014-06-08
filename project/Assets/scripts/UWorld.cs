using UnityEngine;
using System.Collections;
using Box2D;
[AddComponentMenu("Box2DUnity/Physics World Component")]
public class UWorld : MonoBehaviour {

    protected static UWorld instance;

    private static DebugDraw _debugDraw = new DebugDraw();
    public  World world;
    public  int velocityIterations=8;
    public  int positionIterations=3;
    public  int world_scale = 30;
    public  int k_maxContactPoints = 148;
    public Vector2 gravity=new Vector2(0.0f, -9.8f);
    public  bool ready = false;
    public GUIText mousexoord;
    private Vector2 mouse;
    public GameObject box;
    public GameObject roda;
    protected int pwait = 2;

    internal Body _groundBody;
    internal AABB _worldAABB;
 //   internal ContactPoint[] _points = new ContactPoint[k_maxContactPoints];
  //  internal int _pointCount;
  //  internal DestructionListener _destructionListener = new DestructionListener();


     Body _bomb;
     MouseJoint _mouseJoint;
     Vector2 _bombSpawnPoint;
     bool _bombSpawning;
     Vector2 _mouseWorld;


     

    void Awake()
    {
    	instance = this;

        world = new World(gravity, true);
        mouse = new Vector2(0, 0);
        world.DebugDraw = _debugDraw;





        uint flags = 0;
        flags += (uint)DebugDrawFlags.Shape;
        flags += (uint)DebugDrawFlags.Joint;
        //flags +=  (uint)DebugDrawFlags.AABB;
        flags += (uint)DebugDrawFlags.Pair;

        flags += (uint)DebugDrawFlags.CenterOfMass;
        _debugDraw.Flags = (DebugDrawFlags)flags;

        _bombSpawning = false;



        BodyDef bodyDef = new BodyDef();
        _groundBody = world.CreateBody(bodyDef);






        create_box(436 / 2, 20, 430, 10, false);
        create_box(436 / 2, 290, 430, 10, false);

        create_box(20, 327 / 2, 10, 330, false);
        create_box(400, 327 / 2, 10, 330, false);



        print("box 2d is ready");
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



    public  void MouseDown(Vector2 p)
    {
        _mouseWorld = p;

        if (_mouseJoint != null)
        {
            return;
        }

        // Make a small box.
        AABB aabb;
        Vector2 d = new Vector2(0.001f, 0.001f);
        aabb.lowerBound = p - d;
        aabb.upperBound = p + d;

        Fixture _fixture = null;

        // Query the world for overlapping shapes.
        world.QueryAABB(
            (fixtureProxy) =>
            {
                var fixture = fixtureProxy.fixture;
                Body body = fixture.GetBody();
                if (body.GetType() == BodyType.Dynamic)
                {
                    bool inside = fixture.TestPoint(p);
                    if (inside)
                    {
                        _fixture = fixture;

                        // We are done, terminate the query.
                        return false;
                    }
                }

                // Continue the query.
                return true;
            }, ref aabb);

        if (_fixture != null)
        {
            Body body = _fixture.GetBody();
            MouseJointDef md = new MouseJointDef();
            md.bodyA = _groundBody;
            md.bodyB = body;
            md.target = p;
            md.maxForce = 1000.0f * body.GetMass();
            _mouseJoint = (MouseJoint)world.CreateJoint(md);
            body.SetAwake(true);
        }

    }

    public  void MouseUp(Vector2 p)
    {
        if (_mouseJoint != null)
        {
            world.DestroyJoint(_mouseJoint);
            _mouseJoint = null;
        }

        if (_bombSpawning)
        {
            CompleteBombSpawn(p);
        }
    }

    public void MouseMove(Vector2 p)
    {
        _mouseWorld = p;

        if (_mouseJoint != null)
        {
            _mouseJoint.SetTarget(p);
        }
    }

    public void LaunchBomb(Vector2 position, Vector2 velocity)
    {
        if (_bomb != null)
        {
            world.DestroyBody(_bomb);
            _bomb = null;
        }

        BodyDef bd = new BodyDef();
        bd.type = BodyType.Dynamic;
        bd.position = position;

        bd.bullet = true;
        _bomb = world.CreateBody(bd);
        _bomb.SetLinearVelocity(velocity);

        CircleShape circle = new CircleShape();
        circle._radius = 0.3f;

        FixtureDef fd = new FixtureDef();
        fd.shape = circle;
        fd.density = 20.0f;
        fd.restitution = 0.9f;

        Vector2 minV = position - new Vector2(0.3f, 0.3f);
        Vector2 maxV = position + new Vector2(0.3f, 0.3f);

        AABB aabb;
        aabb.lowerBound = minV;
        aabb.upperBound = maxV;

        _bomb.CreateFixture(fd);
    }

    public void SpawnBomb(Vector2 worldPt)
    {
        _bombSpawnPoint = worldPt;
        _bombSpawning = true;
    }

    public void CompleteBombSpawn(Vector2 p)
    {
        if (_bombSpawning == false)
        {
            return;
        }

        float multiplier = 30.0f;
        Vector2 vel = _bombSpawnPoint - p;
        vel *= multiplier;
        LaunchBomb(_bombSpawnPoint, vel);
        _bombSpawning = false;
    }





    void UpdateBox2D()
    {
       world.Step(Time.fixedDeltaTime, velocityIterations, positionIterations);
       world.ClearForces();

       

 
    }

    void OnPostRender()
    {
        if (_mouseJoint != null)
        {
            Vector2 p1 = _mouseJoint.GetAnchorB();
            Vector2 p2 = _mouseJoint.GetTarget();

            _debugDraw.DrawPoint(p1, 0.5f, new Color(0.0f, 1.0f, 0.0f));
            _debugDraw.DrawPoint(p1, 0.5f, new Color(0.0f, 1.0f, 0.0f));
            _debugDraw.DrawSegment(p1, p2, new Color(0.8f, 0.8f, 0.8f));
        }
        if (_bombSpawning)
        {
            _debugDraw.DrawPoint(_bombSpawnPoint, 0.5f, new Color(0.0f, 0.0f, 1.0f));
            _debugDraw.DrawSegment(_mouseWorld, _bombSpawnPoint, new Color(0.8f, 0.8f, 0.8f));
        }
    }

    void FixedUpdate()
    {
        UpdateBox2D();  
    }


    public  void MouseDrag()
    {

        
        // mouse press
        if (Input.GetMouseButtonDown(0) && _mouseJoint == null)
        {

            MouseDown(mouse);
           
        }
        // mouse release
        if (Input.GetMouseButtonUp(0))
        {
            this.MouseUp(mouse);
        }

        // mouse move
        if (_mouseJoint != null)
        {
            this.MouseMove(mouse);
        }
    }
    void Update()
    {
      

      //  Vector3 p = Input.mousePosition;
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.x = p.x;
        mouse.y = p.y;
        p.z=0.0f;


        MouseDrag();


        if (Input.GetMouseButtonDown(1))
        {
            SpawnBomb(mouse);

        }
        if (Input.GetMouseButtonUp(1))
        {
            CompleteBombSpawn(mouse);

        }


     //   if (Input.GetMouseButtonUp(1))
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {

            Instantiate(box, p, Quaternion.identity);
        }
 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Instantiate(roda, p, Quaternion.identity);

        }

        mousexoord.text = mouse.ToString();


       

    }

    public static World PhysicsWorld
    {
        get
        {
            return instance.world;
        }
    }


     void OnDestroy()
    {

        if(_mouseJoint!=null) world.DestroyJoint(_mouseJoint);
        if (_groundBody!=null) world.DestroyBody(_groundBody);
            


        print("clean box2d");
    }
}
