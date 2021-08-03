using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Pwsh.Rdp.Settings
{
    public class OtherSettingsDictionary : Dictionary<string, object>
    {
        #region FIELDS/CONSTANTS


        #endregion

        #region PROPERTIES


        #endregion

        #region CONSTRUCTORS
        public OtherSettingsDictionary()
            : base(StringComparer.CurrentCultureIgnoreCase)
        {
        }
        public OtherSettingsDictionary(int capacity)
            : base(capacity, StringComparer.CurrentCultureIgnoreCase)
        {
        }
        public OtherSettingsDictionary(IDictionary dictionary)
            : this(dictionary.Count)
        {
            foreach (DictionaryEntry de in dictionary)
            {
                if (de.Key is string key)
                {
                    base.Add(key, de.Value);
                }
            }
        }

        #endregion

        #region PUBLIC METHODS
        //public static explicit operator OtherSettingsDictionary(Hashtable hashtable)
        //{
        //    var d = new OtherSettingsDictionary(hashtable.Count);
        //    foreach (DictionaryEntry de in hashtable)
        //    {
        //        if (de.Key is string key)
        //        {
        //            d.Add(key, de.Value);
        //        }
        //    }

        //    return d;
        //}

        #endregion

        #region BACKEND/PRIVATE METHODS


        #endregion
    }
}