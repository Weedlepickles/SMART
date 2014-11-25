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
			//Run application
			Window.Run(60, 60);

            //Run AI test application
            //System.Windows.Forms.Application.Run(new SMART.AI.AITest.AITestForm());
		}
	}
}
