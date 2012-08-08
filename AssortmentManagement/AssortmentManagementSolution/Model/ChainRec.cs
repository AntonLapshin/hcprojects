using System;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Model
{
    public class ChainRec
    {
        public int Id { get; set; }
        public int Seq { get; set; }
        public string Item { get; set; }
        public int Loc { get; set; }

        public ChainValue SourceMethod { get; set; }
        public ChainValue SourceWh { get; set; }
        public ChainValue SourceMethodNew { get; set; }
        public ChainValue SourceWhNew { get; set; }

        public ChainValue Supplier { get; set; }
        public ChainValue SupplierDesc { get; set; }
        public ChainValue SupplierNew { get; set; }
        public ChainValue SupplierDescNew { get; set; }

        public ChainValue Status { get; set; }
        public ChainValue StatusNew { get; set; }

        public ChainValue Action { get; set; }

        public void Default(string item, int wh)
        {
            Id = 1;
            Seq = 1;
            Item = item;
            Loc = wh;
            SourceMethod = new ChainValue { State = ValueStates.Valid, Value = (char)UserValues.SourceMethods.S };
            SourceWh = new ChainValue { State = ValueStates.Valid, Value = null };
            SourceMethodNew = new ChainValue { State = ValueStates.Valid, Value = (char)UserValues.SourceMethods.S };
            SourceWhNew = new ChainValue { State = ValueStates.Valid, Value = null };
            Supplier = new ChainValue { State = ValueStates.Valid, Value = null };
            SupplierDesc = new ChainValue { State = ValueStates.Valid, Value = null };
            SupplierNew = new ChainValue { State = ValueStates.NotValid, Value = null };
            SupplierDescNew = new ChainValue { State = ValueStates.NotValid, Value = null };
            Status = new ChainValue { State = ValueStates.Valid, Value = 0 };
            StatusNew = new ChainValue { State = ValueStates.Valid, Value = null };
            Action = new ChainValue { State = ValueStates.NotValid, Value = Actions.NoAction };
        }

        public void Join(ChainRec row)
        {
            if (SourceMethod != row.SourceMethod) SourceMethod.State = ValueStates.Various;
            if (SourceWh != row.SourceWh) SourceWh.State = ValueStates.Various;
            if (SourceMethodNew != row.SourceMethodNew) SourceMethodNew.State = ValueStates.Various;
            if (SourceWhNew != row.SourceWhNew) SourceWhNew.State = ValueStates.Various;
            if (Supplier != row.Supplier) Supplier.State = ValueStates.Various;
            if (SupplierDesc.Equals(row.SupplierDesc) == false) SupplierDesc.State = ValueStates.Various;
            if (SupplierNew != row.SupplierNew) SupplierNew.State = ValueStates.Various;
            if (SupplierDescNew.Equals(row.SupplierDescNew) == false) SupplierDescNew.State = ValueStates.Various;
            if (Status != row.Status) Status.State = ValueStates.Various;
            if (StatusNew != row.StatusNew) StatusNew.State = ValueStates.Various;
            if (Action != row.Action) Action.State = ValueStates.Various;
        }

        public bool Check(out string error)
        {
            error = "Узел " + Seq + ": ";
            if (SourceMethodNew.State == ValueStates.NotValid || SourceMethodNew.State == ValueStates.Various)
            {
                error += "Метод поставки не заполнен";
                return false;
            }
            if (Convert.ToChar(SourceMethodNew.Value) == (char)UserValues.SourceMethods.W)
            {
                if (SourceWhNew.State == ValueStates.NotValid || SourceWhNew.State == ValueStates.Various ||
                    SourceWhNew.Value == null)
                {
                    error += "Склад поставки не заполнен";
                    return false;
                }
            }
            if (Convert.ToChar(SourceMethodNew.Value) == (char)UserValues.SourceMethods.T)
            {
                if (SourceWhNew.State == ValueStates.NotValid || SourceWhNew.State == ValueStates.Various ||
                    SourceWhNew.Value == null)
                {
                    error += "Склад транзит не заполнен";
                    return false;
                }
            }
            if (Action.Value.Equals(Actions.NoAction))
            {
                error += "Действие не выбрано";
                return false;
            }
            //if (Action.Value.Equals(Actions.Leave))
            //{
                if (SupplierNew.State == ValueStates.NotValid || SupplierNew.State == ValueStates.Various)
                {
                    error += "Поставщик не выбран";
                    return false;
                }
            //}
            return true;
        }
    }

    /*
        public class ChainRec
        {
            public int Id { get; set; }
            public int Seq { get; set; }
            public string Item { get; set; }
            public int Loc { get; set; }

            public char SourceMethod { get; set; }
            public int SourceWh { get; set; }
            public char SourceMethodNew { get; set; }
            public int SourceWhNew { get; set; }

            public int Supplier { get; set; }
            public string SupplierDesc { get; set; }
            public int SupplierNew { get; set; }
            public string SupplierDescNew { get; set; }

            public int Status { get; set; }
            public int StatusNew { get; set; }

            public int Action { get; set; }

            public void Default(string item, int wh)
            {
                Id = 1;
                Seq = 1;
                Item = item;
                Loc = wh;
                SourceMethod = 'S';
                SourceWh = 0;
                SourceMethodNew = 'S';
                SourceWhNew = -2;
                Supplier = 0;
                SupplierDesc = "";
                SupplierNew = -3;
                SupplierDescNew = "";
                Status = 0;
                StatusNew = -3;
                Action = (int)Actions.NoAction;
            }

            public void Join(ChainRec row)
            {
                if (SourceMethod != row.SourceMethod) SourceMethod = '?';
                if (SourceWh != row.SourceWh) SourceWh = -1;
                if (SourceMethodNew != row.SourceMethodNew) SourceMethodNew = '?';
                if (SourceWhNew != row.SourceWhNew) SourceWhNew = -1;
                if (Supplier != row.Supplier) Supplier = -1;
                if (SupplierDesc.Equals(row.SupplierDesc) == false) SupplierDesc = "?";
                if (SupplierNew != row.SupplierNew) SupplierNew = -1;
                if (SupplierDescNew.Equals(row.SupplierDescNew) == false) SupplierDescNew = "?";
                if (Status != row.Status) Status = -1;
                if (StatusNew != row.StatusNew) StatusNew = -1;
                if (Action != row.Action) Action = -1;
            }

            public bool Check(out string error)
            {
                error = "Узел " + Seq + ": ";
                if (SourceMethodNew == ' ' || SourceMethodNew == '?')
                {
                    error += "Метод поставки не заполнен";
                    return false;
                }
                if (SourceMethodNew == 'W')
                    if (SourceWhNew == -1 || SourceWhNew == -2 || SourceWhNew == 0 || SourceWhNew == -3)
                    {
                        error += "Склад поставки не заполнен";
                        return false;
                    }
                if ((Actions)Action == Actions.NoAction)
                {
                    error += "Действие не выбрано";
                    return false;                
                }
                if ((Actions)Action == Actions.Leave)
                {
                    if (SupplierNew == -3 || SupplierNew == -1)
                    {
                        error += "Поставщик не выбран";
                        return false;                     
                    }
                }
                return true;
            }
        }*/
}
