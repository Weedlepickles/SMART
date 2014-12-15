using Jitter;
using Jitter.Collision;
using Jitter.LinearMath;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
    class SMARTWorld : World
    {
        private Renderer floorRenderer;
        
        public SMARTWorld(CollisionSystemPersistentSAP system)
            : base(system)
        {
            float ygrav = -0.0005f;
            Gravity = new JVector(0, ygrav, 0);

            SetDampingFactors(0.01f, 0.01f);
            SetInactivityThreshold(0, 0, float.MaxValue);
            floorRenderer = new Renderer(new ObjMesh(100f, 100f), new Vector4(0.1f, 0.6f, 0.1f, 1));
        }

        public bool CollidesWithGround(JVector position)
        {
            return position.Y <= 0;
        }

        public void Update(TimeSpan deltaTime)
        {
            Step((float)deltaTime.TotalMilliseconds, false); //Runs without multithreading
        }

        public void Render(Camera camera)
        {
            floorRenderer.Render(camera, Matrix4.CreateTranslation(new Vector3(-50, 0, -50)));
        }
    }
}
