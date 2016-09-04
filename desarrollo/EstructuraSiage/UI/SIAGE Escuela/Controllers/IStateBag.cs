using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAGE_Escuela.Controllers
{
    public interface IStateBag
    {
        object this[StateBagKeys key] { get; set; }
        void RemoveAllExcept(List<string> keysNoBorrar);
    }
    public enum StateBagKeys
    {
        Docentes
    }
}