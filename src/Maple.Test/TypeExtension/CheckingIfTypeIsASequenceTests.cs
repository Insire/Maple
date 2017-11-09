using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public sealed class CheckingIfTypeIsASequenceTests
    {
        [TestMethod]
        public void When_checking_type_of_null()
        {
            var e = Should.Throw<ArgumentNullException>(() =>
            {
                Type nullType = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                nullType.IsSequence(out var sequenceType);
            });

            e.ParamName.ShouldBe("type");
        }

        [TestMethod]
        public void When_checking_type_of_string()
        {
            typeof(string).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.String);
        }

        [TestMethod]
        public void When_checking_a_non_sequence_type()
        {
            typeof(NonSequenceTypeOne).IsSequence(out var sequenceType)
                .ShouldBeFalse();

            sequenceType.ShouldBe(SequenceType.Invalid);
        }

        [TestMethod]
        public void When_checking_a_non_sequence_type_which_implements_an_interface()
        {
            typeof(NonSequenceTypeTwo).IsSequence(out var sequenceType)
                .ShouldBeFalse();

            sequenceType.ShouldBe(SequenceType.Invalid);
        }

        [TestMethod]
        public void When_checking_a_generic_sequence_type()
        {
            typeof(GenericSequenceType).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericCustom);
        }

        [TestMethod]
        public void When_checking_a_non_generic_sequence_type()
        {
            typeof(NonGenericSequenceType).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.Custom);
        }

        [TestMethod]
        public void When_checking_an_array()
        {
            typeof(int[]).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.Array);
        }

        [TestMethod]
        public void When_checking_an_array_list()
        {
            typeof(ArrayList).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.ArrayList);
        }

        [TestMethod]
        public void When_checking_a_queue()
        {
            typeof(Queue).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.Queue);
        }

        [TestMethod]
        public void When_checking_a_stack()
        {
            typeof(Stack).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.Stack);
        }

        [TestMethod]
        public void When_checking_a_bit_array()
        {
            typeof(BitArray).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.BitArray);
        }

        [TestMethod]
        public void When_checking_a_list_dictionary()
        {
            typeof(ListDictionary).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.ListDictionary);
        }

        [TestMethod]
        public void When_checking_a_sorted_list()
        {
            typeof(SortedList).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.SortedList);
        }

        [TestMethod]
        public void When_checking_a_hash_table()
        {
            typeof(Hashtable).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.Hashtable);
        }

        [TestMethod]
        public void When_checking_an_interface_of_ilist()
        {
            typeof(IList).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.IList);
        }

        [TestMethod]
        public void When_checking_an_interface_of_icollection()
        {
            typeof(ICollection).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.ICollection);
        }

        [TestMethod]
        public void When_checking_an_interface_of_idictionary()
        {
            typeof(IDictionary).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.IDictionary);
        }

        [TestMethod]
        public void When_checking_an_interface_of_ienumerable()
        {
            typeof(IEnumerable).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.IEnumerable);
        }

        [TestMethod]
        public void When_checking_an_generic_list()
        {
            typeof(List<int>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericList);
        }

        [TestMethod]
        public void When_checking_a_generic_hash_set()
        {
            typeof(HashSet<int>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericHashSet);
        }

        [TestMethod]
        public void When_checking_a_generic_collection()
        {
            typeof(Collection<int>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericCollection);
        }

        [TestMethod]
        public void When_checking_a_generic_linked_list()
        {
            typeof(LinkedList<int>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericLinkedList);
        }

        [TestMethod]
        public void When_checking_a_generic_stack()
        {
            typeof(Stack<int>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericStack);
        }

        [TestMethod]
        public void When_checking_a_generic_queue()
        {
            typeof(Queue<int>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericQueue);
        }

        [TestMethod]
        public void When_checking_an_interface_of_generic_ilist()
        {
            typeof(IList<string>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericIList);
        }

        [TestMethod]
        public void When_checking_an_interface_of_generic_icollection()
        {
            typeof(ICollection<string>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericICollection);
        }

        [TestMethod]
        public void When_checking_an_interface_of_generic_ienumerable()
        {
            typeof(IEnumerable<string>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericIEnumerable);
        }

        [TestMethod]
        public void When_checking_a_generic_dictionary()
        {
            typeof(Dictionary<int, string>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericDictionary);
        }

        [TestMethod]
        public void When_checking_a_generic_sorted_dictionary()
        {
            typeof(SortedDictionary<int, string>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericSortedDictionary);
        }

        [TestMethod]
        public void When_checking_a_generic_sorted_list()
        {
            typeof(SortedList<int, string>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericSortedList);
        }

        [TestMethod]
        public void When_checking_an_interface_of_generic_idictionary()
        {
            typeof(IDictionary<int, string>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericIDictionary);
        }

        [TestMethod]
        public void When_checking_an_interface_of_generic_icollection_of_key_value_pair()
        {
            typeof(ICollection<KeyValuePair<int, string>>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericICollectionKeyValue);
        }

        [TestMethod]
        public void When_checking_an_interface_of_generic_ienumerable_of_key_value_pair()
        {
            typeof(IEnumerable<KeyValuePair<int, string>>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericIEnumerableKeyValue);
        }

        [TestMethod]
        public void When_checking_a_generic_blocking_collection()
        {
            typeof(BlockingCollection<string>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericBlockingCollection);
        }

        [TestMethod]
        public void When_checking_a_generic_concurrent_bag()
        {
            typeof(ConcurrentBag<string>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericConcurrentBag);
        }

        [TestMethod]
        public void When_checking_a_generic_concurrent_dictionary()
        {
            typeof(ConcurrentDictionary<string, int>).IsSequence(out var sequenceType)
                .ShouldBeTrue();

            sequenceType.ShouldBe(SequenceType.GenericConcurrentDictionary);
        }

        private class NonSequenceTypeOne { }

        private class NonSequenceTypeTwo : ICloneable
        {
            public object Clone()
            {
                throw new NotImplementedException();
            }
        }

        private class GenericSequenceType : IEnumerable<string>
        {
            public IEnumerator<string> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class NonGenericSequenceType : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}