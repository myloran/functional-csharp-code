using LaYumba.Functional;
using static LaYumba.Functional.F;
using System;
using SQLitePCL;

namespace Exercises.Chapter6
{
   static class Exercises
   {
      public static void Do() {
         Right(12).write();
         Left("oops").write();
         Render(Right(12d)).write();
         Render(Left("oops")).write();

         ((Either<string, int>)Right(12)).ToOption().write();
         ((Either<string, int>)Left("wrong")).ToOption().write();
         Some(12).ToEither(() => "wrong").write();
         ((Option<int>)None).ToEither(() => "wrong").write();
         
         Some(3)
            .Bind(F1)
            .Bind(F2).write();

         Some(3).ToEither(() => "wrong")
            .Bind(F11)
            .Bind(F2).write();

         Safely<string, int>(() => throw new Exception(), e => $"error: {e}").write();

         Try<string>(() => throw new Exception()).write();
      }

      static string Render(Either<string, double> val) =>
         val.Match(
            l => $"Invalid value {l}",
            r => $"The result is {r}");

      //Either<L, R> => Option<R>
      static Option<R> ToOption<L, R>(this Either<L, R> either) =>
         either.Match(l => None, Some);

      //(Option<R>, () => L) => Either<L, R>
      static Either<L, R> ToEither<L, R>(this Option<R> opt, Func<L> l) =>
         opt.Match<Either<L, R>>(() => l(), r => r);

      // 1. Write a `ToOption` extension method to convert an `Either` into an
      // `Option`. Then write a `ToEither` method to convert an `Option` into an
      // `Either`, with a suitable parameter that can be invoked to obtain the
      // appropriate `Left` value, if the `Option` is `None`. (Tip: start by writing
      // the function signatures in arrow notation)

      static Option<int> F1(int count) => count;
      static Option<int> F2(int count) => count;
      static Either<string, int> F11(int count) => count;

      static Option<R2> Bind<L, R, R2>(this Option<R> opt, Func<R, Either<L, R2>> bind) =>
         opt.Match(() => None, r => bind(r).ToOption());
      
      static Option<R2> Bind<L, R, R2>(this Either<L, R> either, Func<R, Option<R2>> bind) =>
         either.Match(l => None, bind);


      // 2. Take a workflow where 2 or more functions that return an `Option`
      // are chained using `Bind`.

      // Then change the first one of the functions to return an `Either`.

      // This should cause compilation to fail. Since `Either` can be
      // converted into an `Option` as we have done in the previous exercise,
      // write extension overloads for `Bind`, so that
      // functions returning `Either` and `Option` can be chained with `Bind`,
      // yielding an `Option`.

      static Either<L, R> Safely<L, R>(Func<R> r, Func<Exception, L> l) {
         try { return r(); }
         catch (Exception e) { return l(e); }
      }

      // 3. Write a function `Safely` of type ((() → R), (Exception → L)) → Either<L, R> that will
      // run the given function in a `try/catch`, returning an appropriately
      // populated `Either`.

      static Exceptional<T> Try<T>(Func<T> t) {
         try { return t(); }
         catch (Exception e) { return e; }
      }

      // 4. Write a function `Try` of type (() → T) → Exceptional<T> that will
      // run the given function in a `try/catch`, returning an appropriately
      // populated `Exceptional`.
   }
}
