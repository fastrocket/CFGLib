﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFGLib {
	public class GrammarGenerator {
		private Random _rand = new Random(0);

		public Grammar NextCFG(int numNonterminals, int numProductions, int maxProductionLength, IList<Terminal> terminals) {
			if (numNonterminals < 1) {
				throw new ArgumentOutOfRangeException("Need at least one nonterminal");
			}
			var start = RandomNonterminal(1);

			var productions = new List<BaseProduction>();

			for (int i = 0; i < numProductions; i++) {
				productions.Add(RandomProduction(maxProductionLength, numNonterminals, terminals));
			}

			return new Grammar(productions, start);
		}

		private BaseProduction RandomProduction(int maxProductionLength, int numNonterminals, IList<Terminal> terminals) {
			var lhs = RandomNonterminal(numNonterminals);
			var weight = _rand.Next(100) + 1;
			var productionLength = _rand.Next(maxProductionLength + 1);
			Sentence rhs = new Sentence();
			for (int i = 0; i < productionLength; i++) {
				if (_rand.Next(2) == 0) {
					rhs.Add(RandomNonterminal(numNonterminals));
				} else {
					rhs.Add(RandomTerminal(terminals));
				}
			}

			return new Production(lhs, rhs, weight);
		}

		public CNFGrammar NextCNF(int numNonterminals, int numProductions, IList<Terminal> terminals) {
			if (numNonterminals < 1) {
				throw new ArgumentOutOfRangeException("Need at least one nonterminal");
			}
			var start = RandomNonterminal(1);
			int producesEmptyWeight = 0;
			if (numProductions > 0) {
				if (_rand.Next(2) == 1) {
					producesEmptyWeight = _rand.Next(100);
					numProductions--;
				}
			}
			var numNontermProductions = _rand.Next(numProductions);
			var numTermProductions = numProductions - numNontermProductions;
			var nt = new List<CNFNonterminalProduction>();
			var t = new List<CNFTerminalProduction>();

			for (int i = 0; i < numNontermProductions; i++) {
				nt.Add(RandomNTProduction(numNonterminals));
			}
			for (int i = 0; i < numTermProductions; i++) {
				var terminal = RandomTerminal(terminals);
				t.Add(RandomTProduction(numNonterminals, terminals, terminal));
			}
			return new CNFGrammar(nt, t, producesEmptyWeight, start);
		}

		private CNFNonterminalProduction RandomNTProduction(int numNonTerminals, Nonterminal lhs = null) {
			if (lhs == null) {
				lhs = RandomNonterminal(numNonTerminals);
			}
			var rhs1 = RandomNonterminal(numNonTerminals, false);
			var rhs2 = RandomNonterminal(numNonTerminals, false);

			return new CNFNonterminalProduction(lhs, rhs1, rhs2);
		}

		private CNFTerminalProduction RandomTProduction(int numNonterminals, IList<Terminal> terminals, Terminal rhs = null) {
			if (rhs == null) {
				rhs = RandomTerminal(terminals);
			}
			var lhs = RandomNonterminal(numNonterminals);

			return new CNFTerminalProduction(lhs, rhs);
		}

		private Nonterminal RandomNonterminal(int numNonterminals, bool allowStart = true) {
			int num;
			
			if (allowStart) {
				num = _rand.Next(0, numNonterminals);
			} else {
				num = _rand.Next(1, numNonterminals);
			}
			return Nonterminal.Of("X_" + num);
		}
		private Terminal RandomTerminal(IList<Terminal> terminals) {
			return terminals[_rand.Next(terminals.Count)];
		}
	}
}