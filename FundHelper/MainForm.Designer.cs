namespace FundHelper
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridViewFund = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewStock = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.labelGold = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFund)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // timerUpdate
            // 
            this.timerUpdate.Interval = 5000;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 561);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridViewFund);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(976, 531);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基金";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridViewFund
            // 
            this.dataGridViewFund.AllowUserToAddRows = false;
            this.dataGridViewFund.AllowUserToDeleteRows = false;
            this.dataGridViewFund.AllowUserToOrderColumns = true;
            this.dataGridViewFund.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFund.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFund.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewFund.Name = "dataGridViewFund";
            this.dataGridViewFund.ReadOnly = true;
            this.dataGridViewFund.RowTemplate.Height = 23;
            this.dataGridViewFund.Size = new System.Drawing.Size(970, 525);
            this.dataGridViewFund.TabIndex = 0;
            this.dataGridViewFund.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFund_CellDoubleClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridViewStock);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(976, 531);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "指数";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewStock
            // 
            this.dataGridViewStock.AllowUserToAddRows = false;
            this.dataGridViewStock.AllowUserToDeleteRows = false;
            this.dataGridViewStock.AllowUserToOrderColumns = true;
            this.dataGridViewStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewStock.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewStock.Name = "dataGridViewStock";
            this.dataGridViewStock.ReadOnly = true;
            this.dataGridViewStock.RowTemplate.Height = 23;
            this.dataGridViewStock.Size = new System.Drawing.Size(970, 525);
            this.dataGridViewStock.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.labelGold);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(976, 531);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "黄金";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // labelGold
            // 
            this.labelGold.AutoSize = true;
            this.labelGold.Location = new System.Drawing.Point(8, 3);
            this.labelGold.Name = "labelGold";
            this.labelGold.Size = new System.Drawing.Size(64, 17);
            this.labelGold.TabIndex = 0;
            this.labelGold.Text = "labelGold";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.checkBox1);
            this.tabPage4.Controls.Add(this.chart1);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(976, 531);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(970, 525);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(879, 504);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(89, 21);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "基金助手";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFund)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridViewStock;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridViewFund;
        private System.Windows.Forms.Label labelGold;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

