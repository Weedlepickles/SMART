using SMART.AI.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMART.AI.AITest
{
	public partial class QLearningTestForm : Form
	{
		private const float kAccSpeed = 0.5f;
		private const float kMaxSpeed = 4f;

		private Timer tickTimer = new Timer();
		private QLearningEngine mQEngineX, mQEngineY;
		private float vX = 0, vY = 0;
		private bool[] pressButtons = { false, false };
		QLearningControlPanel ControlPanelX;
		QLearningControlPanel ControlPanelY;
		private Random Rnd = new Random();

		public QLearningTestForm()
		{
			InitializeComponent();
			tickTimer.Tick += tickTimer_Tick;
			mQEngineX = QLearningEngine.Create(1, 3);
			mQEngineY = QLearningEngine.Create(1, 3);
			ControlPanelX = new QLearningControlPanel(mQEngineX);
			ControlPanelY = new QLearningControlPanel(mQEngineY);
			ControlPanelX.Text = "X Engine control panel";
			ControlPanelY.Text = "Y Engine control panel";
			ControlPanelX.Show();
			ControlPanelY.Show();
		}

		void tickTimer_Tick(object sender, EventArgs e)
		{
			Tick();
		}

		private void speedMeter_Scroll(object sender, EventArgs e)
		{
			if (speedMeter.Value == 0)
			{
				tickTimer.Stop();
				txtDelay.Text = "Update speed: Stopped";
				btnTick.Enabled = true;
				btnStop.Enabled = false;
				btnWatch.Enabled = true;
				btnTrain.Enabled = true;
			}
			else
			{
				tickTimer.Interval = speedMeter.Maximum - speedMeter.Value;
				txtDelay.Text = "Update speed: " + speedMeter.Value;
				btnTick.Enabled = false;
				tickTimer.Start();
				btnStop.Enabled = true;
				btnWatch.Enabled = false;
				btnTrain.Enabled = false;
			}
		}

		private void btnTick_Click(object sender, EventArgs e)
		{
			Tick();
		}

		private void Tick()
		{
			int[] actionX = mQEngineX.GetAction(GetState(vX, imgDisplay.AgentX, imgDisplay.TargetX, imgDisplay.Width),
				GetReward(vX, imgDisplay.AgentX, imgDisplay.TargetX, imgDisplay.Width, pressButtons[0]));
			int[] actionY = mQEngineY.GetAction(GetState(vY, imgDisplay.AgentY, imgDisplay.TargetY, imgDisplay.Height),
				GetReward(vY, imgDisplay.AgentY, imgDisplay.TargetY, imgDisplay.Height, pressButtons[1]));
			PerformActions(actionX, actionY);
			if (vX > kMaxSpeed) vX = kMaxSpeed;
			if (vX < -kMaxSpeed) vX = -kMaxSpeed;
			if (vY > kMaxSpeed) vY = kMaxSpeed;
			if (vY < -kMaxSpeed) vY = -kMaxSpeed;
			imgDisplay.AgentX += (int)Math.Round(vX);
			imgDisplay.AgentY += (int)Math.Round(vY);

			if (imgDisplay.AgentX < -20 || imgDisplay.AgentX > imgDisplay.Width + 20 ||
				imgDisplay.AgentY < -20 || imgDisplay.AgentY > imgDisplay.Height + 20)
				ResetSimulation();

			float v = Hyp((int)Math.Round(vX), (int)Math.Round(vY));
			if (v > 1.0f)
			{
				float angle = (float)Math.Atan2((double)vY, (double)vX);
				imgDisplay.Direction = angle * (180.0f / (float)Math.PI);
			}

			imgDisplay.Invalidate();
		}

		private List<int> GetState(float v, int aS, int tS, int span)
		{
			List<int> state = new List<int>();
			int mod = (v > 0) ? 1 : -1;
			float val = Math.Abs(v);
			int res = 2;
			if (val < 2) res = 1;
			if (val < 0.45) res = 0;
			state.Add(res * mod);

			int dS = tS - aS;
			mod = (dS > 0) ? 1 : -1;
			dS = Math.Abs(dS);
			res = 2;
			if (dS < 40) res = 1;
			if (dS < 10) res = 0;
			//state.Add(res * mod);

			int sP = 0;
			if (aS > 0)
			{
				sP = 1;
				if (aS > tS - 20) sP = 2;
				if (aS > tS - 10) sP = 3;
				if (aS > tS + 10) sP = 4;
				if (aS > tS + 20) sP = 5;
				if (aS > span) sP = 6;
			}
			state.Add(sP);

			return state;
		}

		private float GetReward(float vS, int aS, int tS, int span, bool buttonPressed)
		{
			if (aS < 0 || aS > span)
				return -1;
			float dS = Math.Abs(tS - aS);
			float reward = 0.0f;
			//if (dS <= 40f)
			//    reward += (1.0f - (dS / 40.0f))/ 2.0f;
			if (dS <= 10f)
				reward += (1.0f - (dS / 10.0f));
			//else if (Math.Abs(vS) < 0.45)
			//    reward = -0.1f;
			//if (buttonPressed) reward *= 0.5f;
			return reward;
		}

		private void PerformActions(int[] actionX, int[] actionY)
		{
			float xAcc = 0;
			float yAcc = 0;
			switch (actionY[0])
			{
				case 0:
					yAcc = -kAccSpeed;
					panelUp.BackColor = Color.Green;
					panelDown.BackColor = Color.DarkGray;
					pressButtons[1] = true;
					break;
				case 1:
					yAcc = 0;
					panelUp.BackColor = Color.DarkGray;
					panelDown.BackColor = Color.DarkGray;
					pressButtons[1] = false;
					break;
				case 2:
					yAcc = kAccSpeed;
					panelUp.BackColor = Color.DarkGray;
					panelDown.BackColor = Color.Green;
					pressButtons[1] = true;
					break;
			}
			switch (actionX[0])
			{
				case 0:
					xAcc = -kAccSpeed;
					panelLeft.BackColor = Color.Green;
					panelRight.BackColor = Color.DarkGray;
					pressButtons[0] = true;
					break;
				case 1:
					xAcc = 0;
					panelLeft.BackColor = Color.DarkGray;
					panelRight.BackColor = Color.DarkGray;
					pressButtons[0] = false;
					break;
				case 2:
					xAcc = kAccSpeed;
					panelLeft.BackColor = Color.DarkGray;
					panelRight.BackColor = Color.Green;
					pressButtons[0] = true;
					break;
			}
			vX += xAcc;
			vY += yAcc;
		}

		private float Hyp(float dx, float dy)
		{
			return (float)Math.Sqrt(dx * dx + dy * dy);
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			ResetSimulation();
			imgDisplay.Invalidate();
		}

		private void AITestForm_Load(object sender, EventArgs e)
		{
			imgDisplay.InitializeState();
			UpdateControlPanelPositions();
		}

		private void ResetSimulation()
		{
			imgDisplay.ResetAgentPos();
			vX = 0;
			vY = 0;
			mQEngineX.Reset();
			mQEngineY.Reset();
			if (checkRnd.Checked)
			{
				imgDisplay.TargetX = Rnd.Next(imgDisplay.Width);
				imgDisplay.TargetY = Rnd.Next(imgDisplay.Height);
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.Application.Exit();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.AddExtension = true;
			dialog.DefaultExt = "qet";
			dialog.Filter = "Q-Learning engine table | *.qet";
			dialog.InitialDirectory = System.Windows.Forms.Application.StartupPath + "\\AI\\Engine\\Saved QStates";
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				StreamWriter file = new StreamWriter(dialog.OpenFile());
				string name = dialog.FileName.Split('.')[0];
				string eX = name + "X.eng";
				string eY = name + "Y.eng";
				file.WriteLine(eX);
				file.WriteLine(eY);
				file.Close();
				mQEngineX.SaveState(eX);
				mQEngineY.SaveState(eY);
				MessageBox.Show("State saved!", "Save status", MessageBoxButtons.OK);
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			QLearningEngine xE = null;
			QLearningEngine yE = null;
			try
			{
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.AddExtension = true;
				dialog.DefaultExt = "qet";
				dialog.Filter = "Q-Learning engine table | *.qet";
				dialog.InitialDirectory = System.Windows.Forms.Application.StartupPath + "\\AI\\Engine\\Saved QStates";
				if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return;
				StreamReader reader = new StreamReader(dialog.OpenFile());
				xE = QLearningEngine.Create(reader.ReadLine());
				yE = QLearningEngine.Create(reader.ReadLine());
				reader.Close();
			}
			catch
			{
				MessageBox.Show("Saved files are corrupt! Unable to load.", "Load status", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			mQEngineX = xE;
			mQEngineY = yE;
			ControlPanelX.SetEngine(mQEngineX);
			ControlPanelY.SetEngine(mQEngineY);
			ResetSimulation();
			MessageBox.Show("State loaded!", "Load status", MessageBoxButtons.OK);
		}

		private void UpdateControlPanelPositions()
		{
			ControlPanelY.Left = this.Right;
			ControlPanelY.Top = this.Top;
			ControlPanelX.Left = this.Right;
			ControlPanelX.Top = ControlPanelY.Bottom;
		}

		private void AITestForm_LocationChanged(object sender, EventArgs e)
		{
			UpdateControlPanelPositions();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			speedMeter.Value = 0;
			tickTimer.Stop();
			txtDelay.Text = "Update speed: Stopped";
			btnTick.Enabled = true;
			btnStop.Enabled = false;
			btnWatch.Enabled = true;
			btnTrain.Enabled = true;
		}

		private void btnWatch_Click(object sender, EventArgs e)
		{
			mQEngineX.LearningRate = 0;
			mQEngineY.LearningRate = 0;
			ControlPanelX.ReadFromEngine();
			ControlPanelY.ReadFromEngine();
			speedMeter.Value = speedMeter.Maximum;
			tickTimer.Interval = speedMeter.Maximum - speedMeter.Value + 1;
			txtDelay.Text = "Update speed: " + speedMeter.Value;
			btnTick.Enabled = false;
			tickTimer.Start();
			btnStop.Enabled = true;
			btnWatch.Enabled = false;
			btnTrain.Enabled = false;
		}

		private void btnTrain_Click(object sender, EventArgs e)
		{
			mQEngineX.LearningRate = 0.9f;
			mQEngineY.LearningRate = 0.9f;
			ControlPanelX.ReadFromEngine();
			ControlPanelY.ReadFromEngine();
			speedMeter.Value = speedMeter.Maximum;
			tickTimer.Interval = speedMeter.Maximum - speedMeter.Value + 1;
			txtDelay.Text = "Update speed: " + speedMeter.Value;
			btnTick.Enabled = false;
			tickTimer.Start();
			btnStop.Enabled = true;
			btnWatch.Enabled = false;
			btnTrain.Enabled = false;
		}
	}
}
