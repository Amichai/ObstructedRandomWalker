﻿using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RandomWalkTest
{
    
    
    /// <summary>
    ///This is a test class for VectorTest and is intended
    ///to contain all VectorTest Unit Tests
    ///</summary>
	[TestClass()]
	public class VectorTest {


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for ReflectLine
		///</summary>
		[TestMethod()]
		public void ReflectLineTest() {
			Vector start = null; // TODO: Initialize to an appropriate value
			Vector end = null; // TODO: Initialize to an appropriate value
			LineSegment target = new LineSegment(start, end); // TODO: Initialize to an appropriate value
			LineSegment lineToReflectOver = null; // TODO: Initialize to an appropriate value
			LineSegment expected = null; // TODO: Initialize to an appropriate value
			LineSegment actual;
			actual = target.ReflectOverLineThroughOrigin(lineToReflectOver);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for op_Multiply
		///</summary>
		[TestMethod()]
		public void op_MultiplyTest() {
			double a = 0F; // TODO: Initialize to an appropriate value
			LineSegment b = null; // TODO: Initialize to an appropriate value
			LineSegment expected = null; // TODO: Initialize to an appropriate value
			LineSegment actual;
			actual = (a * b);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for op_Multiply
		///</summary>
		[TestMethod()]
		public void op_MultiplyTest1() {
			LineSegment a = null; // TODO: Initialize to an appropriate value
			LineSegment b = null; // TODO: Initialize to an appropriate value
			double expected = 0F; // TODO: Initialize to an appropriate value
			double actual;
			actual = (a * b);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for op_Subtraction
		///</summary>
		[TestMethod()]
		public void op_SubtractionTest() {
			LineSegment a = new LineSegment(new Vector(3, 2), new Vector(1, 5));
			LineSegment b = new LineSegment(new Vector(1, 2), new Vector(6, 9));
			LineSegment expected = null;
			LineSegment actual;
			actual = (a - b);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}
