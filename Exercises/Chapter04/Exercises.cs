using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Exercises.Chapter3;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using static System.Console; 
using String = LaYumba.Functional.String;

namespace Exercises.Chapter4
{
   static class Exercises
   {
      public static void Do() {
         Option<string> name = Some("Enrico");
         
         name.Map(String.ToUpper)
            .ForEach(WriteLine);

         IEnumerable<string> names = new[] {"Constantz", "Albert"};
         names.Map(String.ToUpper)
            .ForEach(WriteLine);

         new HashSet<string>().Map(String.ToUpper).FirstOrDefault().write();
         new HashSet<string>{"john"}.Map(String.ToUpper).FirstOrDefault().write();

         new Dictionary<string, string>().Map(String.ToUpper).FirstOrDefault().write();
         new Dictionary<string, string>{{"John", "Robinson"}}.Map(String.ToUpper).FirstOrDefault().write();
      }

      //(ISet<T>, T => R) => ISet<R> 
      public static ISet<R> Map<T, R>(this ISet<T> set, Func<T, R> map) {
         var result = new HashSet<R>();
         foreach (var s in set)
            result.Add(map(s));
         return result;
      }
      
      //(IDictionary<K, T>, (K, T) => (K2, T2)) => IDictionary<K2, T2>
      public static IDictionary<K2, T2> FullMap<K, T, K2, T2>(this IDictionary<K, T> dict, 
         Func<K, K2> mapKey, Func<T, T2> mapValue) 
      {
         var result = new Dictionary<K2, T2>();
         foreach (var pair in dict)
            result[mapKey(pair.Key)] = mapValue(pair.Value);
         return result;
      }
      
      //(IDictionary<K, T>, T => R) => IDictionary<K, R>
      public static IDictionary<K, R> Map<K, T, R>(this IDictionary<K, T> dict, 
         Func<T, R> map) 
      {
         var result = new Dictionary<K, R>();
         foreach (var pair in dict)
            result[pair.Key] = map(pair.Value);
         return result;
      }
      
      public static Option<R> Map<T, R>(Option<T> opt, Func<T, R> map) 
         => opt.Bind(t => Some(map(t)));

      public static IEnumerable<R> Map<T, R>(IEnumerable<T> ts, Func<T, R> map)
         => ts.Bind(t => List(map(t))); 

      // 1 Implement Map for ISet<T> and IDictionary<K, T>. (Tip: start by writing down
      // the signature in arrow no   tation.)

      // 2 Implement Map for Option and IEnumerable in terms of Bind and Return.

      // 3 Use Bind and an Option-returning Lookup function (such as the one we defined
      // in chapter 3) to implement GetWorkPermit, shown below. 

      // Then enrich the implementation so that `GetWorkPermit`
      // returns `None` if the work permit has expired.

      static Option<WorkPermit> GetWorkPermit(Dictionary<string, Employee> people, string employeeId) 
         => people.Lookup(employeeId).Bind(e => e.WorkPermit);
      
      static Option<WorkPermit> GetValidWorkPermit(Dictionary<string, Employee> people, string employeeId) 
         => people.Lookup(employeeId)
            .Bind(e => e.WorkPermit)
            .Where(HasExpired);

      static Func<WorkPermit, bool> HasExpired => w => w.Expiry < DateTime.Today.Date;  

      // 4 Use Bind to implement AverageYearsWorkedAtTheCompany, shown below (only
      // employees who have left should be included).

      static double AverageYearsWorkedAtTheCompany(List<Employee> employees) {
//         employees.Where(e => e.LeftOn.Match(() => false, l => true))
//            .Map(e => e.LeftOn - e.JoinedOn);
         return employees.Bind(e => e.LeftOn.Map(leftOn => YearsBetween(leftOn, e.JoinedOn)))
            .Average();
      }

      static double YearsBetween(DateTime leftOn, DateTime joinedOn) 
         => (leftOn - joinedOn).TotalDays / 365;
   }

   public struct WorkPermit
   {
      public string Number { get; set; }
      public DateTime Expiry { get; set; }
   }

   public class Employee
   {
      public string Id { get; set; }
      public Option<WorkPermit> WorkPermit { get; set; }

      public DateTime JoinedOn { get; }
      public Option<DateTime> LeftOn { get; }
   }
}