using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace Exercises.Chapter2
{
   // 1. Write a console app that calculates a user's Body-Mass Index:
   //   - prompt the user for her height in metres and weight in kg
   //   - calculate the BMI as weight/height^2
   //   - output a message: underweight(bmi<18.5), overweight(bmi>=25) or healthy weight
   // 2. Structure your code so that structure it so that pure and impure parts are separate
   // 3. Unit test the pure parts
   // 4. Unit test the impure parts using the HOF-based approach

   public static class Bmi {
      public enum BmiRange { Underweight, Overweight, NormalWeight}
      
      static readonly Func<string, double> Read = message => {
         Console.Out.WriteLine(message);
         return double.Parse(Console.In.ReadLine());
      };
      
      static readonly Action<BmiRange> Write = message => Console.Out.WriteLine(message);
      
      public static void Main() {
         Run(Read, Write);
      }

      static void Run(Func<string, double> read, Action<BmiRange> write) {
         //input
         var height = read("height");
         var weight = read("weight");
         
         //computation
         var bmi = CalculateBMI(height, weight).ToBmiRange();
         
         //output
         write(bmi);
      }

      static double CalculateBMI(double height, double weight) => weight / Math.Pow(height, 2);
      
      static BmiRange ToBmiRange(this double bmi) 
         => bmi < 18.5f ? BmiRange.Underweight
            : bmi >= 25 ? BmiRange.Overweight
            : BmiRange.NormalWeight;
      
      [Test]
      public static void TestCalculateBMI() {
         var actualBmi = CalculateBMI(2, 100);
         var expectedBmi = 25;
         Assert.AreEqual(expectedBmi, actualBmi);
      }
      
      [TestCase(15, ExpectedResult = BmiRange.Underweight)]
      [TestCase(20, ExpectedResult = BmiRange.NormalWeight)]
      [TestCase(30, ExpectedResult = BmiRange.Overweight)]
      public static BmiRange TestConfigureMessage(double bmi) => bmi.ToBmiRange();
      
      [TestCase(2, 100, ExpectedResult = BmiRange.Overweight)]
      public static BmiRange TestRun(double height, double weight) {
         var result = default(BmiRange);
         Func<string, double> read = message => message == "height" ? height : weight;
         Action<BmiRange> write = message => result = message;
         
         Run(read, write);
         return result;
      }
   }
}
