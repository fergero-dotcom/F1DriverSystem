using System;
using System.Windows.Forms;
using F1DriverSystem.Core;

namespace F1DriverSystem.WinFormsApp
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EditDriverForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public string DriverName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Team { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Points { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public EditDriverForm(Driver driver)
        {
            InitializeComponent();
            cmbTeamUnused.Visible = false;
            label3.Visible = false;
            if (driver != null)
            {
                txtName.Text = driver.Name;
                txtTeam.Text = driver.Team;
                txtPoints.Text = driver.Points.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int points;
            if (string.IsNullOrWhiteSpace(txtName.Text))
            { MessageBox.Show("Введите имя гонщика."); return; }
            if (string.IsNullOrWhiteSpace(txtTeam.Text))
            { MessageBox.Show("Введите команду."); return; }
            if (!int.TryParse(txtPoints.Text, out points) || points < 0)
            { MessageBox.Show("Очки должны быть целым неотрицательным числом."); return; }
            DriverName = txtName.Text.Trim();
            Team = txtTeam.Text.Trim();
            Points = points;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
