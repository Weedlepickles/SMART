using Jitter;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
    class CowSkeleton : Skeleton
    {
        

        public CowSkeleton(string name, Vector3 position, SMARTWorld world, string fileName)
            : base(name, position, world, fileName)
		{

        }

        public void Update(TimeSpan deltaTime)
        {
            base.Update(deltaTime);

            // Do stuff
        }
    }
}
