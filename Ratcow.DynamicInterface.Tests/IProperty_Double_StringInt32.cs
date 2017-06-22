using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratcow.DynamicInterface.Tests
{
    /// <summary>
    /// This interface adds nothing, but is testing composition of 
    /// multiple instances using the code from previous single tests.
    /// </summary>
    public interface IProperty_Double_StringInt32: IProperty_Single_String, IProperty_Single_Int32
    {
    }
}
