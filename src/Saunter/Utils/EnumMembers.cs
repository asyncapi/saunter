using System;
using System.Collections;

namespace Saunter.Utils
{
    internal class EnumMembers
    {
        public Type MemberType { get; set; }
        public IList Members { get; set; }

        internal EnumMembers(Type memberType, IList members)
        {
            MemberType = memberType;
            Members = members;
        }
    }
}