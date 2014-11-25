using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMART.AI.Engine
{
    partial class QLearningControlPanel : Form
    {
        private QLearningEngine mQEngine;

        public QLearningControlPanel(QLearningEngine engine)
        {
            InitializeComponent();
            SetEngine(engine);
        }

        public void SetEngine(QLearningEngine engine)
        {
            mQEngine = engine;
            ReadFromEngine();
        }

        private void QLearningControlPanel_Load(object sender, EventArgs e)
        {
            ReadFromEngine();
        }

        public void ReadFromEngine()
        {
            barLR.Value = (int)(mQEngine.LearningRate * 100);
            barDF.Value = (int)(mQEngine.DiscountFactor * 100);
            barEF.Value = (int)(mQEngine.ExplorationFactor * 100);
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            lblDF.Text = String.Format("{0:0.00}", mQEngine.DiscountFactor);
            lblLR.Text = String.Format("{0:0.00}", mQEngine.LearningRate);
            lblEF.Text = String.Format("{0:0.00}", mQEngine.ExplorationFactor);
        }

        private void barLR_Scroll(object sender, EventArgs e)
        {
            mQEngine.LearningRate = (float)barLR.Value / 100f;
            UpdateLabels();
        }

        private void barDF_Scroll(object sender, EventArgs e)
        {
            mQEngine.DiscountFactor = (float)barDF.Value / 100f;
            UpdateLabels();
        }

        private void barEF_Scroll(object sender, EventArgs e)
        {
            mQEngine.ExplorationFactor = (float)barEF.Value / 100f;
            UpdateLabels();
        }
    }
}
