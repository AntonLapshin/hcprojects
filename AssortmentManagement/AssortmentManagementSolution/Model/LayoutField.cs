using System;
using System.Runtime.Serialization;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Model
{
    [Serializable]
    public class LayoutField : ISerializable
    {
        public string Name { get; set; }
        public FilterTypes Type { get; set; }
        public object[] Values { get; set; }

        public LayoutField(string name, FilterTypes type, object[] values)
        {
            Name = name;
            Type = type;
            Values = values;
        }

        public LayoutField(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Type = (FilterTypes)info.GetValue("Type", typeof(FilterTypes));
            Values = (object[])info.GetValue("Values", typeof(object[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Type", Type);
            info.AddValue("Values", Values);
        }
    }
}