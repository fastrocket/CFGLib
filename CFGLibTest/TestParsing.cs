﻿using CFGLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFGLibTest {
	[TestClass]
	public class TestParsing {
		private static void ExecuteTest(Grammar g, CNFGrammar h, List<Sentence> sentences) {
			foreach (var sentence in sentences) {
				var p1 = h.Cyk(sentence);
				var p2 = g.Earley(sentence);
				Helpers.AssertNear(p1, p2);
			}
		}

		[TestMethod]
		public void TestParsing01() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<X_0> → <X_2> <X_6> [84.663251941866818]"),
				CFGParser.Production("<X_5> → ε [58.744849453561407]"),
				CFGParser.Production("<X_6> → 'x0' [14.931547298064245]"),
				CFGParser.Production("<X_1> → <X_4> 'x3' 'x2' [8.0317742447516771]"),
				CFGParser.Production("<X_2> → <X_6> [31.352648361750251]"),
				CFGParser.Production("<X_5> → 'x2' [70.697925527439423]"),
				CFGParser.Production("<X_4> → ε [95.484905709738328]"),
				CFGParser.Production("<X_2> → <X_2> [1]"),
				CFGParser.Production("<X_2> → <X_1> <X_5> <X_5> <X_1> 'x2' <X_5> <X_0> [76.400903250743127]")
			}, Nonterminal.Of("X_0"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x0 x0"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing02() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<X_0> → <X_1>"),
				CFGParser.Production("<X_1> → 'x1' 'x2'"),
				CFGParser.Production("<X_1> → ε")
			}, Nonterminal.Of("X_0"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x1 x2"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing03() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<S> → <X> 'b'"),
				CFGParser.Production("<S> → 'a'"),
				CFGParser.Production("<X> → <S>"),
				CFGParser.Production("<X> → 'x'")
			}, Nonterminal.Of("S"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("a b b"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing04() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<X_0> → <X_2> 'x2' 'x0'"),
				CFGParser.Production("<X_2> → <X_0>"),
				CFGParser.Production("<X_0> → 'x1'"),
				CFGParser.Production("<X_2> → 'q'")
			}, Nonterminal.Of("X_0"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x1 x2 x0 x2 x0"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing05() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<X_0> → <X_1>"),
				CFGParser.Production("<X_1> → ε"),
				CFGParser.Production("<X_1> → 'x2' <X_0> <X_0> 'x1'"),
				CFGParser.Production("<X_1> → <X_0>"),
			}, Nonterminal.Of("X_0"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x2 x1"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing06() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<A> → <B>"),
				CFGParser.Production("<B> → <A>"),
				CFGParser.Production("<B> → 'x'"),
			}, Nonterminal.Of("A"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing07() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<A> → <B>"),
				CFGParser.Production("<B> → <C>"),
				CFGParser.Production("<C> → <B>"),
				CFGParser.Production("<B> → <A>"),
				CFGParser.Production("<B> → 'x'"),
			}, Nonterminal.Of("A"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing08() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<S> → <A> 'b'"),
				CFGParser.Production("<S> → <AB>"),
				CFGParser.Production("<A> → 'a'"),
				CFGParser.Production("<AB> → 'a' 'b'"),
			}, Nonterminal.Of("S"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("a b"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing09() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<S> → <A> 'b'"),
				CFGParser.Production("<S> → <AB>"),
				CFGParser.Production("<S> → <ABPrime>"),
				CFGParser.Production("<A> → 'a'"),
				CFGParser.Production("<AB> → 'a' 'b'"),
				CFGParser.Production("<ABPrime> → 'a' 'b'"),
			}, Nonterminal.Of("S"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("a b"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing10() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<S> → <A> 'b'"),
				CFGParser.Production("<S> → <B>"),
				CFGParser.Production("<A> → <B>"),
				CFGParser.Production("<B> → <A> 'b'"),
				CFGParser.Production("<A> → ε"),
			}, Nonterminal.Of("S"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("b b"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing11() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<X_0> → <X_1>"),
				CFGParser.Production("<X_1> → ε"),
				CFGParser.Production("<X_1> → 'x2' <X_0> <X_0> 'x1'"),
			}, Nonterminal.Of("X_0"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x2 x2 x1 x1"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing12() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<A> → <B>"),
				CFGParser.Production("<B> → <A>"),
				CFGParser.Production("<B> → 'x'"),
				CFGParser.Production("<B> → 'z'"),
			}, Nonterminal.Of("A"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing13() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<S> → <A>"),
				CFGParser.Production("<A> → <A> 'x' <A>"),
				CFGParser.Production("<A> → ε")
			}, Nonterminal.Of("S"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x x"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing14() {
			var g = new Grammar(new List<Production>{
				CFGParser.Production("<S> → ε"),
				CFGParser.Production("<S> → <A> 'x'"),
				CFGParser.Production("<A> → <S>"),
			}, Nonterminal.Of("S"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing15() {
			// S -> aSa | bSb | ε
			var g = new Grammar(new List<Production> {
				CFGParser.Production(@"<S> -> 'a' <S> 'a'"),
				CFGParser.Production(@"<S> -> 'b' <S> 'b'"),
				CFGParser.Production(@"<S> -> ε"),
			}, Nonterminal.Of("S"));

			var sentences = new List<Sentence> {
				Sentence.FromLetters("ab"),
				Sentence.FromLetters("abc"),
				Sentence.FromLetters("aaa"),
				Sentence.FromLetters("abbba"),
				Sentence.FromLetters(""),
				Sentence.FromLetters("aa"),
				Sentence.FromLetters("bb"),
				Sentence.FromLetters("abba"),
				Sentence.FromLetters("baab"),
				Sentence.FromLetters("aaaa"),
				Sentence.FromLetters("bbbb"),
				Sentence.FromLetters("aaabbabbabbaaa"),
			};
			ExecuteTest(g, g.ToCNF(), sentences);
		}

		[TestMethod]
		public void TestParsing16() {
			var g = new Grammar(new List<Production> {
				CFGParser.Production("<X_0> -> <X_0> <X_1>"),
				CFGParser.Production("<X_2> -> 'b'"),
				CFGParser.Production("<X_0> -> 'b'"),
				CFGParser.Production("<X_0> -> 'a'"),
				CFGParser.Production("<X_2> -> 'a'"),
			}, Nonterminal.Of("X_0"));

			var sentences = new List<Sentence>();
			sentences.Add(Sentence.FromWords("x"));
			sentences.Add(Sentence.FromLetters("abc"));
			sentences.Add(Sentence.FromLetters("ab"));
			sentences.Add(Sentence.FromLetters("ba"));
			sentences.Add(Sentence.FromLetters("aabb"));
			sentences.Add(Sentence.FromLetters("abba"));
			sentences.Add(Sentence.FromLetters("a"));
			sentences.Add(Sentence.FromLetters("b"));
			ExecuteTest(g, g.ToCNF(), sentences);
		}
	}
}