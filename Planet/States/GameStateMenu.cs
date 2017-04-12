using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  interface IMenuGameState
  {
    void Next();
    void Previous();
    void Confirm();
    void Cancel();
  }
}
