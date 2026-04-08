using UnityEngine;

namespace TD.UI {
    public class HPBar : FillBar {
        public override void SetFill(float val) {
            base.SetFill(val);
            fillImg.color = Color.Lerp(Color.red, Color.green, val);
        }
    } 
}
