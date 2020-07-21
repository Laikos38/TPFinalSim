using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TPFinal.Models;
using static TPFinal.Models.Enums;

namespace TPFinal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.cmbGenerator.SelectedIndex = 0;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            #region DobleBuffered DataGridViews
            this.SuspendLayout();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.SetProperty, null,
            dgvResultsSim1, new object[] { true });
            this.ResumeLayout();

            this.SuspendLayout();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.SetProperty, null,
            dgvResultsSim2, new object[] { true });
            this.ResumeLayout();

            this.SuspendLayout();
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.SetProperty, null,
            dgvResultsSim3, new object[] { true });
            this.ResumeLayout();
            #endregion
            #region Columns colors
            this.dgvResultsSim1.Columns[3].DefaultCellStyle.BackColor = Color.SandyBrown;
            this.dgvResultsSim1.Columns[4].DefaultCellStyle.BackColor = Color.SandyBrown;
            this.dgvResultsSim1.Columns[5].DefaultCellStyle.BackColor = Color.SandyBrown;
            this.dgvResultsSim1.Columns[6].DefaultCellStyle.BackColor = Color.LightPink;
            this.dgvResultsSim1.Columns[7].DefaultCellStyle.BackColor = Color.LightPink;
            this.dgvResultsSim1.Columns[8].DefaultCellStyle.BackColor = Color.LightPink;
            this.dgvResultsSim1.Columns[9].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            this.dgvResultsSim1.Columns[10].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            this.dgvResultsSim1.Columns[11].DefaultCellStyle.BackColor = Color.LightGreen;
            this.dgvResultsSim1.Columns[12].DefaultCellStyle.BackColor = Color.LightGreen;
            this.dgvResultsSim1.Columns[13].DefaultCellStyle.BackColor = Color.LightGreen;

            this.dgvResultsSim2.Columns[3].DefaultCellStyle.BackColor = Color.SandyBrown;
            this.dgvResultsSim2.Columns[4].DefaultCellStyle.BackColor = Color.SandyBrown;
            this.dgvResultsSim2.Columns[5].DefaultCellStyle.BackColor = Color.SandyBrown;
            this.dgvResultsSim2.Columns[6].DefaultCellStyle.BackColor = Color.LightPink;
            this.dgvResultsSim2.Columns[7].DefaultCellStyle.BackColor = Color.LightPink;
            this.dgvResultsSim2.Columns[8].DefaultCellStyle.BackColor = Color.LightPink;
            this.dgvResultsSim2.Columns[9].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            this.dgvResultsSim2.Columns[10].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            this.dgvResultsSim2.Columns[11].DefaultCellStyle.BackColor = Color.LightGreen;
            this.dgvResultsSim2.Columns[12].DefaultCellStyle.BackColor = Color.LightGreen;
            this.dgvResultsSim2.Columns[13].DefaultCellStyle.BackColor = Color.LightGreen;

            this.dgvResultsSim3.Columns[3].DefaultCellStyle.BackColor = Color.SandyBrown;
            this.dgvResultsSim3.Columns[4].DefaultCellStyle.BackColor = Color.SandyBrown;
            this.dgvResultsSim3.Columns[5].DefaultCellStyle.BackColor = Color.SandyBrown;
            this.dgvResultsSim3.Columns[6].DefaultCellStyle.BackColor = Color.LightPink;
            this.dgvResultsSim3.Columns[7].DefaultCellStyle.BackColor = Color.LightPink;
            this.dgvResultsSim3.Columns[8].DefaultCellStyle.BackColor = Color.LightPink;
            this.dgvResultsSim3.Columns[9].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            this.dgvResultsSim3.Columns[10].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            this.dgvResultsSim3.Columns[11].DefaultCellStyle.BackColor = Color.LightGreen;
            this.dgvResultsSim3.Columns[12].DefaultCellStyle.BackColor = Color.LightGreen;
            this.dgvResultsSim3.Columns[13].DefaultCellStyle.BackColor = Color.LightGreen;
            #endregion
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Ingrese parámetros válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity = Convert.ToInt32(this.txtQuantity.Text);
            int from = Convert.ToInt32(this.txtFrom.Text);
            int to = Convert.ToInt32(this.txtTo.Text);
            int truckQuantitySim1 = Convert.ToInt32(this.txtTruckQuantitySim1.Text);
            int truckQuantitySim2 = Convert.ToInt32(this.txtTruckQuantitySim2.Text);
            int truckQuantitySim3 = Convert.ToInt32(this.txtTruckQuantitySim3.Text);
            GeneratorType generatorType;
            if (this.cmbGenerator.SelectedIndex == 0)
                generatorType = GeneratorType.Congruential;
            else
                generatorType = GeneratorType.Language;

            this.dgvResultsSim1.Rows.Clear();
            this.dgvResultsSim1.ColumnCount = 14;
            this.dgvResultsSim2.Rows.Clear();
            this.dgvResultsSim2.ColumnCount = 14;
            this.dgvResultsSim3.Rows.Clear();
            this.dgvResultsSim3.ColumnCount = 14;

            LoadTrucksColumns(truckQuantitySim1, truckQuantitySim2, truckQuantitySim3);

            Simulator sim1 = new Simulator(generatorType);
            Simulator sim2 = new Simulator(generatorType);
            Simulator sim3 = new Simulator(generatorType);

            #region Init Sim 1
            Queue<Truck> trucksQueueSim1 = CreateTruckQueue(truckQuantitySim1);
            StateRow previousSim1 = new StateRow();
            previousSim1.IterationNum = 0;
            previousSim1.Watch = 0;
            previousSim1.Event = "Init";
            previousSim1.Shovel = new Shovel() { Status = ShovelStatus.Busy, TruckQueue = trucksQueueSim1 };
            previousSim1.LoadRnd = sim1.GetRandom();
            previousSim1.TimeEndLoad = sim1.UniformGeneratorLoad.Generate(previousSim1.LoadRnd);
            previousSim1.NextEndLoad = previousSim1.Watch + previousSim1.TimeEndLoad;
            previousSim1.Trucks = trucksQueueSim1.ToList();
            previousSim1.Trucks.Insert(0, new Truck() { Id = 1, Status = TruckStatus.InLoad, TimeTravel = 0 });
            #endregion
            #region Init Sim 2
            Queue<Truck> trucksQueueSim2 = CreateTruckQueue(truckQuantitySim2);
            StateRow previousSim2 = new StateRow();
            previousSim2.IterationNum = 0;
            previousSim2.Watch = 0;
            previousSim2.Event = "Init";
            previousSim2.Shovel = new Shovel() { Status = ShovelStatus.Busy, TruckQueue = trucksQueueSim2 };
            previousSim2.LoadRnd = sim2.GetRandom();
            previousSim2.TimeEndLoad = sim2.UniformGeneratorLoad.Generate(previousSim2.LoadRnd);
            previousSim2.NextEndLoad = previousSim2.Watch + previousSim2.TimeEndLoad;
            previousSim2.Trucks = trucksQueueSim2.ToList();
            previousSim2.Trucks.Insert(0, new Truck() { Id = 1, Status = TruckStatus.InLoad, TimeTravel = 0 });
            #endregion
            #region Init Sim 3
            Queue<Truck> trucksQueueSim3 = CreateTruckQueue(truckQuantitySim3);
            StateRow previousSim3 = new StateRow();
            previousSim3.IterationNum = 0;
            previousSim3.Watch = 0;
            previousSim3.Event = "Init";
            previousSim3.Shovel = new Shovel() { Status = ShovelStatus.Busy, TruckQueue = trucksQueueSim3 };
            previousSim3.LoadRnd = sim3.GetRandom();
            previousSim3.TimeEndLoad = sim3.UniformGeneratorLoad.Generate(previousSim3.LoadRnd);
            previousSim3.NextEndLoad = previousSim3.Watch + previousSim3.TimeEndLoad;
            previousSim3.Trucks = trucksQueueSim3.ToList();
            previousSim3.Trucks.Insert(0, new Truck() { Id = 1, Status = TruckStatus.InLoad, TimeTravel = 0 });
            #endregion

            if (from == 0)
            {
                PrintStateRow(this.dgvResultsSim1, previousSim1);
                PrintStateRow(this.dgvResultsSim2, previousSim2);
                PrintStateRow(this.dgvResultsSim3, previousSim3);
            }

            for (int i=0; i<quantity; i++)
            {
                StateRow currentSim1 = sim1.NextStateRow(previousSim1, i);
                StateRow currentSim2 = sim2.NextStateRow(previousSim2, i);
                StateRow currentSim3 = sim3.NextStateRow(previousSim3, i);
                currentSim1.IterationNum = i + 1;
                currentSim2.IterationNum = i + 1;
                currentSim3.IterationNum = i + 1;

                if (i >= from - 1 && i < to)
                {
                    PrintStateRow(this.dgvResultsSim1, currentSim1);
                    PrintStateRow(this.dgvResultsSim2, currentSim2);
                    PrintStateRow(this.dgvResultsSim3, currentSim3);
                }

                previousSim1 = currentSim1;
                previousSim2 = currentSim2;
                previousSim3 = currentSim3;
            }

            double percentageInactivyTimeSim1 = ((previousSim1.Stat3 * 100) / previousSim1.Watch);
            this.txtInactivityPercentageSim1.Text = TruncateToFour(percentageInactivyTimeSim1).ToString();
            this.txtTravelsSim1.Text = previousSim1.Stat1.ToString();
            this.lblFinalWatchSim1.Text = "en " + TruncateToFour(previousSim1.Watch).ToString() + " mins";
            dgvPreviousResults.Rows.Add(truckQuantitySim1, TruncateToFour(percentageInactivyTimeSim1));
            this.txtInactivityTimeSim1.Text = previousSim1.Stat3.ToString();

            double percentageInactivyTimeSim2 = ((previousSim2.Stat3 * 100) / previousSim2.Watch);
            this.txtInactivityPercentageSim2.Text = TruncateToFour(percentageInactivyTimeSim2).ToString();
            this.txtTravelsSim2.Text = previousSim2.Stat1.ToString();
            this.lblFinalWatchSim2.Text = "en " + TruncateToFour(previousSim2.Watch).ToString() + " mins";
            dgvPreviousResults.Rows.Add(truckQuantitySim2, TruncateToFour(percentageInactivyTimeSim2));
            this.txtInactivityTimeSim2.Text = previousSim2.Stat3.ToString();

            double percentageInactivyTimeSim3 = ((previousSim3.Stat3 * 100) / previousSim3.Watch);
            this.txtInactivityPercentageSim3.Text = TruncateToFour(percentageInactivyTimeSim3).ToString();
            this.txtTravelsSim3.Text = previousSim3.Stat1.ToString();
            this.lblFinalWatchSim3.Text = "en " + TruncateToFour(previousSim3.Watch).ToString() + " mins";
            dgvPreviousResults.Rows.Add(truckQuantitySim3, TruncateToFour(percentageInactivyTimeSim3));
            this.txtInactivityTimeSim3.Text = previousSim3.Stat3.ToString();

            List<StateRow> results = new List<StateRow>();
            results.Add(previousSim1);
            results.Add(previousSim2);
            results.Add(previousSim3);
            results = results.OrderBy(sr => sr.Stat3).ThenBy(sr => sr.Trucks.Count).ToList();
            if (results.First() == previousSim1)
                this.txtBestResult.Text = "Simulación 1";
            else if (results.First() == previousSim2)
                this.txtBestResult.Text = "Simulación 2";
            else if (results.First() == previousSim3)
                this.txtBestResult.Text = "Simulación 3";
        }

        private void AllowPositiveIntegerNumbers(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnCleanPreviousResults_Click(object sender, EventArgs e)
        {
            this.dgvPreviousResults.Rows.Clear();
        }

        private void btnCleanParameters_Click(object sender, EventArgs e)
        {
            this.txtQuantity.Text = "";
            this.txtFrom.Text = "";
            this.txtTo.Text = "";
            this.txtTruckQuantitySim1.Text = "";
            this.txtTruckQuantitySim2.Text = "";
            this.txtTruckQuantitySim3.Text = "";
            this.txtInactivityPercentageSim1.Text = "";
            this.txtInactivityPercentageSim2.Text = "";
            this.txtInactivityPercentageSim3.Text = "";
            this.txtTravelsSim1.Text = "";
            this.txtTravelsSim2.Text = "";
            this.txtTravelsSim3.Text = "";
            this.txtBestResult.Text = "";
            this.txtInactivityTimeSim1.Text = "";
            this.txtInactivityTimeSim2.Text = "";
            this.txtInactivityTimeSim3.Text = "";
            this.lblFinalWatchSim1.Text = "";
            this.lblFinalWatchSim2.Text = "";
            this.lblFinalWatchSim3.Text = "";
            this.dgvResultsSim1.Rows.Clear();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(this.txtQuantity.Text))
                return false;
            if (string.IsNullOrEmpty(this.txtFrom.Text))
                return false;
            if (string.IsNullOrEmpty(this.txtTo.Text))
                return false;
            if (string.IsNullOrEmpty(this.txtTruckQuantitySim1.Text))
                return false;
            if (string.IsNullOrEmpty(this.txtTruckQuantitySim2.Text))
                return false;
            if (string.IsNullOrEmpty(this.txtTruckQuantitySim3.Text))
                return false;
            if (Convert.ToInt32(this.txtTo.Text) < Convert.ToInt32(this.txtFrom.Text))
                return false;
            if (Convert.ToInt32(this.txtQuantity.Text) <= 0)
                return false;
            if (Convert.ToInt32(this.txtTruckQuantitySim1.Text) <= 0)
                return false;
            if (Convert.ToInt32(this.txtTruckQuantitySim2.Text) <= 0)
                return false;
            if (Convert.ToInt32(this.txtTruckQuantitySim3.Text) <= 0)
                return false;
            return true;
        }

        private void LoadTrucksColumns(int truckQuantitySim1, int truckQuantitySim2, int truckQuantitySim3)
        {
            for (int i=0; i< truckQuantitySim1; i++)
            {
                this.dgvResultsSim1.ColumnCount += 2;
                int colCount = this.dgvResultsSim1.Columns.Count;
                this.dgvResultsSim1.Columns[colCount - 2].HeaderText = "Estado camión " + (i+1).ToString();
                this.dgvResultsSim1.Columns[colCount - 1].HeaderText = "T. fin viaje " + (i+1).ToString();
            }
            for (int i = 0; i < truckQuantitySim2; i++)
            {
                this.dgvResultsSim2.ColumnCount += 2;
                int colCount = this.dgvResultsSim2.Columns.Count;
                this.dgvResultsSim2.Columns[colCount - 2].HeaderText = "Estado camión " + (i + 1).ToString();
                this.dgvResultsSim2.Columns[colCount - 1].HeaderText = "T. fin viaje " + (i + 1).ToString();
            }
            for (int i = 0; i < truckQuantitySim3; i++)
            {
                this.dgvResultsSim3.ColumnCount += 2;
                int colCount = this.dgvResultsSim3.Columns.Count;
                this.dgvResultsSim3.Columns[colCount - 2].HeaderText = "Estado camión " + (i + 1).ToString();
                this.dgvResultsSim3.Columns[colCount - 1].HeaderText = "T. fin viaje " + (i + 1).ToString();
            }
        }

        private Queue<Truck> CreateTruckQueue(int truckQuantity)
        {
            Queue<Truck> trucksQueue = new Queue<Truck>();
            for (int i=2; i <= truckQuantity; i++)
            {
                Truck truck = new Truck();
                truck.Id = i;
                truck.Status = TruckStatus.Waiting;
                truck.TimeTravel = 0;
                trucksQueue.Enqueue(truck);
            }

            return trucksQueue;
        }

        private object DifferentToZero(double value)
        {
            if (value != 0)
                return (Math.Truncate(value * 10000) / 10000);
            else
                return "";
        }

        private double TruncateToFour(double value)
        {
            return (Math.Truncate(value * 10000) / 10000);
        }

        private string ShovelStatusToString(ShovelStatus status)
        {
            if (status == ShovelStatus.Free)
                return "Libre";
            return "Ocupada";
        }

        private string TruckStatusToString(TruckStatus status)
        {
            if (status == TruckStatus.InLoad)
                return "En carga";
            if (status == TruckStatus.InTravel)
                return "En viaje";
            return "En cola";
        }

        private void PrintStateRow(DataGridView dgv, StateRow stateRow)
        {
            dgv.Rows.Add(
                stateRow.IterationNum,
                DifferentToZero(stateRow.Watch),
                stateRow.Event,
                DifferentToZero(stateRow.LoadRnd),
                DifferentToZero(stateRow.TimeEndLoad),
                DifferentToZero(stateRow.NextEndLoad),
                DifferentToZero(stateRow.TravelRnd),
                DifferentToZero(stateRow.TimeEndTravel),
                DifferentToZero(stateRow.NextEndTravel),
                ShovelStatusToString(stateRow.Shovel.Status),
                stateRow.Shovel.TruckQueue.Count,
                TruncateToFour(stateRow.Stat1),
                TruncateToFour(stateRow.Stat2),
                TruncateToFour(stateRow.Stat3)
            );

            int rowCount = dgv.Rows.Count;
            int c = 0;
            for (int i=0; i<stateRow.Trucks.Count; i++)
            {
                dgv.Rows[rowCount - 1].Cells[14 + c].Value = TruckStatusToString(stateRow.Trucks[i].Status);
                dgv.Rows[rowCount - 1].Cells[14 + c + 1].Value = DifferentToZero(stateRow.Trucks[i].TimeTravel);
                c += 2;
            }
        }
    }
}
