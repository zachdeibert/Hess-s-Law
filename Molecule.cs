using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HesssLaw {
	public class Molecule {
		public double Amount;
		public string Name;

		public IEnumerable<Element> Elements {
			get {
				List<string> elements = new List<string>();
				int split = 0;
				for ( int i = 1; i < Name.Length; ++i ) {
					if ( Name[i] >= 'A' && Name[i] <= 'Z' ) {
						elements.Add(Name.Substring(split, i - split));
						split = i;
					}
				}
				elements.Add(Name.Substring(split));
				return elements.Select(str => Element.Parse(str) * Amount);
			}
		}

		public static Molecule Parse(string str) {
			str = str.Replace(" ", "");
			int i = 0;
			for ( i = 0; (str[i] >= '0' && str[i] <= '9') || str[i] == '.'; ++i );
			if ( i > 0 ) {
				return new Molecule(str.Substring(i), double.Parse(str.Substring(0, i)));
			} else {
				return new Molecule(str);
			}
		}

		public static Molecule operator *(Molecule t, double factor) {
			return new Molecule(t.Name, t.Amount * factor);
		}

		public static Molecule operator +(Molecule t, double amt) {
			return new Molecule(t.Name, t.Amount + amt);
		}

		public override bool Equals(object obj) {
			if ( obj is Molecule ) {
				Molecule m = (Molecule) obj;
				return m.Amount == Amount && m.Name == Name;
			} else {
				return false;
			}
		}

		public override string ToString() {
			if ( Amount == 1 ) {
				return Name;
			} else {
				return string.Format("{0}{1}", Amount, Name);
			}
		}

		public Molecule(string name, double amt = 1) {
			Amount = amt;
			Name = name;
		}

		public Molecule() {
		}
	}
}

