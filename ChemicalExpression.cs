using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HesssLaw {
	public class ChemicalExpression : List<Molecule> {
		public IEnumerable<Element> Elements {
			get {
				return this.SelectMany(mol => mol.Elements);
			}
		}

		public static ChemicalExpression Parse(string str) {
			return new ChemicalExpression(str.Split('+').Select(s => Molecule.Parse(s)));
		}

		public static ChemicalExpression operator *(ChemicalExpression t, double factor) {
			return new ChemicalExpression(t.Select(mol => new Molecule(mol.Name, mol.Amount * factor)));
		}

		public static ChemicalExpression operator +(ChemicalExpression t, ChemicalExpression exp) {
			ChemicalExpression sum = new ChemicalExpression();
			sum.AddRange(t);
			foreach ( Molecule mol in exp ) {
				Molecule our = sum.Where(m => m.Name.Equals(mol.Name)).FirstOrDefault();
				if ( our == null ) {
					sum.Add(mol);
				} else {
					sum.Remove(our);
					sum.Add(our + mol.Amount);
				}
			}
#if DEBUG
			Console.WriteLine("{0} + {1} = {2}", t, exp, sum);
#endif
			return sum;
		}

		public static ChemicalExpression operator +(ChemicalExpression t, Molecule mol) {
			ChemicalExpression exp = new ChemicalExpression();
			exp.Add(mol);
			return t + exp;
		}

		public override bool Equals(object obj) {
			if ( obj is ChemicalExpression ) {
				ChemicalExpression e = (ChemicalExpression) obj;
				return e.All(mol => this.Any(m => m.Equals(mol))) && e.Count == Count;
			} else {
				return false;
			}
		}

		public override string ToString() {
			StringBuilder str = new StringBuilder();
			foreach ( Molecule mol in this.Where(m => m.Amount != 0) ) {
				if ( str.Length > 0 ) {
					str.Append(" + ");
				}
				str.Append(mol);
			}
			return str.ToString();
		}

		public ChemicalExpression(IEnumerable<Molecule> mols) {
			AddRange(mols);
		}

		public ChemicalExpression(params Molecule[] mols) {
			AddRange(mols);
		}

		public ChemicalExpression() {
		}
	}
}

