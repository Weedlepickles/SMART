using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	abstract class Muscle
	{
		float Strength = 0;

		public void UseMuscle(float strength)
		{
			if (Math.Abs(strength) > 1)
				throw new Exception("Too strong!");
			Strength = strength;
		}
	}
}
