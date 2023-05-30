using System.Collections;
using System.Collections.Generic;
using TD.StatusSystem;
using UnityEngine;


namespace TD.Utilities
{
    public static class Utils
    {
        /// <summary>
        /// Creates a dictionary based on a list of keys and its corresponding ordered list of values
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <returns>Returns a dictionary with keys of type T1 and values of type T2</returns>
        public static Dictionary<T1,T2> CreateDictionary<T1,T2>(List<T1> keys, List<T2> values)
        {
            Dictionary<T1, T2> dictionary = new Dictionary<T1, T2>();
            int indexInValuesListToLinkToAKey = 0;
            for (int i = 0; i < keys.Count; i++)
            {
                int j = 0;

                bool hasReachedEndOfValuesList = j >= keys.Count;
                while (!hasReachedEndOfValuesList)
                {
                    bool hasFoundIndexOfValueInListToLinkToAKey = j == indexInValuesListToLinkToAKey == true ? true : false;
                    if (!hasFoundIndexOfValueInListToLinkToAKey)
                    {
                        j++;
                        continue;
                    }
                    dictionary.Add(keys[i], values[indexInValuesListToLinkToAKey]);
                    indexInValuesListToLinkToAKey++;
                    break;
                }
            }
            return dictionary;
        }
    }
}
