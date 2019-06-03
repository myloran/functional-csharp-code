using System;
using System.Security.Cryptography;
using LaYumba.Functional;
using LaYumba.Functional.Option;
using static LaYumba.Functional.F;
using String = System.String;
using Unit = System.ValueTuple;

namespace Exercises.Chapter8 {
  static class Exercises {
    public static void Do() {
      Func<int, int, int> m = (x, y) => x * y;
      Either<string, Func<int, int, int>> eitherMultiplier = m;
      var multiplyBy = eitherMultiplier.Apply(5);
      multiplyBy.Map(f => f(4)).write();

      var exceptionalMultiplier = Exceptional(m.Apply(5));
      var multiplyBy2 = exceptionalMultiplier.Apply(4);
      multiplyBy2.write();

      var s = from i in (Either<string, int>)Right(5)
        select i.write();
      s.ToString();
      
      var s2 = from i in Exceptional(5)
        select i.write();
      s2.ToString();

      Func<int, int> ret = a => a; 
      Some(ret)
        .Apply(4).write();
      
      Right(5)
        .Bind(Add)
        .Bind(Add);

      var either = from i in (Either<string, int>)Right(5)
        from j in Add(i)
        from y in Add2(i, j)
        from z in Add2(j, y)
        select (j, y, z);

      either.ToString().write();
    }

    static Either<L, R2> Select<L, R, R2>(this Either<L, R> either, Func<R, R2> map) =>
      either.Map(map);

    static Either<L, R2> SelectMany<L, R, R2>(this Either<L, R> either, Func<R, Either<L, R2>> bind) =>
      either.Bind(bind);
    
    static Either<L, R3> SelectMany<L, R, R2, R3>(this Either<L, R> either, Func<R, Either<L, R2>> bind, Func<R, R2, R3> project) =>
      either.Bind(r => bind(r).Bind<L, R2, R3>(r2 => project(r, r2)));

    static Exceptional<R> Select<T, R>(this Exceptional<T> exceptional, Func<T, R> map) => 
      exceptional.Map(map); 

    static Either<L, Func<T2, R>> Apply<T1, T2, L, R>(this Either<L, Func<T1, T2, R>> either, Either<L, T1> t1) =>
      either.Bind(f => t1.Map(f.Apply));

    static Exceptional<R> Apply<T, R>(this Exceptional<Func<T, R>> exceptional, Exceptional<T> t) =>
      exceptional.Match(
        e1 => t.Match(
          e2 => e2, 
          v2 => e1),
        f => t.Match(
          e2 => e2, 
          v2 => Exceptional(f(v2))));

    static Func<int, Either<string, int>> Add => a => a == 0 ? (Either<string, int>) Left("no") : Right(a);
    static Func<int, int, Either<string, int>> Add2 => (a, b) => a == 0 ? (Either<string, int>) Left("no") : Right(a + b);
    static Func<int, int, Either<string, int>> Subtract => (a, b) => a - b;
    
    class CookFavouriteDish
    {
      Func<Either<Reason, Unit>> WakeUpEarly;
      Func<Unit, Either<Reason, Ingredients>> ShopForIngredients;
      Func<Ingredients, Either<Reason, Food>> CookRecipe;

      Action<Food> EnjoyTogether;
      Action<Reason> ComplainAbout;
      Action OrderPizza;

      void Start()
      {
        WakeUpEarly()
          .Bind(ShopForIngredients)
          .Bind(CookRecipe)
          .Match(
            Right: dish => EnjoyTogether(dish),
            Left: reason =>
            {
              ComplainAbout(reason);
              OrderPizza();
            });
        
        var a = from i in WakeUpEarly()
          from j in ShopForIngredients(i)
          from y in CookRecipe(j)
          select y;
        
        a.Match(
          reason => {
            ComplainAbout(reason);
            OrderPizza();
          },
          dish => EnjoyTogether(dish));
      }
    }

    class Reason { }
    class Ingredients { }
    class Food { }
  }
}