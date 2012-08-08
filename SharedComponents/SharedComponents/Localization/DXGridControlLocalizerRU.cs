using DevExpress.Xpf.Grid;

namespace SharedComponents.Localization
{
    public class DXGridControlLocalizerRU : GridControlLocalizer
    {
        public override string GetLocalizedString(GridControlStringId id)
        {
            switch (id)
            {
                case GridControlStringId.GridGroupPanelText:
                    return string.Empty;
                default:
                    return base.GetLocalizedString(id);
            }
        }
    }
}
