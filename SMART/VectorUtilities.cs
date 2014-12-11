using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class VectorUtilities
	{
        public static List<float> ListAdd(List<float> l1, List<float> l2)
        {
            if (l1 == null || l2 == null)
                throw new ArgumentNullException();
            if (l1.Count != l2.Count)
                throw new ArgumentException("Cannot add lists with different lengths");

            List<float> result = new List<float>();
            for (int i = 0; i < l1.Count; i++)
            {
                result.Add(l1[i] + l2[i]);
            }
            return result;
        }

        public static List<float> ListSub(List<float> l1, List<float> l2)
        {
            if (l1 == null || l2 == null)
                throw new ArgumentNullException();
            if (l1.Count != l2.Count)
                throw new ArgumentException("Cannot add lists with different lengths");

            List<float> result = new List<float>();
            for (int i = 0; i < l1.Count; i++)
            {
                result.Add(l1[i] - l2[i]);
            }
            return result;
        }

        public static float ListLenSquared(List<float> list)
        {
            if (list == null)
                throw new ArgumentNullException();

            float result = 0f;
            foreach (float v in list)
            {
                result += (v * v);
            }
            return (float)result;
        }

        public static float ListLen(List<float> list)
        {
            return (float)Math.Sqrt(ListLenSquared(list));
        }

	}
}
