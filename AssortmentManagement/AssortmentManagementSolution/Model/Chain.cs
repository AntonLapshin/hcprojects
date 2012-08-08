using System;
using System.Collections.Generic;
using System.Linq;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Model
{
    public class Chain
    {
        public string Item { get; set; }
        public int Loc { get; set; }
        public int Wh { get; set; }
        public List<ChainRec> Nodes { get; set; }

        public Chain()
        {
            Nodes = new List<ChainRec>();
        }

        public bool Check(out string error)
        {
            error = "";
            foreach (var chainRec in Nodes)
            {
                if (chainRec.Check(out error) == false)
                {
                    return false;
                }
            }
            //if (Nodes[Nodes.Count - 1].SourceMethodNew.Value.Equals("S") == false && Nodes[Nodes.Count - 1].SourceMethodNew.Value.Equals("T") == false)
            //{
            //    error = "Последний узел должен иметь тип поставки 'S' или 'T'";
            //    return false;
            //}
            if ((Convert.ToChar(Nodes[Nodes.Count - 1].SourceMethodNew.Value) == (char)SourceMethods.S) == false)
            {
                error = "Последний узел должен иметь тип поставки 'S'";
                return false;
            }
            return true;
        }

        public void SetSupplier(int seq, int supplier, string supplierDesc)
        {
            for (int i = 0; i <= seq - 1; i++)
            {
                Nodes[i].SupplierNew = new ChainValue(supplier);
                Nodes[i].SupplierDescNew = new ChainValue(supplierDesc);
            }
        }

        public void SetSupplierToOneNode(int seq, int supplier, string supplierDesc)
        {
            Nodes[seq - 1].SupplierNew = new ChainValue(supplier);
            Nodes[seq - 1].SupplierDescNew = new ChainValue(supplierDesc);
        }

        public bool SetMethodS(int index)
        {
            if (0 > index || index > Nodes.Count) return false;
            if (Nodes[index].SourceMethodNew.Value != null)
                if (Convert.ToChar(Nodes[index].SourceMethodNew.Value) == (char)SourceMethods.T) Nodes[index].Action = new ChainValue { State = ValueStates.NotValid, Value = Actions.NoAction };
            Nodes[index].SourceMethodNew = new ChainValue((char)SourceMethods.S);
            Nodes[index].SourceWhNew = new ChainValue { State = ValueStates.Valid, Value = null };
            for (int i = index + 1; i < Nodes.Count; i++)
            {
                Nodes.RemoveAt(i);
                i--;
            }
            return true;
        }

        public void SetMethodW(int seq)
        {
            if (Nodes[seq - 1].SourceMethodNew.Value != null)
                if (Convert.ToChar(Nodes[seq - 1].SourceMethodNew.Value) == (char)SourceMethods.T)
                    Nodes[seq - 1].Action = new ChainValue { State = ValueStates.NotValid, Value = Actions.NoAction };
            Nodes[seq - 1].SourceMethodNew = new ChainValue((char)SourceMethods.W);
            Nodes[seq - 1].SourceWhNew = new ChainValue { State = ValueStates.NotValid, Value = null };
        }

        public void SetMethodT(int seq)
        {
            Nodes[seq - 1].SourceMethodNew = new ChainValue((char)SourceMethods.T);
            Nodes[seq - 1].SourceWhNew = new ChainValue { State = ValueStates.NotValid, Value = null };
            //Nodes[seq - 1].Action = new ChainValue(Actions.Transit);
        }
        /*
                public bool SetT(DbManagerDynamic db, List<ChainRec> buffer, int index, int wh)
                {
                    if (0 > index || index > Nodes.Count) return false;
                    if (Nodes[index].SourceWhNew.State == ValueState.Valid)
                    {
                        if (Nodes[index].SourceWhNew.Value.Equals(wh)) return true;
                    }

                    if (RecExists(Nodes, Nodes[0].Item, wh)) return false;

                    Nodes[index].SourceWhNew = new ChainValue(wh);

                    for (int i = index + 1; i < Nodes.Count; i++)
                    {
                        Nodes.RemoveAt(i);
                        i--;
                    }

                    int loc = wh;
                    while (loc != 0 && loc != -2)
                    {
                        var chainRec = FindRec(buffer, Nodes[0].Item, loc);
                        if (chainRec == null)
                        {
                            var chainRecNew = db.LogisticChainGetRec(Nodes[0].Item, loc);
                            if (chainRecNew == null) break;
                            buffer.Add(chainRecNew);
                            chainRec = chainRecNew;
                        }
                        chainRec.Id = Nodes[0].Id;
                        Nodes.Add(chainRec);
                        loc = Convert.ToInt32(chainRec.SourceWhNew.Value);
                        if (RecExists(Nodes, Nodes[0].Item, loc))
                        {
                            break;
                        }
                    }
                    UpdateSequence();
                    return true;
                }
        */
        public bool SetW(DBManager db, List<ChainRec> buffer, int index, int wh)
        {
            if (0 > index || index > Nodes.Count) return false;
            if (Nodes[index].SourceWhNew != null)
                if (Nodes[index].SourceWhNew.State == ValueStates.Valid)
                {
                    if (Nodes[index].SourceWhNew.Value.Equals(wh)) return true;
                }

            if (RecExists(Nodes, Nodes[0].Item, wh)) return false;

            Nodes[index].SourceWhNew = new ChainValue(wh);

            for (int i = index + 1; i < Nodes.Count; i++)
            {
                Nodes.RemoveAt(i);
                i--;
            }

            int loc = wh;
            while (loc != 0 && loc != -2)
            {
                var chainRec = FindRec(buffer, Nodes[0].Item, loc);
                if (chainRec == null)
                {
                    var chainRecNew = db.LogisticChainGetRec(Nodes[0].Item, loc);
                    if (chainRecNew == null) break;
                    buffer.Add(chainRecNew);
                    chainRec = chainRecNew;
                }
                chainRec.Id = Nodes[0].Id;
                Nodes.Add(chainRec);
                loc = Convert.ToInt32(chainRec.SourceWhNew.Value);
                if (RecExists(Nodes, Nodes[0].Item, loc))
                {
                    break;
                }
            }
            var cnt = Nodes.Count;
            var supplier = Nodes[cnt - 1].SupplierNew;
            if (supplier.State == ValueStates.Valid)
                SetSupplier(/*Nodes[cnt - 1].Seq*/cnt, Convert.ToInt32(supplier.Value),
                            Nodes[cnt - 1].SupplierDescNew.Value.ToString());
            UpdateSequence();
            return true;
        }

        public void UpdateSequence()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].Seq = i + 1;
            }
        }

        public override string ToString()
        {
            return Nodes.Aggregate("", (current, chainRow) => current + chainRow.Loc + " ");
        }
        public override bool Equals(object o)
        {
            var chain = (Chain)o;
            if (Nodes.Count != chain.Nodes.Count || Nodes.Count == 0) return false;
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Loc != chain.Nodes[i].Loc) return false;
            }
            return true;
        }

        public void SetAction(int seq, Actions action)
        {
            switch (action)
            {
                case Actions.Leave:
                    {
                        for (int index = 0; index < Nodes.Count; index++)
                        {
                            var chainRec = Nodes[index];
                            if (chainRec.Seq == seq) break;
                            chainRec.Action = new ChainValue(Actions.Leave);
                            //if ((Actions)chainRec.Action.Value != Actions.Transit) chainRec.Action = new ChainValue(Actions.Leave);
                            chainRec.StatusNew = new ChainValue(1);
                        }
                        Nodes[seq - 1].StatusNew = new ChainValue(1);
                        break;
                    }
                case Actions.Switch:
                    {
                        for (int index = 0; index < Nodes.Count; index++)
                        {
                            var chainRec = Nodes[index];
                            if (chainRec.Seq == seq) break;
                            chainRec.Action = new ChainValue(Actions.Leave);
                            //if ((Actions)chainRec.Action.Value != Actions.Transit) chainRec.Action = new ChainValue(Actions.Leave);
                            chainRec.StatusNew = new ChainValue(1);
                        }
                        Nodes[seq - 1].StatusNew = new ChainValue(0);
                        break;
                    }
                case Actions.Close:
                    {
                        for (int index = 0; index < Nodes.Count; index++)
                        {
                            var chainRec = Nodes[index];
                            if (chainRec.Seq == seq) break;
                            chainRec.Action = new ChainValue(Actions.Close);

                            chainRec.StatusNew = new ChainValue(0);
                        }
                        Nodes[seq - 1].StatusNew = new ChainValue(0);

                        break;
                    }
            }
            Nodes[seq - 1].Action = new ChainValue(action);
        }

        public void Join(Chain chain)
        {
            for (int index = 0; index < Nodes.Count; index++)
            {
                var chainRow = Nodes[index];
                chainRow.Join(chain.Nodes[index]);
            }
        }
        public static void Group(List<Chain> chainGroup)
        {

            for (int i = 0; i < chainGroup.Count; i++)
            {
                for (int j = i + 1; j < chainGroup.Count; j++)
                {
                    if (chainGroup[i] != null || chainGroup[j] != null)
                    {
                        if (chainGroup[i].Equals(chainGroup[j]))
                        {
                            chainGroup[i].Join(chainGroup[j]);
                            chainGroup.Remove(chainGroup[j]);
                            j--;
                        }
                    }
                }
            }
            if (GroupIsEmpty(chainGroup))
            {
                var chain = new ChainRec();
                chain.Default(chainGroup[0].Item, chainGroup[0].Wh);
                chainGroup[0].Nodes = new List<ChainRec> { chain };
            }

            for (int i = 0; i < chainGroup.Count; i++)
            {
                for (int j = 0; j < chainGroup[i].Nodes.Count; j++)
                {
                    chainGroup[i].Nodes[j].Id = i + 1;
                }
            }
        }
        public static bool GroupIsEmpty(List<Chain> chainGroup)
        {
            foreach (var chain in chainGroup)
            {
                if (chain.Nodes.Count > 0) return false;
            }
            return true;
        }
        public static Chain GetChainById(List<Chain> chainGroup, int id)
        {
            for (int i = 0; i < chainGroup.Count; i++)
            {
                if (chainGroup[i].Nodes.Count > 0)
                {
                    if (chainGroup[i].Nodes[0].Id == id) return chainGroup[i];
                }
            }
            return null;
        }
        public static ChainRec FindRec(List<ChainRec> list, string item, int loc)
        {
            foreach (var chainRec in list)
            {
                if (chainRec.Item.Equals(item) && chainRec.Loc == loc)
                {
                    return chainRec;
                }
            }
            return null;
        }
        public static bool RecExists(List<ChainRec> list, string item, int loc)
        {
            foreach (var chainRec in list)
            {
                if (chainRec.Item.Equals(item) && chainRec.Loc == loc)
                {
                    return true;
                }
            }
            return false;
        }
        public bool WhExists(int wh)
        {
            foreach (var chainRec in Nodes)
            {
                if (chainRec.Loc == wh)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
