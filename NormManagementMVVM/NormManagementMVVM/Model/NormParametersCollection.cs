using System.Collections.Generic;
using System.Linq;


namespace NormManagement.Model
{
    public class NormParametersCollection
    {
        public List<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> ParameterValues { get; set; } 
        public override string ToString()
        {
            return string.Join(",", ParameterValues.Select(y => y.NAME));
        }
    }
}
