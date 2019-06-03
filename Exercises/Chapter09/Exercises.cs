using NUnit.Framework;
using System;
using System.Linq;
using LaYumba.Functional;
using LaYumba.Functional.Data.LinkedList;
using static LaYumba.Functional.Data.LinkedList.LinkedList;
using LaYumba.Functional.Data.BinaryTree;
using static LaYumba.Functional.Data.BinaryTree.Tree;
using Unit = System.ValueTuple;
using static LaYumba.Functional.F;

namespace Exercises.Chapter9
{
   static class Exercises
   {
      public static void Do() {
         Func<int, int> @double = i => i * 2;
         Some(3).Map(@double).write();

         Func<int, Func<int, int>> multiply = x => y => x * y;
         var multBy3 = Some(3).Map(multiply).write();

         Func<int, int, int> m = (x, y) => x * y;
         Some(3).Map(m).Apply(Some(2)).write();

         Some(m)
            .Apply(None)
            .Apply(Some(2)).write();

         Enumerable.Range(1, 100)
            .Where(i => i % 20 == 0)
            .OrderBy(i => -i)
            .Select(i => $"{i}%")
            .ForEach(i => i.write());

         var linq = from i in Enumerable.Range(1, 100)
            where i % 20 == 0
            orderby -i
            select $"{i}%".write();

         var enumerable = from x in Enumerable.Range(1, 4)
            select (x * 2).write();
         enumerable.ToList();
      } 
      // LISTS

      // Implement functions to work with the singly linked List defined in this chapter:
      // Tip: start by writing the function signature in arrow-notation

      // InsertAt inserts an item at the given index

      // RemoveAt removes the item at the given index

      // TakeWhile takes a predicate, and traverses the list yielding all items until it find one that fails the predicate

      // DropWhile works similarly, but excludes all items at the front of the list


      // complexity:
      // InsertAt: 
      // RemoveAt: 
      // TakeWhile: 
      // DropWhile: 

      // number of new objects required: 
      // InsertAt: 
      // RemoveAt: 
      // TakeWhile: 
      // DropWhile: 

      // TakeWhile and DropWhile are useful when working with a list that is sorted 
      // and you’d like to get all items greater/smaller than some value; write implementations 
      // that take an IEnumerable rather than a List


      // TREES

      // Is it possible to define `Bind` for the binary tree implementation shown in this
      // chapter? If so, implement `Bind`, else explain why it’s not possible (hint: start by writing
      // the signature; then sketch binary tree and how you could apply a tree-returning funciton to
      // each value in the tree).

      // Implement a LabelTree type, where each node has a label of type string and a list of subtrees; 
      // this could be used to model a typical navigation tree or a cateory tree in a website

      // Imagine you need to add localization to your navigation tree: you're given a `LabelTree` where
      // the value of each label is a key, and a dictionary that maps keys
      // to translations in one of the languages that your site must support
      // (hint: define `Map` for `LabelTree` and use it to obtain the localized navigation/category tree)

   }
}