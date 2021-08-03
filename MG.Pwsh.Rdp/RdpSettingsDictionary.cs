using MG.Pwsh.Rdp.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace MG.Pwsh.Rdp
{
    public class RdpSettingsDictionary : IEnumerable
    {
        private OrderedDictionary _nameDict;
        private Hashtable _dict;

        public object this[string key]
        {
            get
            {
                key = this.GetRealKey(key);

                return _dict.Contains(key)
                    ? _dict[key]
                    : null;
            }
            set
            {
                key = this.GetRealKey(key);
                if (_dict.Contains(key) && this.TypesMatch(key, value))
                    _dict[key] = value;
            }
        }

        public RdpSettingsDictionary(int capacity)
        {
            _dict = new Hashtable(capacity, StringComparer.CurrentCultureIgnoreCase);
            _nameDict = new OrderedDictionary(capacity, StringComparer.CurrentCulture);
        }

        public void Add(string displayKey, string key, object value)
        {
            _nameDict.Add(displayKey, key);
            _dict.Add(key, value);
        }

        public IEnumerable GetNames()
        {
            foreach (var kvp in _nameDict)
            {
                yield return kvp;
            }
        }

        public string GetRealKey(string possible)
        {
            if (!_dict.Contains(possible) && _nameDict.Contains(possible))
                possible = (string)_nameDict[possible];

            return possible;
        }
        public string GetKeyFromDisplayName(string displayName)
        {
            return _nameDict.Contains(displayName)
                ? (string)_nameDict[displayName] 
                : null;
        }
        private bool TypesMatch(string key, object newValue)
        {
            if (null != newValue)
                return this[key].GetType().Equals(newValue.GetType());

            else
                return false;
        }

        public void Write(RdpWriter writer)
        {
            for (int i = 0; i < _dict.Count; i++)
            {
                writer.WriteSetting(_nameDict[i]);
                writer.WriteValue(_dict[_nameDict[i]]);
            }
        }

        public IEnumerator GetEnumerator() => _dict.GetEnumerator();
    }
}
