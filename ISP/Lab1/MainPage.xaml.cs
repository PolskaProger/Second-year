//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab1
{
    public partial class MainPage : ContentPage
    {
        int currentState = 1;
        string operMath;
        double firstNum, secondNum;
        public MainPage()
        {
            InitializeComponent();
            OnClean(this, null);
        }

        private void OnClean(object sender, EventArgs e)
        {
            firstNum = 0;
            secondNum = 0;
            currentState = 1;
            this.result.Text="0";
        }
        private void OnNumSelection(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string btnPressed = button.Text;
            if (this.result.Text == "0" || currentState < 0)
            {
                this.result.Text = string.Empty;
                if (currentState < 0)
                    currentState *= -1;
            }

            this.result.Text += btnPressed;

            double number;
            if (double.TryParse(this.result.Text, out number))
            {
                this.result.Text = number.ToString("N0");
                if (currentState == 1)
                {
                    firstNum = number;
                }
                else
                {
                    secondNum = number;
                }
            }
        }


        private void OnOperSelection(object sender, EventArgs e)
        {
            currentState = -2;
            Button button = (Button)sender;
            string btnPressed = button.Text;
            operMath =btnPressed;
            this.result.Text += btnPressed;
        }

        private void OnX2(object sender, EventArgs e)
        {
            if (firstNum==0)
                return;
            firstNum=firstNum*firstNum;
            this.result.Text = firstNum.ToString();

        }

        private void Calculator(object sender, EventArgs e)
        {
            double number;
            if (currentState== 2)
            {
                var res = Calculate.Calculation(firstNum, secondNum, operMath);

                this.result.Text = res.ToString();
                firstNum = res;
                currentState = -1;
            }
            currentState = 1;
            if (double.TryParse(this.result.Text, out number))
            {
                this.result.Text = number.ToString("N0");
                firstNum = number;
            }
        }
    }
}
