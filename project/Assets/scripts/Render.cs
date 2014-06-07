/*
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using Box2D;


    public class DebugDraw   : Box2D.DebugDraw
    {
      //  public float scale = 30;

        public override void DrawPolygon(ref FixedArray8<Vector2> vertices, int count, Color color)
        {
            GL.Begin(GL.LINES);

            for (int i = 0, j = 1; i < count; i++, j++)
            {

                j = (j >= count) ? 0 : j;

                GL.Color(color);
                GL.Vertex3(vertices[i].x , vertices[i].y , 0.0f);
                GL.Color(color);
                GL.Vertex3(vertices[j].x , vertices[j].y , 0.0f);


            }



            GL.End();
        
        }

        public override void DrawSolidPolygon(ref FixedArray8<Vector2> vertices, int count, Color color)
        {
            GL.Begin(GL.LINES);
            
            for (int i = 0, j=1; i < count ; i++,j++)
            {

                j = (j >= count) ? 0 : j;

                GL.Color(color);
                GL.Vertex3(vertices[i].x , vertices[i].y , 0.0f);
                GL.Color(color);
                GL.Vertex3(vertices[j].x , vertices[j].y , 0.0f);


            }

      

            GL.End();

        



        }

        private void DrawSolidPolygon(ref FixedArray8<Vector2> vertices, int count, Color color, bool outline)
        {
            
            if (count == 2)
            {
                DrawPolygon(ref vertices, count, color);
                return;
            }

            Color colorFill = color * (outline ? 0.5f : 1.0f);
            GL.Begin(GL.LINES);

            for (int i = 0, j = 1; i < count; i++, j++)
            {

                j = (j >= count) ? 0 : j;
                GL.Color(colorFill);
                GL.Vertex3(vertices[i].x , vertices[i].y , 0.0f);
                GL.Color(colorFill);
                GL.Vertex3(vertices[j].x , vertices[j].y , 0.0f);



            }



            GL.End();
            if (outline)
            {
                DrawPolygon(ref vertices, count, color);
            }
             
        }

        public override void DrawCircle(Vector2 center, float radius, Color color)
        {
                   


            int segments = 16;
            double increment = Math.PI * 2.0 / (double)segments;
            double theta = 0.0;
            GL.Begin(GL.LINES);
            for (int i = 0; i < segments; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));
                GL.Color(color);
                GL.Vertex3(v1.x , v1.y , 0.0f);
                GL.Color(color);
                GL.Vertex3(v2.x , v2.y , 0.0f);
                theta += increment;
            }
            GL.End();
        }

        public override void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, Color color)
        {
          
            Color colorFill = color * 0.5f;


            int segments = 16;
            double increment = Math.PI * 2.0 / (double)segments;
            double theta = 0.0;
            GL.Begin(GL.LINES);
            for (int i = 0; i < segments; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));
                GL.Color(colorFill);
                GL.Vertex3(v1.x, v1.y , 0.0f);
                GL.Color(colorFill);
                GL.Vertex3(v2.x , v2.y , 0.0f);
                
                theta += increment;
            }
            GL.End();
            DrawSegment(center, center + axis * radius, color);
        }

        public override void DrawSegment(Vector2 p1, Vector2 p2, Color color)
        {


              
            GL.Begin(GL.LINES);
                GL.Color(color);
                GL.Vertex3(p1.x , p1.y, 0.0f);
                GL.Color(color);
                GL.Vertex3(p2.x , p2.y , 0.0f);
            GL.End();


        }

        public override void DrawTransform(ref Box2D.Transform xf)
        {
            float axisScale = 0.4f;
            Vector2 p1 = xf.Position;
            
            Vector2 p2 = p1 + axisScale * xf.R.col1;
            DrawSegment(p1, p2, Color.red);

          
            p2 = p1 + axisScale * xf.R.col2;
            DrawSegment(p1, p2, Color.green);

          }

        public void DrawPoint(Vector2 p, float size, Color color)
        {
            
            FixedArray8<Vector2> verts = new FixedArray8<Vector2>();
            float hs = size / 2.0f ;
            verts[0] = p + new Vector2(-hs, -hs);
            verts[1] = p + new Vector2( hs, -hs);
            verts[2] = p + new Vector2( hs,  hs);
            verts[3] = p + new Vector2(-hs,  hs);

            DrawSolidPolygon(ref verts, 4, color, true);
        }


       


        public void DrawAABB(ref AABB aabb, Color color)
        {
            FixedArray8<Vector2> verts = new FixedArray8<Vector2>();
            verts[0] = new Vector2(aabb.lowerBound.x, aabb.lowerBound.y);
            verts[1] = new Vector2(aabb.upperBound.x, aabb.lowerBound.y);
            verts[2] = new Vector2(aabb.upperBound.x, aabb.upperBound.y);
            verts[3] = new Vector2(aabb.lowerBound.x, aabb.upperBound.y);

            DrawPolygon(ref verts, 4, color);
        }


        public static int _lineCount;
        public static int _fillCount;
      

        

            public int x, y;
        
        
    }

