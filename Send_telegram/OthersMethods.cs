using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignUI_Send_telegram
{
   public static class OthersMethods
    {
        public static IEnumerable<Control> GetAllControls(this Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>().ToArray();

            return controls.SelectMany(ctrl => GetAllControls(ctrl, type))
                                  .Concat(controls)
                                  .Where(c => c.GetType() == type);
        }
    }
}
