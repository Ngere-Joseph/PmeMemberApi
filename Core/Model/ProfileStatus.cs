using System.ComponentModel;

namespace PmeMemberApi.Core.Model
{
    public enum ProfileStatus
    {
        [Description("All")]
        All = -1,
        [Description("Active")]
        Active = 1,
        [Description("Blocked")]
        Blocked = 2,
        [Description("Deleted")]
        Deleted = 3,
        [Description("AdminDeleted")]
        AdminDeleted = 4,
    }
}
