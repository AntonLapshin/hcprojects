using System;
using System.ComponentModel;

namespace AssortmentManagement.UserValues
{
    public static class EnumExtender
    {
        public static string Description(this Enum enumerate)
        {
            var type = enumerate.GetType();
            var fieldInfo = type.GetField(enumerate.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : enumerate.ToString();
        }
    }

    public enum MeasureStatuses
    {
        [Description("В ассортименте")]
        InAssortment = 1,
        [Description("Не в ассортименте")]
        NotInAssortment = 0,
        [Description("Не задан")]
        None = -1,
    }

    public enum OperationModes
    {
        [Description("Тестовый")]
        Test = 2,
        [Description("Рабочий")]
        Production = 1,
        [Description("Не задан")]
        None = 0,
    }

    public enum SessionStates
    {
        [Description("Активный")]
        Active = 2,
        [Description("Неактивный")]
        Inactive = 1,
        [Description("Не задан")]
        None = 0,
    }

    public enum OrderPlaces
    {
        [Description("Поставщик")]
        Supplier = 1,
        [Description("Магазин")]
        Store = 2,
        [Description("Офис")]
        Office = 3,
        [Description("Не задан")]
        None = 0,
    }

    public enum Actions
    {
        [Description("Удалить")]
        Delete = -1,
        [Description("Нет действия")]
        NoAction = 0,
        [Description("Оставить")]
        Leave = 1,
        [Description("Изменение")]
        Modify = 2,
        [Description("Развезти и переключить")]
        Switch = 3,
        [Description("Развезти и закончить")]
        Close = 4,
        [Description("Транзит")]
        Transit = 5,
        [Description("Не задано")]
        None = 5
    }

    public enum SourceMethods
    {
        [Description("W")]
        W = 'W',
        [Description("S")]
        S = 'S',
        [Description("T")]
        T = 'T',
        [Description("Не задан")]
        None = 'N',
    }

    public enum ValueStates
    {
        [Description("Определено")]
        Valid,
        [Description("Не определено")]
        NotValid,
        [Description("Разное")]
        Various
    }
    public enum InputDataTypes
    {
        [Description("Поставщик")]
        Supplier,
        [Description("Место заказа")]
        OrderPlace,
        [Description("Метод поставки")]
        SourceMethod,
        [Description("Склад поставки")]
        SourceWh
    }

    public enum LocTypes
    {
        [Description("Магазин")]
        S = 'S',
        [Description("Склад")]
        W = 'W',
        [Description("Не задан")]
        None = 'N',
    }

    public enum FilterTypes
    {
        [Description("Включено")]
        Included,
        [Description("Исключено")]
        Excluded
    }

    public enum StateTypes
    {
        [Description("Отменить")]
        Undo,
        [Description("Вернуть")]
        Redo
    }

    public enum FormTypes
    {
        [Description("Первая форма")]
        Base,
        [Description("Вторая форма")]
        Secondary
    }

    public enum FormActions
    {
        [Description("Добавить")]
        Add = 0,
        [Description("Удалить")]
        Delete = 1,
        [Description("Изменить")]
        Modify = 2,
        [Description("Отменить изменение")]
        ModifyCancel = 3,
        [Description("Восстановить исходное состояние")]
        Restore = 4
    }

    public enum ExitCodes
    {
        [Description("EXCEPTION")]
        Exception = 'E',
        [Description("SUCCESSFUL")]
        Successful = 'S',
        [Description("NONE")]
        None = 'N',
    }

    public enum InitializeResults
    {
        [Description("Отмена")]
        Cancel = -2,
        [Description("Успешное выполнение")]
        Successful = 0,
        [Description("Ошибка")]
        Error = 1
    }
    public enum CheckStatuses
    {
        [Description("Не задан")]
        None = -3,
        [Description("Отмена")]
        Cancel = -2,
        [Description("Исключение")]
        Exception = -1,
        [Description("Успешное выполнение")]
        Success = 0,
        [Description("Ошибка")]
        Error = 1,
        [Description("Предупреждение")]
        Warning = 2,
        [Description("Идёт выполнение")]
        Executing = 3
    }
    public enum CheckTypes
    {
        [Description("Локальные")]
        Local = 'L',
        [Description("Глобальные")]
        Global = 'G'
    }
    public static class DocTypes
    {
        public static readonly string Regular = "REGULAR";
        public static readonly string Operative = "OPERATIVE";
        public static readonly string ExpendMaterial = "EXPENDMATERIAL";

        public static string Description(string value)
        {
            if (value == Regular) return "Обычный";
            if (value == Operative) return "Оперативный";
            if (value == ExpendMaterial) return "Расходники склад";
            return "Неизвестный";
        }
    }
    public enum LocPermissionTypes
    {
        [Description("Полные права")]
        Full = 'F',
        [Description("Права на управление методом поставки S")]
        Supplier = 'S',
        [Description("Права на управление методом поставки W и T")]
        WarehouseTransit = 'W',
    }
    public static class LogEvents
    {
        public static readonly string Exception = "DB_EXCEPTION";
        public static readonly string UnhandledException = "UNHANDLED_EXCEPTION";
        public static readonly string InitStart = "INIT_START";
        public static readonly string InitEnd = "INIT_END";
    }

    public static class ItemTypes
    {
        public static readonly string Item = "Товар";
        public static readonly string ExpendMaterial = "Расходник";
        public static readonly string None = "Не задан";
    }

    public static class Loc
    {
        public static readonly int None = 0;
    }

    public static class Item
    {
        public static readonly string None = "100000000";
    }
    public enum WhRestExists
    {
        Yes=1,
        No=0
    }
}
