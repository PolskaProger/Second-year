using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public static class Calculate
    {
        public static double Calculation(double Num1, double Num2, string operation )
        {
            double ans = 0;
            switch (operation)
            {
                case "/":
                    ans= Num1/Num2;
                    break;
                case "*":
                    ans= Num1*Num2;
                    break;
                case "+":
                    ans= Num1+Num2;
                    break;
                case "-":
                    ans= Num1-Num2;
                    break;
                case "x^y":
                    ans= Math.Pow(Num1,Num2);
                    break;
                default:
                    break;

            }

            return ans;
        } 
    }
}
