using System;
using LaYumba.Functional;

namespace Exercises.Chapter10 {
  public class Exercises {
    public static void Do() {
      var rand = new Random();

      T Pick<T>(T l, T r) => rand.NextDouble() < 0.5 ? l : r;

      Pick(1 + 2, 3 + 4).write();

      T Pick2<T>(Func<T> l, Func<T> r) => (rand.NextDouble() < 0.5 ? l : r)();

      Pick2(() => 1 + 2, () => 3 + 4).write();

      Func<string> lazyGrandma = () => {
        "getting grandma".write();
        return "grandma";
      };
      Func<string, string> turnBlue = s => {
        "turning blue".write();
        return $"blue {s}";
      };
      Func<string> lazyGrandmaBlue = lazyGrandma.Map(turnBlue);
      lazyGrandmaBlue().write();
    }
  }
}