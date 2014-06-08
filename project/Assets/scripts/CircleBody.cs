using UnityEngine;
using System.Collections;
using Box2D;

public class CircleBody : IBody {


    public override void createShape()
    {
        print("circle body start");
        if (world != null)
        {

            CircleShape shape = new CircleShape();
            shape._radius = rad;
            FixtureDef fd = new FixtureDef();
            fd.friction =  friction;
            fd.restitution = elasticity;
            fd.density =  mass;
            fd.shape = shape;
            body.CreateFixture(fd);
            setTrasform();
            print("circle def :" + fd.ToString());
        }
    }

   
   

}
