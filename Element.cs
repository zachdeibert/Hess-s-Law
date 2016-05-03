using System;

namespace HesssLaw {
	public class Element {
		public string Name;
		public double Amount;

		public static Element Parse(string str) {
			int i = str.Length - 1;
			for ( i = str.Length - 1; str[i] >= '0' && str[i] <= '9'; --i );
			++i;
			if ( i < str.Length ) {
				return new Element(str.Substring(0, i), int.Parse(str.Substring(i)));
			} else {
				return new Element(str);
			}
		}

		public static Element operator *(Element t, double factor) {
			return new Element(t.Name, t.Amount * factor);
		}

		public static Element operator +(Element t, double amt) {
			return new Element(t.Name, t.Amount + amt);
		}

		public override bool Equals(object obj) {
			if ( obj is Element ) {
				Element e = (Element) obj;
				return e.Name == Name && e.Amount == Amount;
			} else {
				return false;
			}
		}

		public override string ToString() {
			return string.Format("{0}{1}", Amount, Name);
		}

		public Element() {
		}

		public Element(string name, double amount = 1) {
			Name = name;
			Amount = amount;
		}
	}
}

