using System;
using System.Collections.Generic;

public class DocumentFilterContext
{
    public DocumentFilterContext(IEnumerable<Type> asyncApiTypes)
    {
        AsyncApiTypes = asyncApiTypes;
    }

    public IEnumerable<Type> AsyncApiTypes { get; }
}
