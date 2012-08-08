using DevExpress.XtraPivotGrid.Localization;

namespace SharedComponents.Localization
{
    public class DXPivotGridLocalizerRU : PivotGridLocalizer
    {
        public override string GetLocalizedString(PivotGridStringId id)
        {
            switch (id)
            {
                case PivotGridStringId.GrandTotal:
                    return "Главный итог";
                case PivotGridStringId.TotalFormat: //косяк
                    return "Итог {0}";
                case PivotGridStringId.PopupMenuClearSorting:
                    return "Убрать сортировку";
                case PivotGridStringId.PopupMenuHideField:
                    return "Скрыть";
                case PivotGridStringId.PopupMenuHidePrefilter:
                    return "Скрыть фильтр";
                case PivotGridStringId.PopupMenuRefreshData:
                    return "Обновить";
                case PivotGridStringId.SearchBoxText:
                    return "Поиск";
                case PivotGridStringId.CustomizationFormHiddenFields:
                    return "Скрытые поля";
                case PivotGridStringId.EditPrefilter:
                    return "Редактировать фильтр";
                case PivotGridStringId.PopupMenuFieldOrder:
                    return "Порядок столбцов";
                case PivotGridStringId.PopupMenuShowPrefilter:
                    return "Показать фильтр";
                case PivotGridStringId.PrefilterFormCaption:
                    return "Фильтр";
                case PivotGridStringId.PopupMenuSortFieldByColumn:
                    return "Сортировать {0} по этому столбцу";
                case PivotGridStringId.PopupMenuCollapseAll:
                    return "Свернуть всё";
                case PivotGridStringId.PopupMenuExpandAll:
                    return "Развернуть всё";
                case PivotGridStringId.PopupMenuCollapse:
                    return "Свернуть элемент";
                case PivotGridStringId.PopupMenuExpand:
                    return "Развернуть элемент";
                case PivotGridStringId.PopupMenuSortFieldByRow:
                    return "Сортировать {0} по этой строке";
                case PivotGridStringId.PopupMenuSortAscending:
                    return "Сортировать А-Я";
                case PivotGridStringId.PopupMenuSortDescending:
                    return "Сортировать Я-А";
                case PivotGridStringId.PopupMenuShowFieldList:
                    return "Показать список полей";
                case PivotGridStringId.PopupMenuHideFieldList:
                    return "Скрыть список полей";
                case PivotGridStringId.PopupMenuMovetoBeginning:
                    return "Переместить в начало";
                case PivotGridStringId.PopupMenuMovetoEnd:
                    return "Переместить в конец";
                case PivotGridStringId.PopupMenuMovetoLeft:
                    return "Переместить влево";
                case PivotGridStringId.PopupMenuMovetoRight:
                    return "Переместить вправо";
                case PivotGridStringId.FilterOk:
                    return "Принять";
                case PivotGridStringId.FilterCancel:
                    return "Отмена";
                case PivotGridStringId.CustomizationFormCaption:
                    return "Список полей";
                case PivotGridStringId.RowArea:
                    return "Область строк";
                case PivotGridStringId.DataHeadersCustomization:
                    return "Область данных";
                case PivotGridStringId.ColumnArea:
                    return "Область столбцов";
                case PivotGridStringId.ColumnHeadersCustomization:
                    return "Область колонок";
                case PivotGridStringId.RowHeadersCustomization:
                    return "Область строк";
                case PivotGridStringId.FilterHeadersCustomization:
                    return "Область фильтров";
                default:
                    return base.GetLocalizedString(id);
            }
        }
    }
}