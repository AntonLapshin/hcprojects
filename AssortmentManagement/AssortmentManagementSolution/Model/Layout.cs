using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AssortmentManagement.Model
{
    [Serializable]
    public class Layout : ISerializable
    {
        public SortedList<int, LayoutField> ColumnArea { get; set; }
        public SortedList<int, LayoutField> RowArea { get; set; }
        public SortedList<int, LayoutField> DataArea { get; set; }
        public SortedList<int, LayoutField> FilterArea { get; set; }

        public Layout()
        {
            ColumnArea = new SortedList<int, LayoutField>();
            RowArea = new SortedList<int, LayoutField>();
            DataArea = new SortedList<int, LayoutField>();
            FilterArea = new SortedList<int, LayoutField>();
        }

        public Layout(SerializationInfo info, StreamingContext context)
        {
            ColumnArea = (SortedList<int, LayoutField>)info.GetValue("ColumnArea", typeof(SortedList<int, LayoutField>));
            RowArea = (SortedList<int, LayoutField>)info.GetValue("RowArea", typeof(SortedList<int, LayoutField>));
            DataArea = (SortedList<int, LayoutField>)info.GetValue("DataArea", typeof(SortedList<int, LayoutField>));
            FilterArea = (SortedList<int, LayoutField>)info.GetValue("FilterArea", typeof(SortedList<int, LayoutField>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ColumnArea", ColumnArea);
            info.AddValue("RowArea", RowArea);
            info.AddValue("DataArea", DataArea);
            info.AddValue("FilterArea", FilterArea);
        }

        public byte[] SaveToArray()
        {
            Stream s = new MemoryStream();
            var bFormatter = new BinaryFormatter();
            bFormatter.Serialize(s, this);

            var bytes = ((MemoryStream)s).ToArray();
            return bytes;
        }

        public void LoadFromArray(byte[] bytes)
        {
            Stream s = new MemoryStream(bytes);
            var bFormatter = new BinaryFormatter();
            var layout = (Layout)bFormatter.Deserialize(s);

            ColumnArea = layout.ColumnArea;
            RowArea = layout.RowArea;
            DataArea = layout.DataArea;
            FilterArea = layout.FilterArea;
        }
    }
}
