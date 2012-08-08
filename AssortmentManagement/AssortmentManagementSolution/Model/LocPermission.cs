using AssortmentManagement.UserValues;

namespace AssortmentManagement.Model
{
    public class LocPermission
    {
        public int Loc { get; private set; }
        public LocPermissionTypes Permission { get; private set; }

        public override string ToString()
        {
            return Loc.ToString();
        }

        public LocPermission(int loc, LocPermissionTypes permission)
        {
            Loc = loc;
            Permission = permission;
        }
    }
}
