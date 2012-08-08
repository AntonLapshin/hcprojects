using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using NormManagementMVVM.Model;
using System.Linq;

namespace NormManagementMVVM.ViewModel
{
    public class HeadViewModel : ViewModelBase, ICollection<RowViewModel>
    {
        public Y_NORM_NORMATIVE_HEAD Entity { get; private set; }
        public ObservableCollection<RowViewModel> Rows { get; private set; }
        public static Controllers Controllers { get; private set; }
        public RepresentationViewModel Representation { get; set; }

        public string ProfileName
        {
            get
            {
                return "Профиль: " + Entity.Y_NORM_PROFILE_HEAD;
            }
        }
        public string ProfileContent
        {
            get { return "Параметры: " + GetProfileContent(); }
        }

        public HeadViewModel(Y_NORM_NORMATIVE_HEAD head)
        {
            Entity = head;
            Rows = new ObservableCollection<RowViewModel>();
            Controllers = new Controllers();
            Representation = new RepresentationViewModel(this);

            foreach (var row in head.Y_NORM_NORMATIVE_ROW)
            {
                var r = new RowViewModel(row, this);
                Rows.Add(r);
            }
            Rows.Sort(row => row.SeqNum);

            //RaisePropertyChanged("Representation");
        }

        private string GetProfileContent()
        {
            var result = "";
            foreach (Y_NORM_PROFILE_DETAIL detail in Entity.Y_NORM_PROFILE_HEAD.Y_NORM_PROFILE_DETAIL)
                result = result + GetProfileDetail(detail);
            return result;
        }
        private string GetProfileDetail(Y_NORM_PROFILE_DETAIL profileParam)
        {
            return profileParam.Y_NORM_PARAMETERS.DESC_RU + ": " + GenericRepository.GetParameterNames(profileParam.ID_PARAM, profileParam.VALUE);
        }

        public void CloneRow(RowViewModel row)
        {
            var r = row.Clone() as RowViewModel;
            Rows.Insert(Rows.IndexOf(row), r);
        }

        #region Implementation of IEnumerable

        public IEnumerator<RowViewModel> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<RowViewModel>

        public void Add(RowViewModel item)
        {
            Rows.Add(item);
        }

        public void Clear()
        {
            var cnt = Rows.Count;
            for (int i = 0; i < cnt; i++)
            {
                Rows[0].Clear();
            }
        }

        public bool Contains(RowViewModel item)
        {
            return Rows.Contains(item);
        }

        public void CopyTo(RowViewModel[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(RowViewModel item)
        {
            return Rows.Remove(item);
        }

        public int Count
        {
            get { return Rows.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}