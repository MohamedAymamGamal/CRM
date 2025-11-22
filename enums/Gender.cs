using System.Runtime.Serialization;

namespace CRM.API.enums
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male,
        [EnumMember(Value = "Female")]
        Female,
    }
}