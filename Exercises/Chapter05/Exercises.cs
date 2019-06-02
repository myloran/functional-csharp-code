using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Examples.Chapter3;

namespace Exercises.Chapter5
{
   public static class Exercises
   {
      //(IEnurable<T>, T => R) => IOrderedEnumerable<T>
      //(IEnurable<T>, int) => IEnurable<T>
      //(IEnurable<T>, T => R) => IEnurable<R>
      //IEnurable<Decimal> => Decimal
      // 1. Without looking at any code or documentation (or intllisense), write the function signatures of
      // `OrderByDescending`, `Take` and `Average`, which we used to implement `AverageEarningsOfRichestQuartile`:
      static decimal AverageEarningsOfRichestQuartile(List<Person> population)
         => population
            .OrderByDescending(p => p.Earnings)
            .Take(population.Count/4)
            .Select(p => p.Earnings)
            .Average();

      // 2 Check your answer with the MSDN documentation: https://docs.microsoft.com/
      // en-us/dotnet/api/system.linq.enumerable. How is Average different?

      public static Func<T, K> Compose<T, R, K>(this Func<T, R> first, Func<R, K> second)
         => t => second(first(t));

      // 3 Implement a general purpose Compose function that takes two unary functions
      // and returns the composition of the two.
   }
}
