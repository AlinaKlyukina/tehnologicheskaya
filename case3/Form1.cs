using System;
using System.Data;
using System.Windows.Forms;

namespace case3
{
    public partial class Form1 : Form
    {
        private double firstNumber = 0;
        private double secondNumber = 0;
        private string operation = "";
        private bool isOperationPerformed = false;
        private bool isCalculated = false;
        private double currentResult = 0;
        private string lastOperator = "";
        private bool isNewOperation = true;
        private double memoryValue = 0;
        private bool isDarkTheme = false;

        public Form1()
        {
            InitializeComponent();
        }

        // Обработчик для всех цифровых кнопок
        private void Button_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "0") || (isOperationPerformed))
                textBox1.Text = "";

            isOperationPerformed = false;
            Button button = (Button)sender;
            textBox1.Text += button.Text;
            isCalculated = false; // Сбрасываем флаг, если продолжаем ввод
        }

        // Обработчик для кнопок операций (+, -, *, /)
        private void Operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            // Если уже есть результат, выполняем расчет перед новой операцией
            if (!isNewOperation)
            {
                Calculate();
            }

            lastOperator = button.Text; // Сохраняем текущий оператор
            firstNumber = Double.Parse(textBox1.Text); // Сохраняем первое число
            currentResult = firstNumber;  // Обновляем текущий результат
            isOperationPerformed = true;
            isNewOperation = false; // Готовы к следующему числу
        }

        // Обработчик для кнопки "="
        private void button16_Click(object sender, EventArgs e)
        {
            if (isCalculated)
            {
                // Если вычисление уже было выполнено, ничего не делаем
                return;
            }

            try
            {
                // Проверяем, что текст в поле не пустой
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    // Проверяем, если строка содержит операцию деления
                    if (textBox1.Text.Contains("/") && textBox1.Text.EndsWith("0"))
                    {
                        textBox1.Text = "Деление на ноль!";
                        return;
                    }

                    // Выполнение вычислений с помощью DataTable
                    var result = new DataTable().Compute(textBox1.Text, null);
                    textBox1.Text = result.ToString();

                    // Устанавливаем флаг, что вычисление выполнено
                    isCalculated = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = ""; // Очищаем текстовое поле или оставляем предыдущий ввод
            }
        }

        // Метод для выполнения расчетов
        private void Calculate()
        {
            // Проверяем, что в текстовом поле есть корректное число
            if (double.TryParse(textBox1.Text, out double secondNumber))
            {
                switch (lastOperator)
                {
                    case "+":
                        currentResult += secondNumber;
                        break;
                    case "-":
                        currentResult -= secondNumber;
                        break;
                    case "*":
                        currentResult *= secondNumber;
                        break;
                    case "/":
                        if (secondNumber == 0)
                        {
                            textBox1.Text = "Деление на ноль!";
                            return;
                        }
                        currentResult /= secondNumber;
                        break;
                    default:
                        // Если оператор не выбран, просто выводим текущее число
                        currentResult = secondNumber;
                        break;
                }

                textBox1.Text = currentResult.ToString(); // Выводим результат
                isNewOperation = true; // Готовы к новой операции
            }
            else
            {
                //MessageBox.Show("Ошибка: некорректный ввод числа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "Некорректный ввод!"; // Очищаем поле ввода или сбрасываем на 0
                return;
            }
        }

        // Обработчик для кнопки "C" (сброс)
        private void ClearButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            firstNumber = 0;
            secondNumber = 0;
            currentResult = 0;
            lastOperator = "";
            isNewOperation = true;
            isCalculated = false;
        }

        // Обработчик для кнопки "Сохранить в память"
        private void buttonSaveToMemory_Click(object sender, EventArgs e)
        {
            // Проверяем, что текст в поле не пустой
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                memoryValue = Double.Parse(textBox1.Text);
            }
        }

        // Обработчик для кнопки "Вставить из памяти"
        private void buttonPasteFromMemory_Click(object sender, EventArgs e)
        {
            textBox1.Text += memoryValue.ToString();
        }

        // Обработчик для кнопки "Очистка памяти"
        private void buttonClearMemory_Click(object sender, EventArgs e)
        {
            memoryValue = 0;
        }

        // Обработчик для кнопки "Изменение цветовой схемы"
        private void buttonChangeColor_Click(object sender, EventArgs e)
        {
            if (!isDarkTheme)
            {
                // Изменение на темную тему
                this.BackColor = System.Drawing.Color.Black; // Цвет фона формы
                textBox1.BackColor = System.Drawing.Color.Black; // Цвет фона текстового поля
                textBox1.ForeColor = System.Drawing.Color.White; // Цвет текста текстового поля

                // Изменение цвета для всех кнопок на форме
                foreach (Control control in this.Controls)
                {
                    if (control is Button)
                    {
                        Button btn = (Button)control;
                        btn.BackColor = System.Drawing.Color.Gray; // Цвет фона кнопок
                        btn.ForeColor = System.Drawing.Color.White; // Цвет текста кнопок
                    }
                }

                isDarkTheme = true; // Устанавливаем, что тема теперь тёмная
            }
            else
            {
                // Возвращение к светлой теме
                this.BackColor = System.Drawing.Color.White; // Цвет фона формы
                textBox1.BackColor = System.Drawing.Color.White; // Цвет фона текстового поля
                textBox1.ForeColor = System.Drawing.Color.Black; // Цвет текста текстового поля

                // Изменение цвета для всех кнопок на светлую тему
                foreach (Control control in this.Controls)
                {
                    if (control is Button)
                    {
                        Button btn = (Button)control;
                        btn.BackColor = System.Drawing.Color.White; // Цвет фона кнопок
                        btn.ForeColor = System.Drawing.Color.Black; // Цвет текста кнопок
                    }
                }

                isDarkTheme = false; // Устанавливаем, что тема теперь светлая
            }
        }
    }
}
