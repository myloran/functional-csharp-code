using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;
using LaYumba.Functional.Data.LinkedList;
using static LaYumba.Functional.Data.LinkedList.LinkedList;
using LaYumba.Functional.Data.BinaryTree;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using static LaYumba.Functional.Data.BinaryTree.Tree;
using Unit = System.ValueTuple;
//using static LaYumba.Functional.F;

namespace Exercises.Chapter9
{
   static class Exercises
   {
//      public static void Test() {
//         Func<int, int> @double = i => i * 2;
//         Some(3).Map(@double).write();
//
//         Func<int, Func<int, int>> multiply = x => y => x * y;
//         var multBy3 = Some(3).Map(multiply).write();
//
//         Func<int, int, int> m = (x, y) => x * y;
//         Some(3).Map(m).Apply(Some(2)).write();
//
//         Some(m)
//            .Apply(None)
//            .Apply(Some(2)).write();
//
//         Enumerable.Range(1, 100)
//            .Where(i => i % 20 == 0)
//            .OrderBy(i => -i)
//            .Select(i => $"{i}%")
//            .ForEach(i => i.write());
//
//         var linq = from i in Enumerable.Range(1, 100)
//            where i % 20 == 0
//            orderby -i
//            select $"{i}%".write();
//
//         var enumerable = from x in Enumerable.Range(1, 4)
//            select (x * 2).write();
//         enumerable.ToList();
//      }
      public static void Do() {
//         Test();
         var empty = List<string>();
         var letters = List("a", "b");
         var taxi = List("c", letters);

         var fruits = List("pineapple", "pear", "banana", "pectin");
         var tropicalMix = fruits.Add("kiwi");
         var yellowFruits = fruits.Add("lemon");
         var fruits2 = fruits.InsertAt("sdf", 1);
         var fruits3 = fruits.RemoveAt(1);
         var fruits4 = fruits.TakeWhile(f => f.StartsWith("p"));
         var fruits5 = fruits.DropWhile(f => f.StartsWith("p"));
         var fruits6 = fruits.AsEnumerable().TakeWhile(f => f.StartsWith("p"));
         var fruits7 = fruits.AsEnumerable().DropWhile(f => f.StartsWith("p"));
         fruits7.ForEach(f => f.write());

         var tree = Tree("wtf", List(
            Tree("wtf2"),
            Tree("wtf3"),
            Tree("wtf4")));
         
         var translations = new Dictionary<string, string> {
            {"wtf", "ok"},
            {"wtf2", "ok2"},
            {"wtf3", "ok3"},
            {"wtf4", "ok4"},
         };

         Func<string, string> translate = label => translations[label];
         
         tree.Map(translate).ToString().write();
      }

      static LaYumba.Functional.Data.LinkedList.List<T> InsertAt<T>(this LaYumba.Functional.Data.LinkedList.List<T> list, T item, int index) =>
         index == 0 ? List(item, list) 
            : list.Match(
               () => throw new Exception(),
               (t, ts) => List(t, ts.InsertAt(item, index - 1)));
      
      // LISTS

      // Implement functions to work with the singly linked List defined in this chapter:
      // Tip: start by writing the function signature in arrow-notation

      // InsertAt inserts an item at the given index

      static LaYumba.Functional.Data.LinkedList.List<T> RemoveAt<T>(this LaYumba.Functional.Data.LinkedList.List<T> list, int index) =>
         list.Match(
            () => throw new Exception(),
            (t, ts) => index == 0 ? ts : List(t, ts.RemoveAt(index - 1)));

      // RemoveAt removes the item at the given index

      static LaYumba.Functional.Data.LinkedList.List<T> TakeWhile<T>(this LaYumba.Functional.Data.LinkedList.List<T> list, Func<T, bool> p) =>
         list.Match(
            () => List<T>(),
            (t, ts) => p(t) ? List(t, ts.TakeWhile(p)): List<T>());

      // TakeWhile takes a predicate, and traverses the list yielding all items until it find one that fails the predicate

      static LaYumba.Functional.Data.LinkedList.List<T> DropWhile<T>(this LaYumba.Functional.Data.LinkedList.List<T> list, Func<T, bool> p) =>
         list.Match(
            () => List<T>(),
            (t, ts) => p(t) ? ts.DropWhile(p) : List(t, ts));

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
      
      static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> ts, Func<T, bool> p) {
         foreach (var t in ts)
            if (p(t)) yield return t;
            else yield break;
      }
            
      static IEnumerable<T> DropWhile<T>(this IEnumerable<T> ts, Func<T, bool> p) {
         var b = true;
         foreach (var t in ts) {
            if (b && !p(t)) b = false;
            if (!b) yield return t;
         }
      }

      // TakeWhile and DropWhile are useful when working with a list that is sorted 
      // and you’d like to get all items greater/smaller than some value; write implementations 
      // that take an IEnumerable rather than a List

      static Tree<R> Bind<T, R>(this Tree<T> tree, Func<T, Tree<R>> bind) =>
         tree.Match(
            bind,
            (l, r) => Branch(l.Bind(bind), r.Bind(bind)));

      struct LabelTree<T> {
         public T Label { get; }
         public LaYumba.Functional.Data.LinkedList.List<LabelTree<T>> Subtrees { get; }

         public LabelTree(T label, LaYumba.Functional.Data.LinkedList.List<LabelTree<T>> subtrees) {
            Label = label;
            Subtrees = subtrees;
         }

         public override string ToString() => $"{Label} {SubTreesToString}";
         public override bool Equals(object other) => ToString() == other.ToString();

         string SubTreesToString => Subtrees.Match(() => "", (t, ts) => $"[{string.Join(",", List(t, ts).AsEnumerable())}]");
      }
      
//      Dictionary<string, LaYumba.Functional.Data.LinkedList.List<T>> dict

      static LabelTree<R> Map<T, R>(this LabelTree<T> tree, Func<T, R> map) =>
         new LabelTree<R>(map(tree.Label), tree.Subtrees.Map(s => s.Map(map)));
      
      static LabelTree<T> Tree<T>(T label, LaYumba.Functional.Data.LinkedList.List<LabelTree<T>> subtrees = null) =>
         new LabelTree<T>(label, subtrees ?? List<LabelTree<T>>());
         

//      static LabelTree<R> Map<T, R>(this LabelTree<T> tree, Func<T, R> map) =>
//         tree.subtrees.Match(
//            () => List<R>(),
//            (t, ts) => List(map(t), ts.Map(map)));


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

      [Test]
      public static void LabelTreeTest() {
         //arrange
         var tree = Tree("wtf", List(
            Tree("wtf2"),
            Tree("wtf3", List(
               Tree("what"),
               Tree("what2"),
               Tree("what3"))),
            Tree("wtf4")));
         
         var translations = new Dictionary<string, string> {
            {"wtf", "ok"},
            {"wtf2", "ok2"},
            {"wtf3", "ok3"},
            {"wtf4", "ok4"},
            {"what", "not"},
            {"what2", "not2"},
            {"what3", "not3"},
         };

         //act
         var actual = tree.Map(label => translations[label]);
         
         //assert
         var expected = Tree("ok", List(
            Tree("ok2"),
            Tree("ok3", List(
               Tree("not"),
               Tree("not2"),
               Tree("not3"))),
            Tree("ok4")));
         
         Assert.AreEqual(expected, actual);
      }
   }
}