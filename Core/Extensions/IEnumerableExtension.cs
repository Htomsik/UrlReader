using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Extensions;

/// <summary>
///     Extension for IEnumerable tyes
/// </summary>
public static class EnumerableExtension
{
    #region Partition :  Enumerable separator

    /// <summary>
    ///     Enumerable separator
    /// </summary>
    /// <param name="source">Separating source</param>
    /// <param name="size">Size o units</param>
    /// <typeparam name="T">generic some value</typeparam>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> Partition<T>
        (this IEnumerable<T> source, int size)
    {
        T[] array = null;
        int count = 0;
        foreach (T item in source)
        {
            if (array == null)
            {
                array = new T[size];
            }
            array[count] = item;
            count++;
            if (count == size)
            {
                yield return new ReadOnlyCollection<T>(array);
                array = null;
                count = 0;
            }
        }
        if (array != null)
        {             
            Array.Resize(ref array, count);
            yield return new ReadOnlyCollection<T>(array);
        }
    }


    #endregion
    
    #region AsAsync : returned enumerable as IAsyncEnumerable

    /// <summary>
    ///     returned enumerable as IAsyncEnumerable
    /// </summary>
    /// <param name="values">Returned items</param>
    /// <param name="condition">Returned condition for eac item</param>
    /// <typeparam name="TValue">Some generic tye</typeparam>
    public static async IAsyncEnumerable<TValue> AsAsync<TValue>(this IEnumerable<TValue> values,Func<TValue,bool> condition = null)
    {
        condition ??= _ => true;
        
        foreach (var item in values)
            if (condition(item))
            {
                yield return item;
            }
       
    }

    #endregion
}