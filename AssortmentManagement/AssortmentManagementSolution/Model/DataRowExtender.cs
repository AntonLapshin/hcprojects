using System;
using System.Data;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Model
{
    public static class DataRowExtender
    {
        public static string GetItem(this DataRow row)
        {
            const string param = "ITEM";
            return row[param] == DBNull.Value ? Item.None : row[param].ToString();
        }

        public static int GetLoc(this DataRow row)
        {
            const string param = "LOC";
            return row[param] == DBNull.Value ? Loc.None : Convert.ToInt32(row[param]);
        }

        public static LocTypes GetLocType(this DataRow row)
        {
            const string param = "DIM_LOC_TYPE";
            return row[param] == DBNull.Value ? LocTypes.None : (LocTypes)Convert.ToChar(row[param]);
        }

        public static string GetItemType(this DataRow row)
        {
            const string param = "DIM_ITEM_TYPE";
            return row[param] == DBNull.Value ? ItemTypes.None : row[param].ToString();
        }

        public static Actions GetAction(this DataRow row)
        {
            const string param = "ACTION";
            return row[param] == DBNull.Value ? Actions.None : (Actions)Convert.ToInt32(row[param]);
        }

        public static OrderPlaces GetOrderPlace(this DataRow row)
        {
            const string param = "DIM_ITEMLOC_ORDERPLACE";
            return row[param] == DBNull.Value ? OrderPlaces.None : (OrderPlaces)Convert.ToInt32(row[param]);
        }

        public static OrderPlaces GetOrderPlaceNew(this DataRow row)
        {
            const string param = "DIM_ITEMLOC_ORDERPLACE_NEW";
            return row[param] == DBNull.Value ? OrderPlaces.None : (OrderPlaces)Convert.ToInt32(row[param]);
        }

        public static SourceMethods GetSourceMethod(this DataRow row)
        {
            const string param = "DIM_ITEMLOC_SOURCEMETHOD";
            return row[param] == DBNull.Value ? SourceMethods.None : (SourceMethods)Convert.ToChar(row[param]);
        }

        public static SourceMethods GetSourceMethodNew(this DataRow row)
        {
            const string param = "DIM_ITEMLOC_SOURCEMETHOD_NEW";
            return row[param] == DBNull.Value ? SourceMethods.None : (SourceMethods)Convert.ToChar(row[param]);
        }

        public static MeasureStatuses GetMeasureStatus(this DataRow row)
        {
            const string param = "MEASURE_STATUS";
            return row[param] == DBNull.Value ? MeasureStatuses.None : (MeasureStatuses)Convert.ToInt32(row[param]);
        }

        public static MeasureStatuses GetMeasureStatusNew(this DataRow row)
        {
            const string param = "MEASURE_STATUS_NEW";
            return row[param] == DBNull.Value ? MeasureStatuses.None : (MeasureStatuses)Convert.ToInt32(row[param]);
        }
    }
}
