using UnityEngine;
using UnityEngine.UI;

namespace TD.UI {
	public class FillBar : MonoBehaviour {
		[SerializeField] protected Image fillImg;

		public virtual void SetFill(float val) {
			fillImg.fillAmount = val;
		}
	} 
}
