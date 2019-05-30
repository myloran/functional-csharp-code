using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Rest.Serialization;

namespace Exercises.Chapter1
{                     
   //function that takes function => iterator apply, conditional apply
   //function that takes function and returns function => adapter
   //function that returns function => factory
   //function that takes function => hole in the middle
   static class Exercises
   {
      public static void Do() {
         Func<int, bool> p = a => true;
         NegatePredicate(p);

//         Func<int, int, bool> isOk = (a, b) => a < b;
//         var isWrong = isOk.SwapArgs();
      }

      static Func<T, bool> NegatePredicate<T>(Func<T, bool> predicate) => t => !predicate(t);//1
      
      static List<T> QuickSort<T>(List<T> list) {
         var result = new List<T>(list);
         result.Sort();
         return result;
      }
      
      static List<T> QuickSort<T>(List<T> list, Comparison<T> comparison) {
         var result = new List<T>(list);
         result.Sort(comparison);
         return result;
      }
      
//      algorithm quicksort(A) is
//         if A is empty
//      return A
//         pivot := A.pop() 
//      lA := A.filter(where e < pivot) (создать массив с элементами меньше опорного)
//      rA := A.filter(where e > pivot) (создать массив с элементами больше опорного)
//      return quicksort(lA) + [pivot] + quicksort(rA) (вернуть массив состоящий из отсортированной левой части, опорного и отсортированной правой части)
      
      static List<int> QuickSort2(List<int> list) {
         if (list.Count == 0) return list;
         
         var pivot = list[0];
         var rest = list.Skip(1).ToList();
         
         var left = rest.Where(e => e < pivot).ToList();
         var right = rest.Where(e => e > pivot).ToList();
         
         return QuickSort2(left)
            .Append(pivot)
            .Concat(QuickSort2(right))
            .ToList();
      }
      
      static List<T> QuickSort2<T>(List<T> list, Comparison<T> comparison) {
         if (list.Count == 0) return list;
         
         var pivot = list[0];
         var rest = list.Skip(1).ToList();
         
         var left = rest.Where(e => comparison(e, pivot) < 0).ToList();
         var right = rest.Where(e => comparison(e, pivot) > 0).ToList();
         
         return QuickSort2(left, comparison)
            .Append(pivot)
            .Concat(QuickSort2(right, comparison))
            .ToList();
      }

      static T Using<T, TDisp>(Func<TDisp> getDisposable, Func<TDisp, T> use) where TDisp : IDisposable {
         using (var disposable = getDisposable())
            return use(disposable);
      }

      static Func<T2, T1, R> SwapArgs<T1, T2, R>(this Func<T1, T2, R> f)
         => (t2, t1) => f(t1, t2);
      // 1. Write a function that negates a given predicate: whenvever the given predicate
      // evaluates to `true`, the resulting function evaluates to `false`, and vice versa.

      // 2. Write a method that uses quicksort to sort a `List<int>` (return a new list,
      // rather than sorting it in place).

      // 3. Generalize your implementation to take a `List<T>`, and additionally a 
      // `Comparison<T>` delegate.

      // 4. In this chapter, you've seen a `Using` function that takes an `IDisposable`
      // and a function of type `Func<TDisp, R>`. Write an overload of `Using` that
      // takes a `Func<IDisposable>` as first
      // parameter, instead of the `IDisposable`. (This can be used to fix warnings
      // given by some code analysis tools about instantiating an `IDisposable` and
      // not disposing it.)
   }
}
