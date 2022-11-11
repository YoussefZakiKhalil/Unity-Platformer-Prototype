using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUAS.MMT
{

    public interface ICommandU
    {

        void Execute();
        void Undo();

    }

}
