using System;
using System.Collections.Generic;
using System.Linq;

namespace HesssLaw {
	public class ChemicalEquation {
		public ChemicalExpression Reactants;
		public ChemicalExpression Products;
		public double Enthalpy;

		public bool IsBalanced {
			get {
				List<Element> reactants = new List<Element>();
				List<Element> products = new List<Element>();
				foreach ( Element reactant in Reactants.Elements ) {
					Element old = reactants.FirstOrDefault(e => e.Name.Equals(reactant.Name));
					if ( old == null ) {
						reactants.Add(reactant);
					} else {
						reactants.Remove(old);
						reactants.Add(old + reactant.Amount);
					}
				}
				foreach ( Element product in Products.Elements ) {
					Element old = products.FirstOrDefault(e => e.Name.Equals(product.Name));
					if ( old == null ) {
						products.Add(product);
					} else {
						products.Remove(old);
						products.Add(old + product.Amount);
					}
				}
				return reactants.Count == products.Count &&
					reactants.All(e => products.Any(e2 => e.Equals(e2)));
			}
		}

		public static ChemicalEquation Parse(string str, double enthalpy = 0) {
			string[] parts = str.Split(new string[] {
				"->",
				"=>"
			}, 2, StringSplitOptions.None);
			ChemicalEquation eq = new ChemicalEquation(ChemicalExpression.Parse(parts[0]), ChemicalExpression.Parse(parts[1]), enthalpy);
			if ( !eq.IsBalanced ) {
				throw new FormatException("Equation was not balanced!");
			}
			return eq;
		}

		public static ChemicalEquation operator *(ChemicalEquation t, double factor) {
			ChemicalEquation r;
			if ( factor < 0 ) {
				r = new ChemicalEquation(t.Products, t.Reactants, -t.Enthalpy) * -factor;
			} else {
				r = new ChemicalEquation(t.Reactants * factor, t.Products * factor, t.Enthalpy * factor);
			}
#if DEBUG
			Console.WriteLine("{0} * {1} = {2}", t, factor, r);
#endif
			return r;
		}

		public static ChemicalEquation operator +(ChemicalEquation a, ChemicalEquation b) {
			ChemicalEquation r = new ChemicalEquation(a.Reactants + b.Reactants, a.Products + b.Products, a.Enthalpy + b.Enthalpy);
#if DEBUG
			Console.WriteLine("{0} + {1} = {2}", a, b, r);
#endif
			return r;
		}

		public void Normalize() {
			ChemicalExpression net = new ChemicalExpression();
			foreach ( Molecule mol in Reactants ) {
				net += mol;
			}
			foreach ( Molecule mol in Products ) {
				net += mol * -1;
			}
			Reactants.Clear();
			Products.Clear();
			net.RemoveAll(mol => mol.Amount == 0);
			foreach ( Molecule mol in net ) {
				if ( mol.Amount > 0 ) {
					Reactants += mol;
				} else {
					Products += mol * -1;
				}
			}
		}

		public override bool Equals(object obj) {
			if ( obj is ChemicalEquation ) {
				ChemicalEquation e = (ChemicalEquation) obj;
				if ( Products.Count < 1 ) {
					return false;
				}
				Molecule match = e.Products.FirstOrDefault(mol => mol.Name.Equals(Products[0].Name));
				if ( match == null ) {
					return false;
				}
				double factor = Products[0].Amount / match.Amount;
				return e.Products.Count == Products.Count && e.Reactants.Count == Reactants.Count &&
					e.Products.All(mol => Products.Contains(mol * factor)) &&
					e.Reactants.All(mol => Reactants.Contains(mol * factor));
			} else {
				return false;
			}
		}

		public override string ToString() {
			return string.Format("{0} -> {1} (ΔH: {2})", Reactants, Products, Enthalpy);
		}

		public ChemicalEquation(ChemicalExpression reactants, ChemicalExpression products, double enthalpy) {
			Reactants = reactants;
			Products = products;
			Enthalpy = enthalpy;
		}

		public ChemicalEquation() {
			Products = new ChemicalExpression();
			Reactants = new ChemicalExpression();
		}
	}
}

