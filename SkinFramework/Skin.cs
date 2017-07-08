using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinFramework
{
    public abstract class Skin
    {
        protected Skin()
        {
            
        }


        public abstract SkinDefinition Load();

        public virtual void ApplySkin(SkinWindow window)
        {
            
        }

        public virtual void PaintSkin(SkinWindow window)
        {
            
        }

    }
}
