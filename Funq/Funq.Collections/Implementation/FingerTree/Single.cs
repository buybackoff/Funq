using System;
using System.Collections.Generic;
using Funq.Collections.Common;

namespace Funq.Collections.Implementation
{
	static partial class FingerTree<TValue>
	{
		internal abstract partial class FTree<TChild>
		{
			internal sealed class Single : FTree<TChild>
			{
				public Digit CenterDigit;

				public Single(Digit centerDigit, Lineage lineage)
					: base(centerDigit.Measure, TreeType.Single, lineage, 1)
				{
					CenterDigit = centerDigit;
				}

				public override Leaf<TValue> this[int index]
				{
					get
					{
						var r = CenterDigit[index];
						return r;
					}
				}

				public override bool IsFragment
				{
					get
					{
						return CenterDigit.IsFragment;
					}
				}

				public override TChild Left
				{
					get
					{
						return CenterDigit.Left;
					}
				}

				public override TChild Right
				{
					get
					{
						return CenterDigit.Right;
					}
				}

				private FTree<TChild> _mutate(Digit digit)
				{
					CenterDigit = digit;
					Measure = digit.Measure;
					return this;
				}

				public FTree<TChild> MutateOrCreate(Digit digit, Lineage lineage)
				{
					return Lineage.AllowMutation(lineage) ? _mutate(digit) : new Single(digit, lineage);
				}

				public override FTree<TChild> AddFirst(TChild item, Lineage lineage)
				{
					FTree<TChild> ret;
#if ASSERTS
					var expected = Measure + item.Measure;
#endif
					if (CenterDigit.Size < 4)
					{
						ret = MutateOrCreate(CenterDigit.AddFirst(item, lineage), lineage);
					}
					else
					{
						var leftmost = new Digit(item, CenterDigit.First, lineage);
						var rightmost = CenterDigit.RemoveFirst(lineage);
						ret = new Compound(leftmost, FTree<Digit>.Empty, rightmost, lineage);
					}
#if ASSERTS
					ret.Measure.Is(expected);
					ret.Left.Is(item);
#endif
					return ret;

				}

				public override FTree<TChild> AddLast(TChild item, Lineage lineage)
				{
#if ASSERTS
					var expected = Measure + item.Measure;
#endif
					FTree<TChild> ret;
					if (CenterDigit.Size < 4)
					{
						ret = new Single(CenterDigit.AddLast(item, lineage), lineage);
					}
					else
					{
						var rightmost = new Digit(CenterDigit.Fourth, item, lineage);
						var leftmost = CenterDigit.RemoveLast(lineage);

						ret = new Compound(leftmost, FTree<Digit>.Empty, rightmost, lineage);
					}
#if ASSERTS
					ret.Measure.Is(expected);
					ret.Right.Is(item);
#endif
					return ret;

				}

				public override FTree<TChild> RemoveFirst(Lineage lineage)
				{
					FTree<TChild> ret;
#if ASSERTS
					var expected = Measure - Left.Measure;
					var expected_first = Measure > 1 ? this.CenterDigit.Second : null;
#endif
					if (CenterDigit.Size > 1)
					{
						var newDigit = CenterDigit.RemoveFirst(lineage);
						ret =  MutateOrCreate(newDigit, lineage);
					}
					else
					{
						ret = Empty;
					}
#if ASSERTS
					ret.Measure.Is(expected);
					if (expected_first != null) ret.Left.Is(expected_first);
#endif
					return ret;
				}

				public override FTree<TChild> RemoveLast(Lineage lineage)
				{
					if (CenterDigit.Size > 1)
					{
						var newDigit = CenterDigit.RemoveLast(lineage);
						return MutateOrCreate(newDigit, lineage);
					}
					return Empty;
				}

				public override FTree<TChild> Insert(int index, Leaf<TValue> leaf, Lineage lineage)
				{
					Digit leftmost, rightmost;
					CenterDigit.Insert(index, leaf, out leftmost, out rightmost, lineage);
					if (rightmost == null)
					{
						return MutateOrCreate(leftmost, lineage);
					}
					return new Compound(leftmost, FTree<Digit>.Empty, rightmost, lineage);
				}

				public override void Iter(Action<Leaf<TValue>> action)
				{
					CenterDigit.Iter(action);
				}

				public override void IterBack(Action<Leaf<TValue>> action)
				{
					CenterDigit.IterBack(action);
				}

				public override bool IterBackWhile(Func<Leaf<TValue>, bool> func)
				{
					return CenterDigit.IterBackWhile(func);
				}

				public override bool IterWhile(Func<Leaf<TValue>, bool> func)
				{
					return CenterDigit.IterWhile(func);
				}

				public override FTree<TChild> Remove(int index, Lineage lineage)
				{
					if (CenterDigit.Measure == 1 && index == 0)
					{
						return EmptyTree.Instance;
					}
#if ASSERTS
					CenterDigit.IsFragment.Is(false);
#endif

					var res = CenterDigit.Remove(index, lineage);
					if (res == null)
					{
						return Empty;
					}
					return MutateOrCreate(res, lineage);
				}

				public override FTree<TChild> Reverse(Lineage lineage)
				{
					return new Single(CenterDigit.Reverse(lineage), lineage);
				}

				public override void Split(int count, out FTree<TChild> leftmost, out FTree<TChild> rightmost, Lineage lineage)
				{
					Digit left_digit;
					Digit right_digit;
					if (count == 0) {
						leftmost = Empty;
						rightmost = this;
					}
					if (count == Measure) {
						leftmost = this;
						rightmost = Empty;
					}
					CenterDigit.Split(count, out left_digit, out right_digit, lineage);
					leftmost = left_digit != null ? new Single(left_digit, lineage) : Empty;
					rightmost = right_digit != null ? new Single(right_digit, lineage) : Empty;
				}

				public override FTree<TChild> Update(int index, Leaf<TValue> leaf, Lineage lineage)
				{
					return new Single(CenterDigit.Update(index, leaf, lineage), lineage);
				}

				public override FingerTreeElement GetChild(int index) {
					if (index != 0) throw Errors.Arg_out_of_range("index");
					return CenterDigit;
				}
			}
		}
	}
}