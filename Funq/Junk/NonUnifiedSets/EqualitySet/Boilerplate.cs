﻿

namespace Funq.Collections
{
		using System;
	using System.Collections.Generic;
	using Funq;
	using Funq.Abstract;
	
	public partial class FunqSet<T> : Trait_SetLike<T, FunqSet<T>>
	{
		private FunqSet<TElem2> GetPrototype<TElem2>(IEqualityComparer<TElem2> ph)
		{
			return FunqSet<TElem2>.Empty(ph);
		}
	
		/// <summary>
		/// Applies the specified selector on each element of the collection, 
		/// returning the same kind of collection with a different element time.
		/// </summary>
		/// <typeparam name="TRElem">The type of the output collection element.</typeparam>
		/// <param name="selector">The selector.</param>
		/// <param name="handler">A new equality or comparison handler for constructing the resulting set.</param>
		/// <returns></returns>
		public FunqSet<TRElem> Select<TRElem>(Func<T, TRElem> selector, IEqualityComparer<TRElem> handler = null)
		{
			if (selector == null) throw Errors.Is_null;
			return Select(GetPrototype(handler), selector);
		}
	
		/// <summary>
		/// Applies the specified selector on each element of the collection, discarding all elements for which the selector returns Option.None.
		/// </summary>
		/// <typeparam name="TRElem">The type of the output collection element.</typeparam>
		/// <param name="selector">The selector.</param>
		/// <param name="handler">A new equality or comparison handler for constructing the resulting set.</param>
		/// <returns></returns>
		public FunqSet<TRElem> Select<TRElem>(Func<T, Option<TRElem>> selector, IEqualityComparer<TRElem> handler = null )
		{
			if (selector == null) throw Errors.Is_null;
			return Choose(this.GetPrototype<TRElem>(handler), selector);
		}
	
		/// <summary>
		/// Casts the specified handler.
		/// </summary>
		/// <typeparam name="TRElem">The type of the R elem.</typeparam>
		/// <param name="handler">The handler.</param>
		/// <returns></returns>
		public FunqSet<TRElem> Cast<TRElem>(IEqualityComparer<TRElem> handler = null )
		{
			return Cast<TRElem, FunqSet<TRElem>>(GetPrototype<TRElem>(handler));
		}
	
		/// <summary>
		/// Ofs the type.
		/// </summary>
		/// <typeparam name="TRElem">The type of the R elem.</typeparam>
		/// <param name="handler">The handler.</param>
		/// <returns></returns>
		public FunqSet<TRElem> OfType<TRElem>(IEqualityComparer<TRElem> handler = null) 
		{
			return OfType<TRElem, FunqSet<TRElem>>(GetPrototype<TRElem>(handler));
		}
	
		/// <summary>
		/// Applies the specified accumulator over each element of a collection, constructing a collection from its partial results.
		/// </summary>
		/// <typeparam name="TRElem">The type of the output collection elem.</typeparam>
		/// <param name="initial">The initial value for the accumulator.</param>
		/// <param name="accumulator">The accumulator.</param>
		/// <param name="handler">A new equality or comparison handler for constructing the resulting set.</param>
		/// <returns></returns>
		public FunqSet<TRElem> Scan<TRElem>(TRElem initial, Func<TRElem, T, TRElem> accumulator, IEqualityComparer<TRElem> handler = null )
		{
			if (accumulator == null) throw Errors.Is_null;
			return Scan(GetPrototype<TRElem>(handler), initial, accumulator);
		}
	}
	
		}