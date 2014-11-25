using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
	class Program
	{
		static Application Window = new Application();

		[STAThread]
		static void Main(string[] args)
		{
			Window.Title = "S.M.A.R.T.";
			Window.Run(60, 60);
		}
	}
}
