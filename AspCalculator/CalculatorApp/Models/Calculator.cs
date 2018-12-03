using System;

namespace CalculatorApp.Models
{
    public class Calculator
    {
        public double LeftOperand { get; set; }

        public double RightOperand { get; set; }

        public string Operator { get; set; }

        public double Result { get; set; }

        public void CalculateResult()
        {
            switch (this.Operator)
            {
                case "+":
                    this.Result = this.LeftOperand + this.RightOperand;
                    break;
                case "-":
                    this.Result = this.LeftOperand - this.RightOperand;
                    break;
                case "*":
                    this.Result = this.LeftOperand * this.RightOperand;
                    break;
                case "/":
                    this.Result = this.LeftOperand / this.RightOperand;
                    break;
                case "^":
                    this.Result = Math.Pow(this.LeftOperand, this.RightOperand);
                    break;
            }
        }
    }
}
