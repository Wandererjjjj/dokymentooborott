using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Deform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeMenuStrip(); // Инициализация меню
        }

        private void InitializeMenuStrip()
        {
            // Создаём главное меню
            MenuStrip mainMenu = new MenuStrip
            {
                Location = new System.Drawing.Point(0, 0),
                Name = "mainMenuStrip",
                Size = new System.Drawing.Size(860, 24),
                TabIndex = 19
            };

            // Создаём пункт меню "Файл"
            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("Файл");

            // Создаём подпункт "Экспорт в Excel"
            ToolStripMenuItem exportMenuItem = new ToolStripMenuItem("Экспорт в Excel");
            exportMenuItem.Click += BTExport_Click; // Привязываем обработчик события

            // Добавляем подпункт "Построить график"
            ToolStripMenuItem chartMenuItem = new ToolStripMenuItem("Построить график в Excel");
            chartMenuItem.Click += BTChart_Click; // Привязываем обработчик события

            // Добавляем новый пункт меню "Построить график в отдельном окне"
            ToolStripMenuItem chartWindowMenuItem = new ToolStripMenuItem("Построить график в отдельном окне");
            chartWindowMenuItem.Click += BTChartWindow_Click; // Привязываем новый обработчик события

            // Добавляем подпункты в меню "Файл"
            fileMenuItem.DropDownItems.Add(exportMenuItem);
            fileMenuItem.DropDownItems.Add(chartMenuItem);
            fileMenuItem.DropDownItems.Add(chartWindowMenuItem);

            // Добавляем пункт меню в главное меню
            mainMenu.Items.Add(fileMenuItem);

            // Добавляем меню на форму
            this.Controls.Add(mainMenu);

            // Устанавливаем меню как главное меню формы
            this.MainMenuStrip = mainMenu;

            // Сдвигаем все остальные элементы управления вниз
            foreach (Control control in this.Controls)
            {
                if (control != mainMenu)
                {
                    control.Location = new System.Drawing.Point(control.Location.X, control.Location.Y + mainMenu.Height);
                }
            }

            // Увеличиваем высоту формы на высоту меню
            this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height + mainMenu.Height);
        }



        private void LoadDataFromExcel(string filePath, string xColumn, string yColumn, string selectedMaterial,
    List<double> xValues, List<double> yValues, List<string> materialValues)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                // Открываем Excel и загружаем файл
                excelApp = new Excel.Application();
                excelApp.Visible = false;
                workbook = excelApp.Workbooks.Open(filePath);
                worksheet = (Excel.Worksheet)workbook.Worksheets[1];

                // Находим индексы колонок
                int xColumnIndex = -1;
                int yColumnIndex = -1;
                int materialColumnIndex = -1;
                int lastColumn = worksheet.UsedRange.Columns.Count;

                for (int i = 1; i <= lastColumn; i++)
                {
                    string headerValue = Convert.ToString((worksheet.Cells[1, i] as Excel.Range).Value2);
                    if (headerValue == xColumn)
                        xColumnIndex = i;
                    if (headerValue == yColumn)
                        yColumnIndex = i;
                    if (headerValue == "Материал")
                        materialColumnIndex = i;
                }

                if (xColumnIndex == -1 || yColumnIndex == -1)
                {
                    MessageBox.Show("Не удалось найти указанные колонки в файле.");
                    return;
                }

                if (materialColumnIndex == -1 && selectedMaterial != "Все материалы")
                {
                    MessageBox.Show("Колонка 'Материал' не найдена в файле, но выбран конкретный материал для фильтрации.");
                    return;
                }

                // Получаем количество строк с данными
                int lastRow = worksheet.UsedRange.Rows.Count;

                // Для отладки: выводим информацию о колонке материалов
                string debugInfo = "Содержимое колонки 'Материал':\n";
                if (materialColumnIndex != -1)
                {
                    for (int i = 2; i <= Math.Min(lastRow, 10); i++) // Проверяем первые 10 строк
                    {
                        object materialValue = (worksheet.Cells[i, materialColumnIndex] as Excel.Range).Value2;
                        debugInfo += $"Строка {i}: {(materialValue != null ? materialValue.ToString() : "null")}\n";
                    }
                    MessageBox.Show(debugInfo);
                }

                // Загружаем данные из колонок с учетом выбранного материала
                for (int i = 2; i <= lastRow; i++) // Начинаем со 2-й строки, пропуская заголовки
                {
                    object xValue = (worksheet.Cells[i, xColumnIndex] as Excel.Range).Value2;
                    object yValue = (worksheet.Cells[i, yColumnIndex] as Excel.Range).Value2;

                    bool includeRow = true;

                    // Если выбран конкретный материал, проверяем соответствие
                    if (materialColumnIndex != -1 && selectedMaterial != "Все материалы")
                    {
                        object materialValue = (worksheet.Cells[i, materialColumnIndex] as Excel.Range).Value2;
                        string materialString = materialValue != null ? materialValue.ToString() : "";

                        // Проверяем соответствие материала, учитывая возможные вариации
                        if (selectedMaterial == "Алюминий")
                        {
                            includeRow = materialString.Contains("Al") || materialString.Contains("Алюминий") ||
                                        materialString.Contains("алюминий") || materialString.Contains("Aluminum");
                        }
                        else if (selectedMaterial == "Медь")
                        {
                            includeRow = materialString.Contains("Cu") || materialString.Contains("Медь") ||
                                        materialString.Contains("медь") || materialString.Contains("Copper");
                        }
                        else if (selectedMaterial == "Железо")
                        {
                            includeRow = materialString.Contains("Fe") || materialString.Contains("Железо") ||
                                        materialString.Contains("железо") || materialString.Contains("Iron");
                        }
                        else if (selectedMaterial == "Никель")
                        {
                            includeRow = materialString.Contains("Ni") || materialString.Contains("Никель") ||
                                        materialString.Contains("никель") || materialString.Contains("Nickel");
                        }
                        else
                        {
                            includeRow = materialString == selectedMaterial;
                        }

                        if (!includeRow)
                            continue; // Пропускаем строки с другим материалом
                    }

                    if (xValue != null && yValue != null)
                    {
                        xValues.Add(Convert.ToDouble(xValue));
                        yValues.Add(Convert.ToDouble(yValue));

                        if (materialColumnIndex != -1)
                        {
                            object materialValue = (worksheet.Cells[i, materialColumnIndex] as Excel.Range).Value2;
                            materialValues.Add(materialValue != null ? materialValue.ToString() : "Неизвестно");
                        }
                        else
                        {
                            materialValues.Add("Неизвестно");
                        }
                    }
                }

                // Если нет данных после фильтрации, выводим сообщение
                if (xValues.Count == 0 && selectedMaterial != "Все материалы")
                {
                    MessageBox.Show($"Не найдено данных для материала '{selectedMaterial}'. Проверьте правильность написания материала в файле Excel.");
                }
            }
            finally
            {
                // Освобождаем ресурсы
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (workbook != null)
                {
                    workbook.Close(false);
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        private void ShowChartInModalWindow(string xColumn, string yColumn, List<double> xValues, List<double> yValues,
    string selectedMaterial, List<string> materialValues)
        {
            // Создаем новую форму для графика
            Form chartForm = new Form
            {
                Text = $"График {yColumn} от {xColumn}",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.Sizable,
                MaximizeBox = true,
                MinimizeBox = true
            };

            // Создаем элемент Chart
            Chart chart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            // Создаем область для графика
            ChartArea chartArea = new ChartArea("MainChartArea");
            chart.ChartAreas.Add(chartArea);

            if (selectedMaterial == "Все материалы" && materialValues.Count > 0)
            {
                // Получаем уникальные материалы
                HashSet<string> uniqueMaterials = new HashSet<string>(materialValues);

                // Словарь для хранения цветов для материалов
                Dictionary<string, Color> materialColors = new Dictionary<string, Color>
        {
            { "Cu", Color.Brown },
            { "Медь", Color.Brown },
            { "медь", Color.Brown },
            { "Copper", Color.Brown },

            { "Al", Color.Silver },
            { "Алюминий", Color.Silver },
            { "алюминий", Color.Silver },
            { "Aluminum", Color.Silver },

            { "Fe", Color.DarkGray },
            { "Железо", Color.DarkGray },
            { "железо", Color.DarkGray },
            { "Iron", Color.DarkGray },

            { "Ni", Color.LightGray },
            { "Никель", Color.LightGray },
            { "никель", Color.LightGray },
            { "Nickel", Color.LightGray }
        };

                // Цвета по умолчанию для неизвестных материалов
                Color[] defaultColors = {
            Color.Blue, Color.Red, Color.Green, Color.Purple,
            Color.Orange, Color.Cyan, Color.Magenta, Color.Yellow
        };
                int colorIndex = 0;

                // Создаем серию для каждого уникального материала
                foreach (string material in uniqueMaterials)
                {
                    Series series = new Series
                    {
                        Name = material,
                        ChartType = SeriesChartType.Point,
                        MarkerStyle = MarkerStyle.Circle,
                        MarkerSize = 8
                    };

                    // Определяем цвет для материала
                    if (materialColors.ContainsKey(material))
                    {
                        series.Color = materialColors[material];
                    }
                    else
                    {
                        // Если материал не найден в словаре, используем цвет по умолчанию
                        series.Color = defaultColors[colorIndex % defaultColors.Length];
                        colorIndex++;
                    }

                    // Добавляем точки данных для текущего материала
                    for (int i = 0; i < xValues.Count; i++)
                    {
                        if (materialValues[i] == material)
                        {
                            series.Points.AddXY(xValues[i], yValues[i]);
                        }
                    }

                    // Добавляем серию на график
                    chart.Series.Add(series);
                }
            }
            else
            {
                // Если выбран конкретный материал, используем одну серию
                Series series = new Series
                {
                    Name = selectedMaterial,
                    ChartType = SeriesChartType.Point,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 8
                };

                // Добавляем все точки данных
                for (int i = 0; i < xValues.Count; i++)
                {
                    series.Points.AddXY(xValues[i], yValues[i]);
                }

                // Добавляем серию на график
                chart.Series.Add(series);
            }

            // Настраиваем заголовок графика
            string titleText = $"Зависимость {yColumn} от {xColumn}";
            if (selectedMaterial != "Все материалы")
                titleText += $" - Материал: {selectedMaterial}";

            Title title = new Title
            {
                Text = titleText,
                Docking = Docking.Top,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Black
            };
            chart.Titles.Add(title);

            // Настраиваем оси
            chartArea.AxisX.Title = xColumn;
            chartArea.AxisY.Title = yColumn;
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;

            // Добавляем легенду
            Legend legend = new Legend
            {
                Docking = Docking.Bottom,
                Title = "Материал",
                TitleFont = new Font("Arial", 10, FontStyle.Bold)
            };
            chart.Legends.Add(legend);

            // Создаем панель для размещения кнопок
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };

            // Добавляем кнопку для экспорта графика в Excel
            Button exportButton = new Button
            {
                Text = "Экспорт в Excel",
                Width = 120,
                Height = 30,
                Location = new Point(10, 5)
            };

            exportButton.Click += (s, e) => {
                ExportChartToExcel(xColumn, yColumn, xValues, yValues, selectedMaterial);
            };

            buttonPanel.Controls.Add(exportButton);

            // Добавляем элементы на форму
            chartForm.Controls.Add(chart);
            chartForm.Controls.Add(buttonPanel);

            // Показываем форму как модальное окно
            chartForm.ShowDialog();
        }

        private void ExportChartToExcel(string xColumn, string yColumn, List<double> xValues, List<double> yValues, string material)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet dataSheet = null;
            Excel.Worksheet chartSheet = null;

            try
            {
                // Запускаем Excel
                excelApp = new Excel.Application();
                excelApp.Visible = true;
                workbook = excelApp.Workbooks.Add();

                // Создаем лист для данных
                dataSheet = (Excel.Worksheet)workbook.Worksheets[1];
                dataSheet.Name = "Данные";

                // Заполняем заголовки
                dataSheet.Cells[1, 1] = xColumn;
                dataSheet.Cells[1, 2] = yColumn;
                dataSheet.Cells[1, 3] = "Материал";

                // Заполняем данные
                for (int i = 0; i < xValues.Count; i++)
                {
                    dataSheet.Cells[i + 2, 1] = xValues[i];
                    dataSheet.Cells[i + 2, 2] = yValues[i];
                    dataSheet.Cells[i + 2, 3] = material;
                }

                // Создаем лист для графика
                chartSheet = (Excel.Worksheet)workbook.Worksheets.Add(After: dataSheet);
                chartSheet.Name = "График";

                // Создаем график
                Excel.ChartObjects chartObjects = (Excel.ChartObjects)chartSheet.ChartObjects();
                Excel.ChartObject chartObject = chartObjects.Add(60, 30, 600, 400);
                Excel.Chart excelChart = chartObject.Chart;

                // Определяем диапазон данных
                Excel.Range xRange = dataSheet.Range[dataSheet.Cells[2, 1], dataSheet.Cells[xValues.Count + 1, 1]];
                Excel.Range yRange = dataSheet.Range[dataSheet.Cells[2, 2], dataSheet.Cells[yValues.Count + 1, 2]];

                // Устанавливаем тип графика
                excelChart.ChartType = Excel.XlChartType.xlXYScatterSmooth;

                // Добавляем серию данных
                Excel.Series series = excelChart.SeriesCollection().NewSeries();
                series.XValues = xRange;
                series.Values = yRange;
                series.Name = material;

                // Настраиваем заголовки
                excelChart.HasTitle = true;
                excelChart.ChartTitle.Text = $"Зависимость {yColumn} от {xColumn} - Материал: {material}";

                excelChart.Axes(Excel.XlAxisType.xlCategory).HasTitle = true;
                excelChart.Axes(Excel.XlAxisType.xlCategory).AxisTitle.Text = xColumn;

                excelChart.Axes(Excel.XlAxisType.xlValue).HasTitle = true;
                excelChart.Axes(Excel.XlAxisType.xlValue).AxisTitle.Text = yColumn;

                // Добавляем легенду
                excelChart.HasLegend = true;
                excelChart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionBottom;

                // Автоподбор ширины столбцов
                dataSheet.Columns.AutoFit();

                MessageBox.Show("График успешно экспортирован в Excel!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте графика: {ex.Message}");
            }
            finally
            {
                // Не закрываем Excel, чтобы пользователь мог работать с файлом
                // Освобождаем COM-объекты
                if (chartSheet != null) Marshal.ReleaseComObject(chartSheet);
                if (dataSheet != null) Marshal.ReleaseComObject(dataSheet);
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);
            }
        }

        private void BTChartWindow_Click(object sender, EventArgs e)
        {
            try
            {
                // Создаем диалог открытия файла
                OpenFileDialog openDialog = new OpenFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Выберите файл Excel с данными"
                };

                // Если пользователь выбрал файл и нажал OK
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openDialog.FileName;

                    // Проверяем, существует ли файл
                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("Файл не найден.");
                        return;
                    }

                    // Создаем форму для выбора параметров графика
                    using (var chartOptionsForm = new Form())
                    {
                        chartOptionsForm.Text = "Параметры графика";
                        chartOptionsForm.Size = new Size(400, 250); // Увеличиваем высоту для дополнительного элемента
                        chartOptionsForm.StartPosition = FormStartPosition.CenterParent;
                        chartOptionsForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                        chartOptionsForm.MaximizeBox = false;
                        chartOptionsForm.MinimizeBox = false;

                        // Создаем метки и комбобоксы для выбора данных
                        var lblX = new Label { Text = "Ось X:", Location = new Point(20, 20), AutoSize = true };
                        var lblY = new Label { Text = "Ось Y:", Location = new Point(20, 50), AutoSize = true };
                        var lblMaterial = new Label { Text = "Материал:", Location = new Point(20, 80), AutoSize = true };

                        var cmbX = new ComboBox { Location = new Point(120, 20), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                        var cmbY = new ComboBox { Location = new Point(120, 50), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                        var cmbMaterial = new ComboBox { Location = new Point(120, 80), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

                        // Добавляем возможные колонки для выбора
                        string[] columns = {
                    "Высота до (см)",
                    "Диаметр до (см)",
                    "Высота после (см)",
                    "Диаметр после (см)",
                    "Разница диаметра (см)",
                    "Разница высоты (см)"
                };

                        cmbX.Items.AddRange(columns);
                        cmbY.Items.AddRange(columns);

                        // Добавляем материалы
                        string[] materials = {
                    "Все материалы",
                    "Медь",
                    "Алюминий",
                    "Железо",
                    "Никель"
                };

                        cmbMaterial.Items.AddRange(materials);

                        // Выбираем значения по умолчанию
                        cmbX.SelectedIndex = 0;
                        cmbY.SelectedIndex = 2;
                        cmbMaterial.SelectedIndex = 0;

                        // Создаем кнопку для построения графика
                        var btnCreate = new Button
                        {
                            Text = "Построить график",
                            Location = new Point(150, 130),
                            DialogResult = DialogResult.OK
                        };

                        // Добавляем элементы на форму
                        chartOptionsForm.Controls.Add(lblX);
                        chartOptionsForm.Controls.Add(lblY);
                        chartOptionsForm.Controls.Add(lblMaterial);
                        chartOptionsForm.Controls.Add(cmbX);
                        chartOptionsForm.Controls.Add(cmbY);
                        chartOptionsForm.Controls.Add(cmbMaterial);
                        chartOptionsForm.Controls.Add(btnCreate);

                        chartOptionsForm.AcceptButton = btnCreate;

                        // Показываем форму и ждем результат
                        if (chartOptionsForm.ShowDialog() == DialogResult.OK)
                        {
                            string xColumn = cmbX.SelectedItem.ToString();
                            string yColumn = cmbY.SelectedItem.ToString();
                            string selectedMaterial = cmbMaterial.SelectedItem.ToString();

                            // Загружаем данные из Excel для построения графика в отдельном окне
                            List<double> xValues = new List<double>();
                            List<double> yValues = new List<double>();
                            List<string> materialValues = new List<string>();

                            LoadDataFromExcel(filePath, xColumn, yColumn, selectedMaterial, xValues, yValues, materialValues);

                            // Если нет данных, выводим сообщение
                            if (xValues.Count == 0)
                            {
                                MessageBox.Show("Нет данных для построения графика с выбранными параметрами.");
                                return;
                            }

    
                            // Показываем график в отдельном модальном окне
                            ShowChartInModalWindow(xColumn, yColumn, xValues, yValues, selectedMaterial, materialValues);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при построении графика: {ex.Message}");
            }
        }
        private void BTChart_Click(object sender, EventArgs e)
        {
            try
            {
                // Создаем диалог открытия файла
                OpenFileDialog openDialog = new OpenFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Выберите файл Excel с данными"
                };

                // Если пользователь выбрал файл и нажал OK
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openDialog.FileName;

                    // Проверяем, существует ли файл
                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("Файл не найден.");
                        return;
                    }

                    // Создаем форму для выбора параметров графика
                    using (var chartOptionsForm = new Form())
                    {
                        chartOptionsForm.Text = "Параметры графика";
                        chartOptionsForm.Size = new Size(400, 200);
                        chartOptionsForm.StartPosition = FormStartPosition.CenterParent;
                        chartOptionsForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                        chartOptionsForm.MaximizeBox = false;
                        chartOptionsForm.MinimizeBox = false;

                        // Создаем метки и комбобоксы для выбора данных
                        var lblX = new Label { Text = "Ось X:", Location = new Point(20, 20), AutoSize = true };
                        var lblY = new Label { Text = "Ось Y:", Location = new Point(20, 50), AutoSize = true };

                        var cmbX = new ComboBox { Location = new Point(120, 20), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                        var cmbY = new ComboBox { Location = new Point(120, 50), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

                        // Добавляем возможные колонки для выбора
                        string[] columns = {
                    "Высота до (см)",
                    "Диаметр до (см)",
                    "Высота после (см)",
                    "Диаметр после (см)",
                    "Разница диаметра (см)",
                    "Разница высоты (см)"
                };

                        cmbX.Items.AddRange(columns);
                        cmbY.Items.AddRange(columns);

                        // Выбираем значения по умолчанию
                        cmbX.SelectedIndex = 0;
                        cmbY.SelectedIndex = 2;

                        // Создаем кнопку для построения графика
                        var btnCreate = new Button
                        {
                            Text = "Построить график",
                            Location = new Point(150, 100),
                            DialogResult = DialogResult.OK
                        };

                        // Добавляем элементы на форму
                        chartOptionsForm.Controls.Add(lblX);
                        chartOptionsForm.Controls.Add(lblY);
                        chartOptionsForm.Controls.Add(cmbX);
                        chartOptionsForm.Controls.Add(cmbY);
                        chartOptionsForm.Controls.Add(btnCreate);

                        chartOptionsForm.AcceptButton = btnCreate;

                        // Показываем форму и ждем результат
                        if (chartOptionsForm.ShowDialog() == DialogResult.OK)
                        {
                            string xColumn = cmbX.SelectedItem.ToString();
                            string yColumn = cmbY.SelectedItem.ToString();

                            // Создаем график в Excel
                            CreateExcelChart(filePath, xColumn, yColumn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при построении графика: {ex.Message}");
            }
        }
        private void CreateExcelChart(string filePath, string xColumn, string yColumn)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet dataSheet = null;
            Excel.Worksheet chartSheet = null;

            try
            {
                // Запускаем Excel и открываем файл
                excelApp = new Excel.Application();
                excelApp.Visible = true; // Делаем Excel видимым для пользователя
                workbook = excelApp.Workbooks.Open(filePath);
                dataSheet = (Excel.Worksheet)workbook.Worksheets[1];

                // Находим индексы колонок для X и Y
                int xColumnIndex = -1;
                int yColumnIndex = -1;
                int lastColumn = dataSheet.UsedRange.Columns.Count;

                for (int i = 1; i <= lastColumn; i++)
                {
                    string headerValue = Convert.ToString((dataSheet.Cells[1, i] as Excel.Range).Value2);
                    if (headerValue == xColumn)
                        xColumnIndex = i;
                    if (headerValue == yColumn)
                        yColumnIndex = i;
                }

                if (xColumnIndex == -1 || yColumnIndex == -1)
                {
                    MessageBox.Show("Не удалось найти указанные колонки в файле.");
                    return;
                }

                // Создаем новый лист для графика
                chartSheet = (Excel.Worksheet)workbook.Worksheets.Add(After: dataSheet);
                chartSheet.Name = $"График {yColumn} от {xColumn}";

                // Определяем диапазон данных
                int lastRow = dataSheet.UsedRange.Rows.Count;

                // Создаем график
                Excel.ChartObjects chartObjects = (Excel.ChartObjects)chartSheet.ChartObjects();
                Excel.ChartObject chartObject = chartObjects.Add(60, 30, 600, 400);
                Excel.Chart chart = chartObject.Chart;

                // Определяем диапазоны данных для осей X и Y
                Excel.Range xRange = dataSheet.Range[dataSheet.Cells[2, xColumnIndex], dataSheet.Cells[lastRow, xColumnIndex]];
                Excel.Range yRange = dataSheet.Range[dataSheet.Cells[2, yColumnIndex], dataSheet.Cells[lastRow, yColumnIndex]];

                // Устанавливаем источник данных для графика
                chart.SetSourceData(yRange);

                // Устанавливаем тип графика (линейный график)
                chart.ChartType = Excel.XlChartType.xlXYScatterSmooth;

                // Настраиваем серию данных
                Excel.Series series = (Excel.Series)chart.SeriesCollection(1);
                series.XValues = xRange;

                // Настраиваем оси
                chart.HasTitle = true;
                chart.ChartTitle.Text = $"Зависимость {yColumn} от {xColumn}";

                chart.Axes(Excel.XlAxisType.xlCategory).HasTitle = true;
                chart.Axes(Excel.XlAxisType.xlCategory).AxisTitle.Text = xColumn;

                chart.Axes(Excel.XlAxisType.xlValue).HasTitle = true;
                chart.Axes(Excel.XlAxisType.xlValue).AxisTitle.Text = yColumn;

                // Добавляем легенду
                chart.HasLegend = true;
                chart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionBottom;

                // Сохраняем изменения
                workbook.Save();

                // Выводим сообщение об успешном создании графика
                MessageBox.Show($"График зависимости {yColumn} от {xColumn} успешно создан на отдельном листе.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании графика в Excel: {ex.Message}");
            }
            finally
            {
                // Не закрываем Excel, чтобы пользователь мог работать с графиком
                // Но освобождаем COM-объекты
                if (chartSheet != null) Marshal.ReleaseComObject(chartSheet);
                if (dataSheet != null) Marshal.ReleaseComObject(dataSheet);
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);
            }
        }

        private void BTExport_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, что расчёт был выполнен (поля вывода не пустые)
                if (string.IsNullOrEmpty(tBHeigReturn.Text) || string.IsNullOrEmpty(tBOsnReturn.Text))
                {
                    MessageBox.Show("Сначала выполните расчёт деформации.");
                    return;
                }

                // Создаем диалог сохранения файла
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Сохранить результаты в Excel",
                    DefaultExt = "xlsx",
                    FileName = "DeformationResults.xlsx"
                };

                // Если пользователь выбрал файл и нажал OK
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveDialog.FileName;

                    // Собираем данные для экспорта
                    double origHeight = double.Parse(textBox2.Text);
                    double origOsnov = double.Parse(textBox1.Text);
                    double deformHeight = double.Parse(tBHeigReturn.Text);
                    double deformOsnov = double.Parse(tBOsnReturn.Text);
                    string material = cBMaterials.SelectedItem.ToString();

                    // Определяем тип фигуры
                    string figureType = "";
                    if (radioCylinder.Checked) figureType = "Цилиндр";
                    else if (radioKonus.Checked) figureType = "Конус";
                    else if (radioParrall.Checked) figureType = "Параллелепипед";
                    else if (radioTreug.Checked) figureType = "Треугольная призма";

                    // Вычисляем разницу размеров
                    double diameterDifference = Math.Abs(origOsnov - deformOsnov);
                    double heightDifference = Math.Abs(origHeight - deformHeight);

                    Excel.Application excelApp = null;
                    Excel.Workbook workbook = null;
                    Excel.Worksheet worksheet = null;

                    try
                    {
                        excelApp = new Excel.Application();
                        excelApp.Visible = false; // Не показываем Excel пользователю

                        // Проверяем, существует ли файл
                        if (System.IO.File.Exists(filePath))
                        {
                            workbook = excelApp.Workbooks.Open(filePath);
                        }
                        else
                        {
                            workbook = excelApp.Workbooks.Add();
                        }

                        worksheet = (Excel.Worksheet)workbook.Worksheets[1];

                        // Находим последнюю заполненную строку
                        int lastRow = 1;
                        try
                        {
                            lastRow = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row + 1;
                        }
                        catch
                        {
                            // Если файл пустой, начинаем с первой строки
                            lastRow = 1;
                        }

                        // Если файл новый, добавляем заголовки
                        if (lastRow == 1)
                        {
                            worksheet.Cells[1, 1] = "Высота до (см)";
                            worksheet.Cells[1, 2] = "Диаметр до (см)";
                            worksheet.Cells[1, 3] = "Высота после (см)";
                            worksheet.Cells[1, 4] = "Диаметр после (см)";
                            worksheet.Cells[1, 5] = "Материал";
                            worksheet.Cells[1, 6] = "Фигура";
                            worksheet.Cells[1, 7] = "Разница диаметра (см)";
                            worksheet.Cells[1, 8] = "Разница высоты (см)";
                            lastRow++;
                        }

                        // Записываем данные в новую строку
                        worksheet.Cells[lastRow, 1] = origHeight;
                        worksheet.Cells[lastRow, 2] = origOsnov;
                        worksheet.Cells[lastRow, 3] = deformHeight;
                        worksheet.Cells[lastRow, 4] = deformOsnov;
                        worksheet.Cells[lastRow, 5] = material;
                        worksheet.Cells[lastRow, 6] = figureType;
                        worksheet.Cells[lastRow, 7] = diameterDifference;
                        worksheet.Cells[lastRow, 8] = heightDifference;

                        // Автоматически подгоняем ширину столбцов
                        worksheet.Columns.AutoFit();

                        // Сохраняем файл
                        workbook.SaveAs(filePath);
                        workbook.Close();
                        excelApp.Quit();

                        MessageBox.Show($"Данные успешно сохранены в файл: {filePath}");
                    }
                    finally
                    {
                        // Освобождаем ресурсы COM-объектов
                        if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                        if (workbook != null) Marshal.ReleaseComObject(workbook);
                        if (excelApp != null) Marshal.ReleaseComObject(excelApp);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Excel: {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void bTCalc_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка выбора материала
                if (cBMaterials.SelectedIndex == -1)
                {
                    MessageBox.Show("Пожалуйста, выберите материал.");
                    return;
                }

                TypeOfMetal m;
                switch (cBMaterials.SelectedIndex)
                {
                    case 0:
                        m = TypeOfMetal.Cu;
                        break;
                    case 1:
                        m = TypeOfMetal.Al;
                        break;
                    case 2:
                        m = TypeOfMetal.Fe;
                        break;
                    case 3:
                        m = TypeOfMetal.Ni;
                        break;
                    default:
                        throw new Exception("Некорректный выбор материала.");
                }

                // Проверка корректности ввода диаметра
                if (!double.TryParse(textBox1.Text, out double diametr) || diametr <= 0)
                {
                    MessageBox.Show("Пожалуйста, введите корректное положительное значение диаметра.");
                    return;
                }

                // Проверка корректности ввода высоты
                if (!double.TryParse(textBox2.Text, out double height) || height <= 0)
                {
                    MessageBox.Show("Пожалуйста, введите корректное положительное значение высоты.");
                    return;
                }

                // Проверка выбора фигуры
                if (!radioCylinder.Checked && !radioKonus.Checked && !radioParrall.Checked && !radioTreug.Checked)
                {
                    MessageBox.Show("Пожалуйста, выберите тип фигуры.");
                    return;
                }

                AbstractFigure dump;
                if (radioCylinder.Checked)
                    dump = new Cylinder(height, diametr, m);
                else if (radioParrall.Checked)
                    dump = new Parrallelepiped(height, diametr, m);
                else if (radioTreug.Checked)
                    dump = new TrianglePrism(height, diametr, m);
                else
                    dump = new Cone(height, diametr, m);

                dump.Calc();

                tBHeigReturn.Text = dump.deformHeight.ToString();
                tBOsnReturn.Text = dump.deformOsnov.ToString();

                dump.Draw(pictureBox1, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}