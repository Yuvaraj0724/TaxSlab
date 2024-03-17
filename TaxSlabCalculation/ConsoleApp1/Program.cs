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
    // Here ITaxable is a interface and it is used for inheritance.
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
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Person Details");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(" ");

            while (true)
            {
                Console.Write("Enter Name: ");
                string inputName = Console.ReadLine();

                if (string.IsNullOrEmpty(inputName) || string.IsNullOrWhiteSpace(inputName))
                {
                    Console.WriteLine("Name is required");
                    Console.Write("Enter Name: ");
                    inputName = Console.ReadLine().ToLower();
                }
                else
                {
                    Name = inputName;
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
            bool validAge = false;
            while (!validAge)
            {
                Console.Write("Enter age: ");

                string inputAge = Console.ReadLine();
                try { 
                    age = Convert.ToInt32(inputAge);
                    if(age > 0)
                    {
                        validAge = true;
                    }
                    else
                    {
                        Console.WriteLine("Age must be greater than zero.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format! Please enter a number for age.");
                }

            }
            
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Account Details");
            Console.WriteLine("----------------------------------------");


            bool validIncome = false;
            while (!validIncome)
            {
                Console.Write("Enter income: ");
                string inputIncome = Console.ReadLine();

                try
                {
                    income = Convert.ToDouble(inputIncome);
                    if (income > 0)
                    {
                        validIncome = true;
                    }
                    else
                    {
                        Console.WriteLine("Income must be greater than zero.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format! Please enter a number for income.");
                }
            }

            bool validInvestment = false;
            while (!validInvestment)
            {
                Console.Write("Enter investment amount: ");
                string inputInvestment = Console.ReadLine();

                try
                {
                    investment = Convert.ToDouble(inputInvestment);
                    if (investment >= 0)
                    {
                        validInvestment = true;
                    }
                    else
                    {
                        Console.WriteLine("Amount must be greater than or equal to zero.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format! Please enter a number for investment.");
                }
            }

            bool validHomeLoanOrRent = false;
            while (!validHomeLoanOrRent)
            {
                Console.Write("Enter home loan/house rent amount: ");
                string inputHomeLoanOrRent = Console.ReadLine();

                try
                {
                    homeLoanOrRent = Convert.ToDouble(inputHomeLoanOrRent);
                    if (homeLoanOrRent >= 0)
                    {
                        validHomeLoanOrRent = true;
                    }
                    else
                    {
                        Console.WriteLine("Amount must be greater than or equal to zero.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format! Please enter a number for home loan/house rent.");
                }
            }

        }

        public void CalculateTax()
        {
            double taxableIncome = income;
            double homeLoanExemption = Math.Min(0.8 * homeLoanOrRent, 0.2 * income);
            taxableIncome -= homeLoanExemption;

            double investmentExemption = Math.Min(investment, 100000);
            taxableIncome -= investmentExemption;

            double totalTax = 0;

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
            Console.ReadLine();
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
