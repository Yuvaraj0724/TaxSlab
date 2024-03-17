using System;
using System.Xml.Schema;
using TaxSlab.Interfaces;

namespace TaxCalculatorApp
{
  
    // Base class for Taxpayer
    abstract class Taxpayer
    {
        protected double income;
        protected double homeLoanOrRent;
        protected double investment;

        public abstract void TakeInput();
    }

    // Derived class for IndividualTaxpayer
    // Here ITaxable is a interface and it is inherited.
    class IndividualTaxpayer : Taxpayer, ITaxable
    {
        private string Name;
        private string gender;
        private int age;

        public override void TakeInput()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Tax Calculator");
            Console.WriteLine("========================================");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Persona Details");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            while (true)
            {
                Console.Write("Enter Name: ");
                string inputGender = Console.ReadLine().ToLower();
                if(string.IsNullOrEmpty(inputGender) || string.IsNullOrWhiteSpace(inputGender))
                {
                    Console.WriteLine("Name is required");
                    Console.Write("Enter Name: ");
                    inputGender = Console.ReadLine().ToLower();
                }
                else
                {
                    Name = inputGender;
                    break;
                }
            }
            while (true)
            {
                Console.Write("Enter gender (Male/Female): ");
                string inputGender = Console.ReadLine().ToLower();

                if (inputGender == "male" || inputGender == "female")
                {
                    gender = inputGender;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter 'Male' or 'Female'.");
                }
            }
            while (true)
            {
                Console.Write("Enter age: ");
                int inputAge = Convert.ToInt32(Console.ReadLine());
                if(inputAge > 0)
                {
                    age= inputAge;
                    break;
                }
                else
                {
                    Console.WriteLine("Enter correct age: ");
                }
            }
            
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Account Details");
            Console.WriteLine("----------------------------------------");


            Console.Write("Enter income: ");
            income = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter investment amount: ");
            investment = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter home loan/house rent amount: ");
            homeLoanOrRent = Convert.ToDouble(Console.ReadLine());

           
        }

        public void CalculateTax()
        {
            double taxableIncome = income;
            double homeLoanExemption = Math.Min(0.8 * homeLoanOrRent, 0.2 * income);
            taxableIncome -= homeLoanExemption;

            double investmentExemption = Math.Min(investment, 100000);
            taxableIncome -= investmentExemption;

            double totalTax = 0;

            // Applying tax slabs based on gender and age
            double[][] taxSlabs;
            if (gender == "male")
            {
                taxSlabs = new double[][] { new double[] { 160000, 0 }, new double[] { 300000, 0.1 }, new double[] { 500000, 0.2 } };
                if (age >= 60)
                {
                    taxSlabs = new double[][] { new double[] { 240000, 0 }, new double[] { 300000, 0.1 }, new double[] { 500000, 0.2 } };
                }
            }
            else
            {
                taxSlabs = new double[][] { new double[] { 190000, 0 }, new double[] { 300000, 0.1 }, new double[] { 500000, 0.2 } };
                if (age >= 60)
                {
                    taxSlabs = new double[][] { new double[] { 240000, 0 }, new double[] { 300000, 0.1 }, new double[] { 500000, 0.2 } };
                }
            }
           
            foreach (var slab in taxSlabs)
            {
                if (taxableIncome <= 0)
                {
                    break;
                }

                double amountInSlab = slab[0];
                double taxRate = slab[1];

                double taxOnSlab = Math.Min(taxableIncome, amountInSlab) * taxRate;
                totalTax += taxOnSlab;
                taxableIncome -= amountInSlab;
            }
            Console.WriteLine("");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Tax Calculation Result");
            Console.WriteLine("----------------------------------------");

            Console.WriteLine($"Total taxable income: {income - homeLoanExemption - investmentExemption}");
            Console.WriteLine($"Income tax: {totalTax}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ITaxable taxPayer = new IndividualTaxpayer();
            taxPayer.TakeInput();
            taxPayer.CalculateTax();
        }
    }
}
