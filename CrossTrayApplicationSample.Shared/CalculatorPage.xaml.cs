using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossTrayApplicationSample.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculatorPage : ContentPage
    {
        public CalculatorPage()
        {
            InitializeComponent();
        }

        private Func<double, double, double> _operation;
        private double _currentInput;
        private double _currentValue;
        private bool _newOperator;
        private bool _inputReset = true;

        private void OnClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new ContentPage());
            var button = (Button)sender;
            switch (button.Text)
            {
                // number inputs
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case ",":
                    if (_newOperator)
                    {
                        _currentInput = 0;
                        _newOperator = false;
                        _inputReset = false;
                    }

                    if (button.Text == ",")
                    {
                        if (!Value.Text.Contains(","))
                            Value.Text = _currentInput + ",";
                        if (_currentInput == 0)
                            Value.Text = "0,";
                    }
                    else
                    {
                        var numberInput = button.Text;
                        if (Value.Text.EndsWith(","))
                        {
                            numberInput = Value.Text + numberInput;
                        }
                        else if(_currentInput != 0)
                        {
                            numberInput = $"{Math.Round(_currentInput, 0)}{button.Text},{_currentInput - Math.Round(_currentInput, 0)}";
                        }
                        
                        _currentInput = double.Parse(numberInput);
                        Value.Text = FormatToReasonableLength(_currentInput);
                        if (_inputReset)
                        {
                            _inputReset = false;
                            _currentValue = _currentInput;
                            TotalValue.Text = FormatToReasonableLength(_currentValue);
                        }
                    } 
                    break;
                // Operators
                case "+":
                    SetOperation((input, value) => input + value, "+");
                    break;
                case "-":
                    SetOperation((input, value) => input - value, "-");
                    break;
                case "X":
                    SetOperation((input, value) => input * value, "X");
                    break;
                case "/":
                    SetOperation((input, value) => input == 0 ? 0 : input / value, "/");
                    break;
                // calculate
                case "=":
                    ApplyLastOperation();
                    _operation = null;
                    _inputReset = true;
                    TotalValue.Text = string.Empty;
                    break;
                // Reset
                case "C": 
                    _currentValue = 0;
                    _currentInput = 0;
                    Value.Text = "0";
                    TotalValue.Text = string.Empty;
                    _operation = null;
                    _inputReset = true;
                    break;
            }
        }

        /// <summary>
        /// When a new operation is chosen:
        /// * The last operation gets executed
        /// * A flag gets set, that a new operation was chosen and the next input will replace the current shown value
        /// * The operator character gets added to the total value
        /// </summary> 
        private void SetOperation(Func<double, double, double> operation, string operatorChar)
        {
            ApplyLastOperation();
            _operation = operation;
            _newOperator = true;
            TotalValue.Text = $"{FormatToReasonableLength(_currentValue)} {operatorChar}";
        }

        /// <summary>
        /// Applies the last operation:
        /// * Sets the current value to the result of the calculation
        /// * Sets the current input to the new current value in case there is no new input before the next operator execution -> this gets overwritten with the next input
        /// * Updates the value and total value label text
        /// </summary>
        private void ApplyLastOperation()
        {
            if (_operation == null) return;

            _currentValue = _operation.Invoke(_currentInput, _currentValue);
            _currentInput = _currentValue;
            Value.Text = FormatToReasonableLength(_currentValue);
            TotalValue.Text = FormatToReasonableLength(_currentValue);
        }

        /// <summary>
        /// Format the number to a length that should be visible without word wrapping by using scientific notation when 15 characters are reached.
        /// </summary> 
        private string FormatToReasonableLength(double value)
        {
            return value.ToString().Length <= 15
                ? value.ToString()
                : value.ToString("e2");
        }
    }
}