// Approximation (Symbolic Regression) using Genetic Programming and Gene Expression Programming
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright ? AForge.NET, 2006-2011
// contacts@aforgenet.com
//

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;

using AForge;
using AForge.Genetic;
using AForge.Controls;
using System.Collections.Generic;

namespace Approximation
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView dataList;
        private System.Windows.Forms.ColumnHeader xColumnHeader;
        private System.Windows.Forms.ColumnHeader yColumnHeader;
        private System.Windows.Forms.Button loadDataButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.GroupBox groupBox2;
        private AForge.Controls.Chart chart;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox populationSizeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox selectionBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox iterationsBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox currentIterationBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox currentErrorBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox functionsSetBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox geneticMethodBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox solutionBox;

        private double[,] data = null;

        private int populationSize = 100;
        private int iterations = 1000;
        private int selectionMethod = 0;
        private int functionsSet = 0;
        private int geneticMethod = 0;
        private int timesNoImprovement = 0;

        private Thread workerThread = null;
        private Label label9;
        private TextBox txtTimesNoImproment;
        private Label label10;
        private volatile bool needToStop = false;

        // Constructor
        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            chart.AddDataSeries("data", Color.Red, Chart.SeriesType.Dots, 5);
            chart.AddDataSeries("solution", Color.Blue, Chart.SeriesType.Line, 1);

            selectionBox.SelectedIndex = selectionMethod;
            functionsSetBox.SelectedIndex = functionsSet;
            geneticMethodBox.SelectedIndex = geneticMethod;
            UpdateSettings();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataList = new System.Windows.Forms.ListView();
            this.xColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.loadDataButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chart = new AForge.Controls.Chart();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.geneticMethodBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.functionsSetBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.selectionBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.populationSizeBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.currentErrorBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.solutionBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTimesNoImproment = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataList);
            this.groupBox1.Controls.Add(this.loadDataButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 357);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data";
            // 
            // dataList
            // 
            this.dataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.xColumnHeader,
            this.yColumnHeader});
            this.dataList.FullRowSelect = true;
            this.dataList.GridLines = true;
            this.dataList.HideSelection = false;
            this.dataList.Location = new System.Drawing.Point(12, 23);
            this.dataList.Name = "dataList";
            this.dataList.Size = new System.Drawing.Size(192, 294);
            this.dataList.TabIndex = 0;
            this.dataList.UseCompatibleStateImageBehavior = false;
            this.dataList.View = System.Windows.Forms.View.Details;
            // 
            // xColumnHeader
            // 
            this.xColumnHeader.Text = "X";
            // 
            // yColumnHeader
            // 
            this.yColumnHeader.Text = "Y";
            // 
            // loadDataButton
            // 
            this.loadDataButton.Location = new System.Drawing.Point(12, 323);
            this.loadDataButton.Name = "loadDataButton";
            this.loadDataButton.Size = new System.Drawing.Size(90, 27);
            this.loadDataButton.TabIndex = 1;
            this.loadDataButton.Text = "&Load";
            this.loadDataButton.Click += new System.EventHandler(this.loadDataButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chart);
            this.groupBox2.Location = new System.Drawing.Point(240, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 357);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Function";
            // 
            // chart
            // 
            this.chart.Location = new System.Drawing.Point(12, 23);
            this.chart.Name = "chart";
            this.chart.RangeX = ((AForge.Range)(resources.GetObject("chart.RangeX")));
            this.chart.RangeY = ((AForge.Range)(resources.GetObject("chart.RangeY")));
            this.chart.Size = new System.Drawing.Size(336, 323);
            this.chart.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.geneticMethodBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.functionsSetBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.iterationsBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.selectionBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.populationSizeBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(612, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(222, 228);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // geneticMethodBox
            // 
            this.geneticMethodBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.geneticMethodBox.Items.AddRange(new object[] {
            "GP",
            "GEP"});
            this.geneticMethodBox.Location = new System.Drawing.Point(132, 110);
            this.geneticMethodBox.Name = "geneticMethodBox";
            this.geneticMethodBox.Size = new System.Drawing.Size(78, 24);
            this.geneticMethodBox.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 18);
            this.label8.TabIndex = 6;
            this.label8.Text = "Genetic method:";
            // 
            // functionsSetBox
            // 
            this.functionsSetBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionsSetBox.Items.AddRange(new object[] {
            "Simple",
            "Extended"});
            this.functionsSetBox.Location = new System.Drawing.Point(132, 81);
            this.functionsSetBox.Name = "functionsSetBox";
            this.functionsSetBox.Size = new System.Drawing.Size(78, 24);
            this.functionsSetBox.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 19);
            this.label7.TabIndex = 4;
            this.label7.Text = "Functions set:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(150, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "( 0 - inifinity )";
            // 
            // iterationsBox
            // 
            this.iterationsBox.Location = new System.Drawing.Point(150, 179);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.Size = new System.Drawing.Size(60, 22);
            this.iterationsBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Iterations:";
            // 
            // selectionBox
            // 
            this.selectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectionBox.Items.AddRange(new object[] {
            "Elite",
            "Rank",
            "Roulette"});
            this.selectionBox.Location = new System.Drawing.Point(132, 52);
            this.selectionBox.Name = "selectionBox";
            this.selectionBox.Size = new System.Drawing.Size(78, 24);
            this.selectionBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Selection method:";
            // 
            // populationSizeBox
            // 
            this.populationSizeBox.Location = new System.Drawing.Point(150, 23);
            this.populationSizeBox.Name = "populationSizeBox";
            this.populationSizeBox.Size = new System.Drawing.Size(60, 22);
            this.populationSizeBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Population size:";
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(636, 343);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(90, 26);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(744, 343);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(90, 26);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.currentErrorBox);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.currentIterationBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(612, 249);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(222, 87);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current iteration";
            // 
            // currentErrorBox
            // 
            this.currentErrorBox.Location = new System.Drawing.Point(150, 52);
            this.currentErrorBox.Name = "currentErrorBox";
            this.currentErrorBox.ReadOnly = true;
            this.currentErrorBox.Size = new System.Drawing.Size(60, 22);
            this.currentErrorBox.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 19);
            this.label6.TabIndex = 2;
            this.label6.Text = "Error:";
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(150, 23);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.ReadOnly = true;
            this.currentIterationBox.Size = new System.Drawing.Size(60, 22);
            this.currentIterationBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 19);
            this.label5.TabIndex = 0;
            this.label5.Text = "Iteration:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.solutionBox);
            this.groupBox5.Location = new System.Drawing.Point(12, 381);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(822, 57);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Solution:";
            // 
            // solutionBox
            // 
            this.solutionBox.Location = new System.Drawing.Point(12, 23);
            this.solutionBox.Name = "solutionBox";
            this.solutionBox.ReadOnly = true;
            this.solutionBox.Size = new System.Drawing.Size(798, 22);
            this.solutionBox.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(294, 464);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(384, 17);
            this.label9.TabIndex = 7;
            this.label9.Text = "When there has been no improvement in the population for ";
            // 
            // txtTimesNoImproment
            // 
            this.txtTimesNoImproment.Location = new System.Drawing.Point(684, 461);
            this.txtTimesNoImproment.Name = "txtTimesNoImproment";
            this.txtTimesNoImproment.Size = new System.Drawing.Size(48, 22);
            this.txtTimesNoImproment.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(738, 461);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 19);
            this.label10.TabIndex = 2;
            this.label10.Text = "iterations.";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(888, 531);
            this.Controls.Add(this.txtTimesNoImproment);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Approximation (Symbolic Regression) using Genetic Programming and Gene Expression" +
    " Programming";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        // Delegates to enable async calls for setting controls properties
        private delegate void SetTextCallback(System.Windows.Forms.Control control, string text);

        // Thread safe updating of control's text property
        private void SetText(System.Windows.Forms.Control control, string text)
        {
            if (control.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { control, text });
            }
            else
            {
                control.Text = text;
            }
        }

        // On main form closing
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // check if worker thread is running
            if ((workerThread != null) && (workerThread.IsAlive))
            {
                needToStop = true;
                while (!workerThread.Join(100))
                    Application.DoEvents();
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            populationSizeBox.Text = populationSize.ToString();
            iterationsBox.Text = iterations.ToString();
            txtTimesNoImproment.Text = timesNoImprovement.ToString();
        }

        // Load data
        private void loadDataButton_Click(object sender, System.EventArgs e)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                // read maximum 50 points
                float[,] tempData = new float[50, 2];
                float minX = float.MaxValue;
                float maxX = float.MinValue;

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);
                    string str = null;
                    int i = 0;

                    // read the data
                    while ((i < 50) && ((str = reader.ReadLine()) != null))
                    {
                        string[] strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');
                        // parse X
                        tempData[i, 0] = float.Parse(strs[0]);
                        tempData[i, 1] = float.Parse(strs[1]);

                        // search for min value
                        if (tempData[i, 0] < minX)
                            minX = tempData[i, 0];
                        // search for max value
                        if (tempData[i, 0] > maxX)
                            maxX = tempData[i, 0];

                        i++;
                    }

                    // allocate and set data
                    data = new double[i, 2];
                    Array.Copy(tempData, 0, data, 0, i * 2);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }

                // update list and chart
                UpdateDataListView();
                chart.RangeX = new Range(minX, maxX);
                chart.UpdateDataSeries("data", data);
                chart.UpdateDataSeries("solution", null);
                // enable "Start" button
                startButton.Enabled = true;
            }
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all current records
            dataList.Items.Clear();
            // add new records
            for (int i = 0, n = data.GetLength(0); i < n; i++)
            {
                dataList.Items.Add(data[i, 0].ToString());
                dataList.Items[i].SubItems.Add(data[i, 1].ToString());
            }
        }

        // Delegates to enable async calls for setting controls properties
        private delegate void EnableCallback(bool enable);

        // Enable/disale controls (safe for threading)
        private void EnableControls(bool enable)
        {
            if (InvokeRequired)
            {
                EnableCallback d = new EnableCallback(EnableControls);
                Invoke(d, new object[] { enable });
            }
            else
            {
                loadDataButton.Enabled = enable;
                populationSizeBox.Enabled = enable;
                iterationsBox.Enabled = enable;
                selectionBox.Enabled = enable;
                functionsSetBox.Enabled = enable;
                geneticMethodBox.Enabled = enable;

                startButton.Enabled = enable;
                stopButton.Enabled = !enable;
                txtTimesNoImproment.Enabled = enable;
            }
        }

        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {
            solutionBox.Text = string.Empty;

            try
            {
                timesNoImprovement = int.Parse(txtTimesNoImproment.Text);
            }
            catch
            {
                timesNoImprovement = 5;
            }

            // get population size
            try
            {
                populationSize = Math.Max(10, Math.Min(100, int.Parse(populationSizeBox.Text)));
            }
            catch
            {
                populationSize = 40;
            }
            // iterations
            try
            {
                iterations = Math.Max(0, int.Parse(iterationsBox.Text));
            }
            catch
            {
                iterations = 100;
            }
            // update settings controls
            UpdateSettings();

            selectionMethod = selectionBox.SelectedIndex;
            functionsSet = functionsSetBox.SelectedIndex;
            geneticMethod = geneticMethodBox.SelectedIndex;

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(SearchSolution));

            workerThread.Start();
        }

        // On button "Stop"
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            // stop worker thread
            needToStop = true;
            while (!workerThread.Join(100))
                Application.DoEvents();
            workerThread = null;
        }

        // Worker thread

        void SearchSolution()
        {
            // create fitness function
            SymbolicRegressionFitness fitness = new SymbolicRegressionFitness(data, new double[] { 1, 2, 3, 5, 7 });
            // create gene function
            IGPGene gene = (functionsSet == 0) ?
                (IGPGene)new SimpleGeneFunction(6) :
                (IGPGene)new ExtendedGeneFunction(6);
            // create population
            Population population = new Population(populationSize,
                (geneticMethod == 0) ?
                    (IChromosome)new GPTreeChromosome(gene) :
                    (IChromosome)new GEPChromosome(gene, 15),
                fitness,
                (selectionMethod == 0) ? (ISelectionMethod)new EliteSelection() :
                (selectionMethod == 1) ? (ISelectionMethod)new RankSelection() :
                                           (ISelectionMethod)new RouletteWheelSelection()
                );
            // iterations
            int i = 1;
            // solution array
            double[,] solution = new double[50, 2];
            double[] input = new double[6] { 0, 1, 2, 3, 5, 7 };

            // calculate X values to be used with solution function
            for (int j = 0; j < 50; j++)
            {
                solution[j, 0] = chart.RangeX.Min + (double)j * chart.RangeX.Length / 49;
            }

            int countNoImprovement = 0;
            // loop
            while (!needToStop)
            {
                // run one epoch of genetic algorithm

                //_________________________________________________________________________
                //---------------------Terminate Condition------------------------------
                //Stop chuong trinh neu the he con kem hon the he cha me {timesNoImprovement} lan lien tiep
                //
                var parentValue = population.FitnessAvg;

                population.RunEpoch();

                var offSprings = population.FitnessAvg;
                if (offSprings < parentValue)
                {
                    countNoImprovement++;
                    if (countNoImprovement == timesNoImprovement)
                    {
                        break;
                    }
                }
                else
                {
                    countNoImprovement = 0;
                }



                try
                {
                    // get best solution
                    string bestFunction = population.BestChromosome.ToString();

                    // calculate best function
                    for (int j = 0; j < 50; j++)
                    {
                        input[0] = solution[j, 0];
                        solution[j, 1] = PolishExpression.Evaluate(bestFunction, input);
                    }
                    chart.UpdateDataSeries("solution", solution);
                    // calculate error
                    double error = 0.0;
                    for (int j = 0, k = data.GetLength(0); j < k; j++)
                    {
                        input[0] = data[j, 0];
                        error += Math.Abs(data[j, 1] - PolishExpression.Evaluate(bestFunction, input));
                    }

                    // set current iteration's info
                    SetText(currentIterationBox, i.ToString());
                    SetText(currentErrorBox, error.ToString("F3"));
                }
                catch
                {
                    // remove any solutions from chart in case of any errors
                    chart.UpdateDataSeries("solution", null);
                }

                // increase current iteration
                i++;

                //
                if ((iterations != 0) && (i > iterations))
                    break;
            }

            // show solution
            string postfix = population.BestChromosome.ToString();

            //_______________________________________________________________________________________________
            //chuyen hau thu tu -> trung thu tu

            try
            {
                string infix = GetInfix(postfix);
                SetText(solutionBox, infix);
            }
            catch
            {
                SetText(solutionBox, "Error when convert from postFix to inFix");
            }

            // enable settings controls
            EnableControls(true);
        }

        string[] binaryOperator = { "+", "-", "*", "/" };
        //string[] unaryOperator = { "sin", "cos", "tan", "ln", "sqrt", "exp" };

        //bool IsUnaryOperator(string _operator)
        //{
        //    foreach (var op in unaryOperator)
        //    {
        //        if (_operator.Equals(op))
        //            return true;
        //    }
        //    return false;
        //}

        bool IsBinaryOperator(string _operator)
        {
            foreach (var op in binaryOperator)
            {
                if (_operator.Equals(op))
                    return true;
            }
            return false;
        }

        bool IsOperand(string operand)
        {
            if (operand.Contains("$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        String GetInfix(String postFix)
        {
            Stack s = new Stack();
            string[] expressionParts = postFix.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < expressionParts.Length; i++)
            {
                if (IsOperand(expressionParts[i]))
                {
                    s.Push(expressionParts[i] + " ");
                }
                else if (IsBinaryOperator(expressionParts[i]))
                {
                    string binaryOperator = expressionParts[i];
                    string operand1 = s.Peek().ToString();
                    s.Pop();
                    string operand2 = s.Peek().ToString();
                    s.Pop();

                    s.Push("( " + operand2 + binaryOperator + operand1 + " )");
                }
                else // this is unary operator
                {
                    string unaryOperator = expressionParts[i];
                    string operand = s.Peek().ToString();
                    s.Pop();

                    s.Push(unaryOperator + "( " + operand + " )");
                }

            }

            return s.Peek().ToString();
        }

    }
}
