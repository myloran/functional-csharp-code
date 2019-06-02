using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Examples.Chapter4;
//using System.Configuration;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using Enum = System.Enum;
using Unit = System.ValueTuple;

public static class OkwyEx {
   public static Unit write(this object obj) {
      Console.Out.WriteLine(obj);
      return Unit();
   }
}
namespace Exercises.Chapter3
{
   public static class Exercises
   {
//      static readonly Action<object> write = message => Console.Out.WriteLine(message);
      
      public static void Do() {
         Option<string> _ = None;
         Option<string> john = Some("John");

         Greet(None).write();
         Greet(john).write();

         Int.Parse("10").write();
         Int.Parse("hello").write();
         
         new NameValueCollection().Lookup("green").write();
         new Dictionary<string, string>().Lookup("red").write();

         Parse<DayOfWeek>("Friday").write();
         Parse<DayOfWeek>("Freeday").write();

         Lookup(new List<int>(), IsOdd).write();
         Lookup(new List<int>{1}, IsOdd).write();
         new List<int>().Last(IsOdd).write();
         new List<int>{1,3}.Last(IsOdd).write();
//         new List<int>().Lookup(IsOdd).write();
//         new List<int>(1).Lookup(IsOdd).write();
         Email.Of("wrong").write();
         Email.Of("adress@gmail.com").write();
      }
      
      static bool IsOdd(int i) => i % 2 == 1;

      static string Greet(Option<string> greetee)
         => greetee.Match(
            () => "Sorry, Who?",
            name => $"Hello {name}");
      
      enum DayOfWeek { Monday, Friday, Saturday }

      
      public static Option<T> Parse<T>(string s) where T : struct 
         => Enum.TryParse(s, out T t) ? Some(t) : None;

      //(IEnumerable<T>, T => bool) => T
      public static Option<T> Lookup<T>(this IEnumerable<T> ts, Func<T, bool> p) {
         foreach (T t in ts) if (p(t)) return t;
         return None;
//         var result = source.Where(predicate).ToList();
//         return result.Count > 0
//            ? Some(result[0])
//            : None;
      }

      public class Email {
         public static Option<Email> Of(string email) 
            => regex.IsMatch(email)
               ? Some(new Email(email))
               : None;
            
         public static implicit operator string(Email email) => email.Value;
         
         Email(string value) => this.Value = value;

         string Value { get; }
         static readonly Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
      }
      
      public static Option<T> Last<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
         T last = default(T);
         var isFound = false;
         
         foreach (var element in source) {
            if (!predicate(element)) continue;
            
            isFound = true;
            last = element;
         }

         if (isFound)
            return last;
         return None;
      }

      // 1 Write a generic function that takes a string and parses it as a value of an enum. It
      // should be usable as follows:

      // Enum.Parse<DayOfWeek>("Friday") // => Some(DayOfWeek.Friday)
      // Enum.Parse<DayOfWeek>("Freeday") // => None

      // 2 Write a Lookup function that will take an IEnumerable and a predicate, and
      // return the first element in the IEnumerable that matches the predicate, or None
      // if no matching element is found. Write its signature in arrow notation:

      // bool isOdd(int i) => i % 2 == 1;
      // new List<int>().Lookup(isOdd) // => None
      // new List<int> { 1 }.Lookup(isOdd) // => Some(1)

      // 3 Write a type Email that wraps an underlying string, enforcing that it’s in a valid
      // format. Ensure that you include the following:
      // - A smart constructor
      // - Implicit conversion to string, so that it can easily be used with the typical API
      // for sending emails

      // 4 Take a look at the extension methods defined on IEnumerable inSystem.LINQ.Enumerable.
      // Which ones could potentially return nothing, or throw some
      // kind of not-found exception, and would therefore be good candidates for
      // returning an Option<T> instead?
   }

   // 5.  Write implementations for the methods in the `AppConfig` class
   // below. (For both methods, a reasonable one-line method body is possible.
   // Assume settings are of type string, numeric or date.) Can this
   // implementation help you to test code that relies on settings in a
   // `.config` file?
   public class AppConfig
   {
      NameValueCollection source;

//      public AppConfig() : this(ConfigurationManager.AppSettings) { }

      public AppConfig(NameValueCollection source)
      {
         this.source = source;
      }

      public Option<T> Get<T>(string name) 
         => source[name] == null
            ? None
            : Some((T)Convert.ChangeType(source[name], typeof(T)));

      public T Get<T>(string name, T defaultValue)
         => Get<T>(name).Match(
            () => defaultValue,
            value => value);
   }
}
