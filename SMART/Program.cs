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
			Window.Title = "S.M.A.R.T.";
		    Window.Run(200, 30);

            //Run Q-Learning AI test application
            //System.Windows.Forms.Application.Run(new SMART.AI.AITest.QLearningTestForm());

            //Run Neural Network AI test application
            //System.Windows.Forms.Application.Run(new SMART.AI.AITest.NNTestForm());
		}
	}
}
