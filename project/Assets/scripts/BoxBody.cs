using UnityEngine;
using System.Collections;
using Box2D;

public class BoxBody : IBody  {



    public override void createShape()
    {
        print("box body start");

        if (world != null)
        {

            PolygonShape shape = new PolygonShape();
            shape.SetAsBox(width / 2, height / 2);
            //    shape.SetAsBox(width / 2, height / 2, new Vector2(0, 0),  MathUtils.DegreeToRadian(rotation));//transform.rotation.eulerAngles.z));// MathUtils.RadianToDegree(48));//transform.rotation.eulerAngles.z-90));// transform.rotation.eulerAngles.z/Mathf.Deg2Rad);

            FixtureDef fd = new FixtureDef();

            fd.friction =  friction;
            fd.restitution = elasticity;
         
            fd.density =  mass;

            fd.shape = shape;

            body.CreateFixture(fd);

            setTrasform();
            print("box def :" + fd.ToString());
           // print("mass :" + mass.ToString());

        }
    }
   


}
