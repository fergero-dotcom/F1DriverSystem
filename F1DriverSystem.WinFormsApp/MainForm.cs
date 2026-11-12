using System;
using System.Collections.Generic;
using System.Windows.Forms;
using F1DriverSystem.Core;
using Ninject;

namespace F1DriverSystem.WinFormsApp
{
    /// <summary>
    /// Главная форма системы учёта гонщиков Ф1.
    /// Объект бизнес-логики создаётся через IoC-контейнер Ninject,
    /// который автоматически внедряет репозиторий в объект BL.
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly IKernel ninjectKernel =
            new StandardKernel(new SimpleConfigModule());

        private readonly Logic logic = ninjectKernel.Get<Logic>();

        /// <summary>
        /// Инициализирует главную форму и загружает список гонщиков.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            cmbFilterTeam.Items.Clear();
            cmbFilterTeam.Items.AddRange(new object[] { "Mercedes", "Ferrari", "McLaren", "Red Bull Racing", "Racing Bulls", "Alpine", "Haas F1 Team", "Audi", "Williams", "Aston Martin", "Cadillac" });
            cmbFilterTeam.DropDownStyle = ComboBoxStyle.DropDown;
            RefreshGrid(logic.GetAllDrivers());
        }

        /// <summary>
        /// Обновляет таблицу гонщиков.
        /// </summary>
        /// <param name="drivers">Список гонщиков для отображения.</param>
        private void RefreshGrid(List<Driver> drivers)
        {
            dataGridViewDrivers.DataSource = null;
            dataGridViewDrivers.DataSource = drivers;
            dataGridViewDrivers.ReadOnly = true;
            dataGridViewDrivers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (dataGridViewDrivers.Columns["Id"] != null) dataGridViewDrivers.Columns["Id"].HeaderText = "ID";
            if (dataGridViewDrivers.Columns["Name"] != null) dataGridViewDrivers.Columns["Name"].HeaderText = "Гонщик";
            if (dataGridViewDrivers.Columns["Team"] != null) dataGridViewDrivers.Columns["Team"].HeaderText = "Команда";
            if (dataGridViewDrivers.Columns["Points"] != null) dataGridViewDrivers.Columns["Points"].HeaderText = "Очки";
        }

        /// <summary>
        /// Возвращает выбранного в таблице гонщика.
        /// </summary>
        /// <returns>Выбранный гонщик или null.</returns>
        private Driver GetSelectedDriver()
        {
            if (dataGridViewDrivers.CurrentRow == null)
            {
                return null;
            }
            return dataGridViewDrivers.CurrentRow.DataBoundItem as Driver;
        }

        /// <summary>
        /// Обработчик кнопки добавления гонщика.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (EditDriverForm form = new EditDriverForm(null))
                if (form.ShowDialog() == DialogResult.OK)
                {
                    logic.AddDriver(form.DriverName, form.Team, form.Points);
                    RefreshGrid(logic.GetAllDrivers());
                }
        }

        /// <summary>
        /// Обработчик кнопки редактирования гонщика.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            Driver selected = GetSelectedDriver();
            if (selected == null) { MessageBox.Show("Выберите гонщика."); return; }
            using (EditDriverForm form = new EditDriverForm(selected))
                if (form.ShowDialog() == DialogResult.OK)
                {
                    logic.UpdateDriver(selected.Id, form.DriverName, form.Team, form.Points);
                    RefreshGrid(logic.GetAllDrivers());
                }
        }

        /// <summary>
        /// Обработчик кнопки удаления гонщика.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            Driver selected = GetSelectedDriver();
            if (selected == null) { MessageBox.Show("Выберите гонщика."); return; }
            logic.DeleteDriver(selected.Id);
            RefreshGrid(logic.GetAllDrivers());
        }

        /// <summary>
        /// Обработчик кнопки фильтрации по команде.
        /// </summary>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbFilterTeam.Text))
            { MessageBox.Show("Введите название команды."); return; }
            RefreshGrid(logic.GetDriversByTeam(cmbFilterTeam.Text));
        }

        /// <summary>
        /// Обработчик кнопки сброса фильтра.
        /// </summary>
        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            RefreshGrid(logic.GetAllDrivers());
        }

        /// <summary>
        /// Обработчик кнопки расчёта среднего количества очков.
        /// </summary>
        private void btnCalculateAvg_Click(object sender, EventArgs e)
        {
            lblAvgPoints.Text = "Среднее количество очков: " + logic.CalculateAveragePoints().ToString("F2");
        }
    }
}
