using LaYumba.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using Unit = System.ValueTuple;


namespace Exercises.Chapter7
{
   static class Exercises
   {
      public static void Do() {
         Func<string, string, string> personalizedGreeting = (greet, name) =>
            $"{greet} {name}";
         string[] names = {"Tristan", "Ivan"};
         names.Map(n => personalizedGreeting("Hello", n)).ForEach(n => n.write());
         
         Remainder(-10, -3).write();
         Remainder2(2).write();

         LogF(Level.Info, "wtf1");
         LogInfo("wtf2");
         "wtf3".Info();
         new List<string> {"wtf", "wtf2"}.Map(str => $"a {str}").ForEach(s => s.write());
         new List<string> {"wtf", "wtf2"}.Where(str => str == "wtf").ForEach(s => s.write());
         new List<string> {"wtf", "wtf2"}.Bind(str => Enumerable.Range(0, 10).Map(i => $"{str}{i}")).ForEach(s => s.write());
      }

      static Func<int, int, int> Remainder => (divident, divisor) => divisor % divident;
      static Func<int, int> Remainder2 => Remainder.ApplyR(5);
      
      //(Func<T, K, R>, K) => Func<T, R>
      static Func<T, R> ApplyR<T, K, R>(this Func<T, K, R> f, K k) =>
         t => f(t, k);
      
      static Func<T1, T2, R> ApplyR<T1, T2, T3, R>(this Func<T1, T2, T3, R> f, T3 t3) =>
         (t1, t2) => f(t1, t2, t3);

      // 1. Partial application with a binary arithmethic function:
      // Write a function `Remainder`, that calculates the remainder of 
      // integer division(and works for negative input values!). 

      // Notice how the expected order of parameters is not the
      // one that is most likely to be required by partial application
      // (you are more likely to partially apply the divisor).

      // Write an `ApplyR` function, that gives the rightmost parameter to
      // a given binary function (try to write it without looking at the implementation for `Apply`).
      // Write the signature of `ApplyR` in arrow notation, both in curried and non-curried form

      // Use `ApplyR` to create a function that returns the
      // remainder of dividing any number by 5. 

      // Write an overload of `ApplyR` that gives the rightmost argument to a ternary function

      public enum NumberType { home, mobile }
      
      class PhoneNumber {
         NumberType numberType;
         CountryCode countryCode;
         int number;

         public PhoneNumber(NumberType numberType, CountryCode countryCode, int number) {
            this.numberType = numberType;
            this.countryCode = countryCode;
            this.number = number;
         }
      }

      class CountryCode {
         CountryCode(string value) => Value = value;
         public static implicit operator string(CountryCode code) => code.Value;
         public static implicit operator CountryCode(string code) => new CountryCode(code);
         public override string ToString() => Value;
         string Value { get; }
      }

      static Func<CountryCode, NumberType, int, PhoneNumber> CreateNumber =>
         (code, type, number) => new PhoneNumber(type, code, number);

      static Func<NumberType, int, PhoneNumber> CreateUkNumber =>
         CreateNumber.Apply((CountryCode)"uk");

      static Func<int, PhoneNumber> CreateUkMobile() =>
         CreateUkNumber.Apply(NumberType.mobile);
      // 2. Let's move on to ternary functions. Define a class `PhoneNumber` with 3
      // fields: number type(home, mobile, ...), country code('it', 'uk', ...), and number.
      // `CountryCode` should be a custom type with implicit conversion to and from string.

      // Now define a ternary function that creates a new number, given values for these fields.
      // What's the signature of your factory function? 

      // Use partial application to create a binary function that creates a UK number, 
      // and then again to create a unary function that creates a UK mobile

      static Func<Level, string, Unit> LogF => (level, message) =>
         $"[{level}] {message}".write();

      static Func<string, Unit> LogInfo => LogF.Apply(Level.Info);

      static Func<R> Apply<T, R>(this Func<T, R> func, T t)
         => () => func(t);
      
      static Unit Info(this string message) => LogInfo.Apply(message)();

      delegate void Log(Level level, string message);

      static void Info(this Log log, string message) =>
         $"[{Level.Info}] {message}".write();
      
      static void Debug(this Log log, string message) =>
         $"[{Level.Debug}] {message}".write();
      
      static void Error(this Log log, string message) =>
         $"[{Level.Error}] {message}".write();
      
      static Action<Level, string> Log3 = (level, message) =>  
         $"[{level}] {message}".write();

      static void Info3(this Action<Level, string> log, string message) =>
         log(Level.Info, message);

      // 3. Functions everywhere. You may still have a feeling that objects are ultimately 
      // more powerful than functions. Surely, a logger object should expose methods 
      // for related operations such as Debug, Info, Error? 
      // To see that this is not necessarily so, challenge yourself to write 
      // a very simple logging mechanism without defining any classes or structs. 
      // You should still be able to inject a Log value into a consumer class/function, 
      // exposing operations like Debug, Info, and Error, like so:

      static void ConsumeLog(Log log) 
         => log.Info("look! no objects!");
      
      static void ConsumeLog(Action<Level, string> log) 
         => log.Info3("look! no objects!");

      static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> map) =>
         ts.Aggregate(Enumerable.Empty<R>(), (rs, t) => rs.Append(map(t)));

      static IEnumerable<T> Where<T>(this IEnumerable<T> ts, Func<T, bool> p) =>
         ts.Aggregate(Enumerable.Empty<T>(), (rs, t) => p(t) ? rs.Append(t) : rs);

      static IEnumerable<R> Bind<T, R>(this IEnumerable<T> ts, Func<T, IEnumerable<R>> bind) =>
         ts.Aggregate(Enumerable.Empty<R>(), (rs, t) => rs.Concat(bind(t)));

      enum Level { Debug, Info, Error }
   }
}
