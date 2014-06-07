using System;
using System.Collections.Generic;
using UnityEngine;
using Box2D;

public class RenderPrimitives : MonoBehaviour {

    private Material lineMaterial = null;
    private  UWorld uw;
    private  Vector2 f = new Vector2();
    private  Vector2 v = new Vector2();
    private  Vector2 lv = new Vector2();
    public bool debugEditor = true;
    public bool debug=true;

    private void setColor(Color color)
    {
        Gizmos.color = color;
    }
    private void line(float x, float y, float x2, float y2)
    {
        Gizmos.DrawLine(new Vector3(x, y, 0.0f), new Vector3(x2, y2, 0.0f));

    }
     void  drawSolidCircle(Vector2 center, float radius, Vector2 axis, Color color)
    {

        float angle = 0;
        
        float angleInc = 2 * (float)Mathf.PI / 20;
        setColor(color);
        for (int i = 0; i < 20; i++, angle += angleInc)
        {
           
           v.Set((float)Math.Cos(angle) * radius + center.x, (float)Math.Sin(angle) * radius + center.y);
            if (i == 0)
            {
                 lv.Set(v.x,v.y);
                f.Set(v.x,v.y);
                continue;
            }
            line(lv.x, lv.y, v.x, v.y);
            lv.Set(v.x,v.y);
        }
      line(f.x, f.y, lv.x, lv.y);
      line(center.x, center.y,  center.x + axis.x * radius, center.y + axis.y * radius);
       
    }
      void  drawSolidPolygon(ref FixedArray8<Vector2> vertices, int vertexCount, Color color, bool closed)
     {
         setColor(color);
         lv.Set(vertices[0].x, vertices[0].y);
         f.Set(vertices[0].x, vertices[0].y);
         for (int i = 1; i < vertexCount; i++)
         {
             Vector2 v = vertices[i];
             line(lv.x, lv.y, v.x, v.y);
             lv.Set(v.x,v.y);
         }
         if (closed) line(f.x, f.y, lv.x, lv.y);
     }


      private void drawSegment(Vector2 x1, Vector2 x2, Color color)
      {
          setColor(color);
          line(x1.x, x1.y, x2.x, x2.y);
      }

    void DrawShape(Fixture fixture, Box2D.Transform xf, Color color)
    {
        switch (fixture.ShapeType)
        {
            case ShapeType.Circle:
                {
                    CircleShape circle = (CircleShape)fixture.GetShape();

                    Vector2 center = MathUtils.Multiply(ref xf, circle._p);
                    float radius = circle._radius;
                    Vector2 axis = xf.R.col1;

                    drawSolidCircle(center, radius, axis, color);
                }
                break;

            case ShapeType.Polygon:
                {
                    PolygonShape poly = (PolygonShape)fixture.GetShape();
                    int vertexCount = poly._vertexCount;
                    FixedArray8<Vector2> vertices = new FixedArray8<Vector2>();

                    for (int i = 0; i < vertexCount; ++i)
                    {
                        vertices[i] = MathUtils.Multiply(ref xf, poly._vertices[i]);
                    }

                    drawSolidPolygon(ref vertices, vertexCount, color,true);
                }
                break;

            case ShapeType.Edge:
                {
                    EdgeShape edge = (EdgeShape)fixture.GetShape();
                    Vector2 v1 = MathUtils.Multiply(ref xf, edge._vertex1);
                    Vector2 v2 = MathUtils.Multiply(ref xf, edge._vertex2);
                     drawSegment(v1, v2, color);
                }
                break;

            case ShapeType.Loop:
                {
                    LoopShape loop = (LoopShape)fixture.GetShape();
                    int count = loop._count;

                    Vector2 v1 = MathUtils.Multiply(ref xf, loop._vertices[count - 1]);
                    for (int i = 0; i < count; ++i)
                    {
                        Vector2 v2 = MathUtils.Multiply(ref xf, loop._vertices[i]);
                        drawSegment(v1, v2, color);
                        v1 = v2;
                    }
                }
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (!debugEditor) return;
        if (uw != null)
        {
            for (Body body = uw.world.GetBodyList(); body != null; body = body._next)
            {
                Box2D.Transform xf;
                body.GetTransform(out xf);
                for (Fixture f = body.GetFixtureList(); f != null; f = f.GetNext())
                {
                    DrawShape(f, xf, Color.red);
                }

            }
        }
        

    }


   void Awake()
   {
        uw = Camera.main.GetComponent<UWorld>();

	
   }
    void Start()
    {
        CreateMaterial();
    
    }


    public void Bind()
    {
        lineMaterial.SetPass(0);

    }

 

    void OnPostRender()
    {
        if (!debug) return;
        Bind();
        GL.PushMatrix();
   //     DrawLine(new Vector2(0, 0), new Vector2(480, 320), new Color(1f, 1f, 0f));
        uw.world.DrawDebugData();
        GL.PopMatrix();
    }


    public void CreateMaterial()
    {
        if (lineMaterial != null)
            return;

        lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                                    "SubShader { Pass { " +
                                    "    Blend SrcAlpha OneMinusSrcAlpha " +
                                    "    ZWrite Off Cull Off Fog { Mode Off } " +
                                    "    BindChannels {" +
                                    "      Bind \"vertex\", vertex Bind \"color\", color }" +
                                    "} } }");
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
    }
}
