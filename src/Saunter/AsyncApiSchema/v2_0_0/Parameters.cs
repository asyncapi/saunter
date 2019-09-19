using System.Collections.Generic;
using Saunter.AsyncApiSchema.Utils;

namespace Saunter.AsyncApiSchema.v2_0_0 {
    public class Parameters : Dictionary<ParametersFieldName, OneOf<Parameter, Reference>> { }
}